using System.Collections.Generic;
using System.Linq;
using HearthStone.Database.Enums;
using HearthStone.DeckTracker.Hearthstone.Entities;


namespace HearthStone.DeckTracker.Hearthstone.Secrets
{
	public abstract class SecretsEventHandler
	{
		private const int AvengeDelay = 50;
		private int _avengeDeathRattleCount;
		private bool _awaitingAvenge;
		private int _lastCompetitiveSpiritCheck;
		private HashSet<Entity> EntititesInHandOnMinionsPlayed = new HashSet<Entity>();

		private bool FreeSpaceOnBoard => Game.OpponentMinionCount < 7;
		private bool FreeSpaceInHand => Game.OpponentHandCount < 10;
		private bool HandleAction => HasActiveSecrets && Config.Instance.AutoGrayoutSecrets;
		private bool IsAnyMinionInOpponentsHand => EntititesInHandOnMinionsPlayed.Any(entity => entity.IsMinion);

		public List<Secret> Secrets { get; } = new List<Secret>();

		protected abstract IGame Game { get; }
		protected abstract bool HasActiveSecrets { get; }
		public abstract bool Exclude(string cardId, bool invokeCallback = true);
		public abstract void Exclude(List<string> cardIds);

		public virtual void Reset()
		{
			_avengeDeathRattleCount = 0;
			_awaitingAvenge = false;
			_lastCompetitiveSpiritCheck = 0;
			EntititesInHandOnMinionsPlayed.Clear();
		}

		public void HandleAttack(Entity attacker, Entity defender, bool fastOnly = false)
		{
			if(!HandleAction)
				return;

			if(attacker.GetTag(GameTag.CONTROLLER) == defender.GetTag(GameTag.CONTROLLER))
				return;

			var exclude = new List<string>();

			var freeSpaceOnBoard = FreeSpaceOnBoard;
			if(freeSpaceOnBoard)
				exclude.Add(CardIds.Secrets.Paladin.NobleSacrifice);

			if(defender.IsHero)
			{
				if(!fastOnly)
				{
					if(freeSpaceOnBoard)
						exclude.Add(CardIds.Secrets.Hunter.BearTrap);
					exclude.Add(CardIds.Secrets.Mage.IceBarrier);
				}

				if(freeSpaceOnBoard)
					exclude.Add(CardIds.Secrets.Hunter.WanderingMonster);

				exclude.Add(CardIds.Secrets.Hunter.ExplosiveTrap);

				if(Game.IsMinionInPlay)
					exclude.Add(CardIds.Secrets.Hunter.Misdirection);

				if(attacker.IsMinion && Game.PlayerMinionCount > 1)
					exclude.Add(CardIds.Secrets.Rogue.SuddenBetrayal);

				if(attacker.IsMinion)
				{
					exclude.Add(CardIds.Secrets.Mage.Vaporize);
					exclude.Add(CardIds.Secrets.Hunter.FreezingTrap);
				}
			}
			else
			{
				if(!fastOnly && freeSpaceOnBoard)
				{
					exclude.Add(CardIds.Secrets.Hunter.SnakeTrap);
					exclude.Add(CardIds.Secrets.Hunter.VenomstrikeTrap);
				}

				if(attacker.IsMinion)
					exclude.Add(CardIds.Secrets.Hunter.FreezingTrap);
			}
			Exclude(exclude);
		}

		/// <summary>
		/// see http://hearthstone.gamepedia.com/Advanced_rulebook#Combat for fast vs. slow secrets
		/// a few fast secrets can modify combat
		/// freezing trap and vaporize remove the attacking minion
		/// misdirection, noble sacrifice change the target
		/// if multiple secrets are in play and a fast secret triggers,
		/// we need to eliminate older secrets which would have been triggered by the attempted combat
		/// </summary>
		public void HandleFastCombat(Entity entity)
		{
			if(!HandleAction)
				return;
			if(!entity.HasCardId || Game.ProposedAttacker == 0 || Game.ProposedDefender == 0)
				return;
			if(!CardIds.Secrets.FastCombat.Contains(entity.CardId))
				return;
			if(Game.Entities.TryGetValue(Game.ProposedAttacker, out var attacker)
				&& Game.Entities.TryGetValue(Game.ProposedDefender, out var defender))
				HandleAttack(attacker, defender, true);
		}

		public void HandleMinionPlayed()
		{
			if(!HandleAction)
				return;

			var exclude = new List<string>();

			exclude.Add(CardIds.Secrets.Hunter.Snipe);
			exclude.Add(CardIds.Secrets.Mage.ExplosiveRunes);
			exclude.Add(CardIds.Secrets.Mage.PotionOfPolymorph);
			exclude.Add(CardIds.Secrets.Paladin.Repentance);

			if(FreeSpaceOnBoard)
				exclude.Add(CardIds.Secrets.Mage.MirrorEntity);

			if(FreeSpaceInHand)
				exclude.Add(CardIds.Secrets.Mage.FrozenClone);

			//Hidden cache will only trigger if the opponent has a minion in hand. 
			//We might not know this for certain - requires additional tracking logic.
			var cardsInOpponentsHand = Game.Entities.Select(kvp => kvp.Value).Where(e => e.IsInHand && e.IsControlledBy(Game.Opponent.Id)).ToList();
			foreach (var cardInOpponentsHand in cardsInOpponentsHand)
			{
				EntititesInHandOnMinionsPlayed.Add(cardInOpponentsHand);
			}

			if (IsAnyMinionInOpponentsHand)
			{
				exclude.Add(CardIds.Secrets.Hunter.HiddenCache);
			}

			Exclude(exclude);
		}

		public void HandleMinionDeath(Entity entity)
		{
			if(!HandleAction)
				return;

			var exclude = new List<string>();

			if(FreeSpaceInHand)
			{
				exclude.Add(CardIds.Secrets.Mage.Duplicate);
				exclude.Add(CardIds.Secrets.Paladin.GetawayKodo);
				exclude.Add(CardIds.Secrets.Rogue.CheatDeath);
			}

			var numDeathrattleMinions = 0;
			if(entity.IsActiveDeathrattle)
			{
				if(!CardIds.DeathrattleSummonCardIds.TryGetValue(entity.CardId ?? "", out numDeathrattleMinions))
				{
					if(entity.CardId == HearthStone.Database.CardIds.Collectible.Neutral.Stalagg
						&& Game.Opponent.Graveyard.Any(x => x.CardId == HearthStone.Database.CardIds.Collectible.Neutral.Feugen)
						|| entity.CardId == HearthStone.Database.CardIds.Collectible.Neutral.Feugen
						&& Game.Opponent.Graveyard.Any(x => x.CardId == HearthStone.Database.CardIds.Collectible.Neutral.Stalagg))
						numDeathrattleMinions = 1;
				}
				if(Game.Entities.Any(x => x.Value.CardId == HearthStone.Database.CardIds.NonCollectible.Druid.SouloftheForest_SoulOfTheForestEnchantment
										&& x.Value.GetTag(GameTag.ATTACHED) == entity.Id))
					numDeathrattleMinions++;
				if(Game.Entities.Any(x => x.Value.CardId == HearthStone.Database.CardIds.NonCollectible.Shaman.AncestralSpirit_AncestralSpiritEnchantment
										&& x.Value.GetTag(GameTag.ATTACHED) == entity.Id))
					numDeathrattleMinions++;
			}

			if(Game.OpponentEntity != null && Game.OpponentEntity.HasTag(GameTag.EXTRA_DEATHRATTLES))
				numDeathrattleMinions *= Game.OpponentEntity.GetTag(GameTag.EXTRA_DEATHRATTLES) + 1;

			HandleAvengeAsync(numDeathrattleMinions);

			// redemption never triggers if a deathrattle effect fills up the board
			// effigy can trigger ahead of the deathrattle effect, but only if effigy was played before the deathrattle minion
			if(Game.OpponentMinionCount < 7 - numDeathrattleMinions)
				exclude.Add(CardIds.Secrets.Paladin.Redemption);

			// TODO: break ties when Effigy + Deathrattle played on the same turn
			exclude.Add(CardIds.Secrets.Mage.Effigy);

			Exclude(exclude);
		}

		public async void HandleAvengeAsync(int deathRattleCount)
		{
			if(!HandleAction)
				return;
			_avengeDeathRattleCount += deathRattleCount;
			if(_awaitingAvenge)
				return;
			_awaitingAvenge = true;
			if(Game.OpponentMinionCount != 0)
			{
				await Game.GameTime.WaitForDuration(AvengeDelay);
				if(Game.OpponentMinionCount - _avengeDeathRattleCount > 0)
					Exclude(CardIds.Secrets.Paladin.Avenge);
			}
			_awaitingAvenge = false;
			_avengeDeathRattleCount = 0;
		}

		public void HandleOpponentDamage(Entity entity)
		{
			if(!HandleAction)
				return;
			if(entity.IsHero && entity.IsControlledBy(Game.Opponent.Id))
			{
				Exclude(CardIds.Secrets.Paladin.EyeForAnEye);
				Exclude(CardIds.Secrets.Rogue.Evasion);
			}
		}

		public void HandleTurnsInPlayChange(Entity entity, int turn)
		{
			if(!HandleAction)
				return;
			if(turn <= _lastCompetitiveSpiritCheck || !entity.IsMinion
				|| !entity.IsControlledBy(Game.Opponent.Id) || !Game.OpponentEntity.IsCurrentPlayer)
				return;
			_lastCompetitiveSpiritCheck = turn;
			Exclude(CardIds.Secrets.Paladin.CompetitiveSpirit);
		}

		public async void HandleCardPlayed(Entity entity)
		{
			if(!HandleAction)
				return;

			var exclude = new List<string>();

			if(entity.IsSpell)
			{
				exclude.Add(CardIds.Secrets.Mage.Counterspell);

				if(Game.OpponentHandCount < 10)
					exclude.Add(CardIds.Secrets.Mage.ManaBind);

				if(Game.OpponentMinionCount < 7)
				{
					//CARD_TARGET is set after ZONE, wait for 50ms gametime before checking
					await Game.GameTime.WaitForDuration(50);
					if(entity.HasTag(GameTag.CARD_TARGET)
						&& Game.Entities.TryGetValue(entity.GetTag(GameTag.CARD_TARGET), out Entity target) && target.IsMinion)
						exclude.Add(CardIds.Secrets.Mage.Spellbender);
					exclude.Add(CardIds.Secrets.Hunter.CatTrick);
				}
			}
			else if(entity.IsMinion && Game.PlayerMinionCount > 3)
				exclude.Add(CardIds.Secrets.Paladin.SacredTrial);
			Exclude(exclude);
		}

		public void HandleHeroPower()
		{
			if(!HandleAction)
				return;
			Exclude(CardIds.Secrets.Hunter.DartTrap);
		}

		public void OnEntityRevealedAsMinion(Entity entity)
		{
			if (EntititesInHandOnMinionsPlayed.Contains(entity) && entity.IsMinion)
				Exclude(CardIds.Secrets.Hunter.HiddenCache);
		}

		public void OnNewSecret(Secret secret)
		{
			if (secret.Entity.IsClass(CardClass.HUNTER))
				EntititesInHandOnMinionsPlayed.Clear();
		}
	}
}
