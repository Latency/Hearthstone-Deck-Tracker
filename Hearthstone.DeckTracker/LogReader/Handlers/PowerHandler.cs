#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HearthStone.Database.Enums;
using HearthStone.DeckTracker.Hearthstone;
using HearthStone.DeckTracker.Hearthstone.Entities;
using HearthStone.DeckTracker.LogReader.Interfaces;
using HearthStone.DeckTracker.Utility.Logging;
using CardIds = HearthStone.Database.CardIds;

#endregion

namespace HearthStone.DeckTracker.LogReader.Handlers
{
	public class PowerHandler
	{
		private readonly TagChangeHandler _tagChangeHandler = new TagChangeHandler();
		private readonly List<Entity> _tmpEntities = new List<Entity>();

		public void Handle(string logLine, IHsGameState gameState, IGame game)
		{
			var creationTag = false;
			if(LogConstants.PowerTaskList.GameEntityRegex.IsMatch(logLine))
			{
				var match = LogConstants.PowerTaskList.GameEntityRegex.Match(logLine);
				var id = int.Parse(match.Groups["id"].Value);
				if(!game.Entities.ContainsKey(id))
					game.Entities.Add(id, new Entity(id) {Name = "GameEntity"});
				gameState.SetCurrentEntity(id);
				if(gameState.DeterminedPlayers)
					_tagChangeHandler.InvokeQueuedActions(game);
				return;
			}
			if(LogConstants.PowerTaskList.PlayerEntityRegex.IsMatch(logLine))
			{
				var match = LogConstants.PowerTaskList.PlayerEntityRegex.Match(logLine);
				var id = int.Parse(match.Groups["id"].Value);
				if(!game.Entities.ContainsKey(id))
					game.Entities.Add(id, new Entity(id));
				if(gameState.WasInProgress)
					game.Entities[id].Name = game.GetStoredPlayerName(id);
				gameState.SetCurrentEntity(id);
				if(gameState.DeterminedPlayers)
					_tagChangeHandler.InvokeQueuedActions(game);
				return;
			}
			if(LogConstants.PowerTaskList.TagChangeRegex.IsMatch(logLine))
			{
				var match = LogConstants.PowerTaskList.TagChangeRegex.Match(logLine);
				var rawEntity = match.Groups["entity"].Value.Replace("UNKNOWN ENTITY ", "");
				if(rawEntity.StartsWith("[") && LogConstants.PowerTaskList.EntityRegex.IsMatch(rawEntity))
				{
					var entity = LogConstants.PowerTaskList.EntityRegex.Match(rawEntity);
					var id = int.Parse(entity.Groups["id"].Value);
					_tagChangeHandler.TagChange(gameState, match.Groups["tag"].Value, id, match.Groups["value"].Value, game);
				}
				else if(int.TryParse(rawEntity, out int entityId))
					_tagChangeHandler.TagChange(gameState, match.Groups["tag"].Value, entityId, match.Groups["value"].Value, game);
				else
				{
					var entity = game.Entities.FirstOrDefault(x => x.Value.Name == rawEntity);

					if(entity.Value == null)
					{
						var players = game.Entities.Where(x => x.Value.HasTag(GameTag.PLAYER_ID)).Take(2).ToList();
						var unnamedPlayers = players.Where(x => string.IsNullOrEmpty(x.Value.Name)).ToList();
						var unknownHumanPlayer = players.FirstOrDefault(x => x.Value.Name == "UNKNOWN HUMAN PLAYER");
						if(unnamedPlayers.Count == 0 && unknownHumanPlayer.Value != null)
						{
							Log.Info("Updating UNKNOWN HUMAN PLAYER");
							entity = unknownHumanPlayer;
						}

						//while the id is unknown, store in tmp entities
						var tmpEntity = _tmpEntities.FirstOrDefault(x => x.Name == rawEntity);
						if(tmpEntity == null)
						{
							tmpEntity = new Entity(_tmpEntities.Count + 1) { Name = rawEntity };
							_tmpEntities.Add(tmpEntity);
						}
						Enum.TryParse(match.Groups["tag"].Value, out GameTag tag);
						var value = GameTagHelper.ParseTag(tag, match.Groups["value"].Value);
						if(unnamedPlayers.Count == 1)
							entity = unnamedPlayers.Single();
						else if(unnamedPlayers.Count == 2 && tag == GameTag.CURRENT_PLAYER && value == 0)
							entity = game.Entities.FirstOrDefault(x => x.Value?.HasTag(GameTag.CURRENT_PLAYER) ?? false);
						if(entity.Value != null)
						{
							entity.Value.Name = tmpEntity.Name;
							foreach(var t in tmpEntity.Tags)
								_tagChangeHandler.TagChange(gameState, t.Key, entity.Key, t.Value, game);
							_tmpEntities.Remove(tmpEntity);
							_tagChangeHandler.TagChange(gameState, match.Groups["tag"].Value, entity.Key, match.Groups["value"].Value, game);
						}
						if(_tmpEntities.Contains(tmpEntity))
						{
							tmpEntity.SetTag(tag, value);
							var player = game.Player.Name == tmpEntity.Name ? game.Player
										: (game.Opponent.Name == tmpEntity.Name ? game.Opponent : null);
							if(player != null)
							{
								var playerEntity = game.Entities.FirstOrDefault(x => x.Value.GetTag(GameTag.PLAYER_ID) == player.Id).Value;
								if(playerEntity != null)
								{
									playerEntity.Name = tmpEntity.Name;
									foreach(var t in tmpEntity.Tags)
										_tagChangeHandler.TagChange(gameState, t.Key, playerEntity.Id, t.Value, game);
									_tmpEntities.Remove(tmpEntity);
								}
							}
						}
					}
					else
						_tagChangeHandler.TagChange(gameState, match.Groups["tag"].Value, entity.Key, match.Groups["value"].Value, game);
				}
			}
			else if(LogConstants.PowerTaskList.CreationRegex.IsMatch(logLine))
			{
				var match = LogConstants.PowerTaskList.CreationRegex.Match(logLine);
				var id = int.Parse(match.Groups["id"].Value);
				var cardId = match.Groups["cardId"].Value;
				var zone = GameTagHelper.ParseEnum<Zone>(match.Groups["zone"].Value);
				if(!game.Entities.ContainsKey(id))
				{
					if(string.IsNullOrEmpty(cardId) && zone != Zone.SETASIDE)
					{
						var blockId = gameState.CurrentBlock?.Id;
						if(blockId.HasValue && gameState.KnownCardIds.ContainsKey(blockId.Value))
						{
							cardId = gameState.KnownCardIds[blockId.Value].FirstOrDefault();
							if(!string.IsNullOrEmpty(cardId))
							{
								Log.Info($"Found known cardId for entity {id}: {cardId}");
								gameState.KnownCardIds[blockId.Value].Remove(cardId);
							}
						}
					}
					game.Entities.Add(id, new Entity(id) {CardId = cardId});
				}
				gameState.SetCurrentEntity(id);
				if(gameState.DeterminedPlayers)
					_tagChangeHandler.InvokeQueuedActions(game);
				gameState.CurrentEntityHasCardId = !string.IsNullOrEmpty(cardId);
				gameState.CurrentEntityZone = zone;
				return;
			}
			else if(LogConstants.PowerTaskList.UpdatingEntityRegex.IsMatch(logLine))
			{
				var match = LogConstants.PowerTaskList.UpdatingEntityRegex.Match(logLine);
				var cardId = match.Groups["cardId"].Value;
				var rawEntity = match.Groups["entity"].Value;
				var type = match.Groups["type"].Value;
				int entityId;
				if(rawEntity.StartsWith("[") && LogConstants.PowerTaskList.EntityRegex.IsMatch(rawEntity))
				{
					var entity = LogConstants.PowerTaskList.EntityRegex.Match(rawEntity);
					entityId = int.Parse(entity.Groups["id"].Value);
				}
				else if(!int.TryParse(rawEntity, out entityId))
					entityId = -1;
				if(entityId != -1)
				{
					if(!game.Entities.ContainsKey(entityId))
						game.Entities.Add(entityId, new Entity(entityId));
					if(type != "CHANGE_ENTITY" || string.IsNullOrEmpty(game.Entities[entityId].CardId))
						game.Entities[entityId].CardId = cardId;
					gameState.SetCurrentEntity(entityId);
					if(gameState.DeterminedPlayers)
						_tagChangeHandler.InvokeQueuedActions(game);
				}
				if(gameState.JoustReveals > 0)
				{
					if(game.Entities.TryGetValue(entityId, out Entity currentEntity))
					{
						if(currentEntity.IsControlledBy(game.Opponent.Id))
							gameState.GameHandler.HandleOpponentJoust(currentEntity, cardId, gameState.GetTurnNumber());
						else if(currentEntity.IsControlledBy(game.Player.Id))
							gameState.GameHandler.HandlePlayerJoust(currentEntity, cardId, gameState.GetTurnNumber());
					}
				}
				return;
			}
			else if(LogConstants.PowerTaskList.CreationTagRegex.IsMatch(logLine) && !logLine.Contains("HIDE_ENTITY"))
			{
				var match = LogConstants.PowerTaskList.CreationTagRegex.Match(logLine);
				_tagChangeHandler.TagChange(gameState, match.Groups["tag"].Value, gameState.CurrentEntityId, match.Groups["value"].Value, game, true);
				creationTag = true;
			}
			else if(logLine.Contains("HIDE_ENTITY"))
			{
				if(gameState.CurrentBlock?.CardId == CardIds.Collectible.Neutral.KingTogwaggle
					|| gameState.CurrentBlock?.CardId == CardIds.NonCollectible.Neutral.KingTogwaggle_KingsRansomToken)
				{
					var match = LogConstants.PowerTaskList.HideEntityRegex.Match(logLine);
					if(match.Success)
					{
						var id = int.Parse(match.Groups["id"].Value);
						if(game.Entities.TryGetValue(id, out var entity))
							entity.Info.Hidden = true;
					}
				}
			}
			if(logLine.Contains("End Spectator"))
				gameState.GameHandler.HandleGameEnd();
			else if(logLine.Contains("BLOCK_START"))
			{
				var match = LogConstants.PowerTaskList.BlockStartRegex.Match(logLine);
				var blockType = match.Success ? match.Groups["type"].Value : null;
				var cardId = match.Success ? match.Groups["Id"].Value : null;
				gameState.BlockStart(blockType, cardId);

				if(match.Success && (blockType == "TRIGGER" || blockType == "POWER"))
				{
					var playerEntity =
						game.Entities.FirstOrDefault(
							e => e.Value.HasTag(GameTag.PLAYER_ID) && e.Value.GetTag(GameTag.PLAYER_ID) == game.Player.Id);
					var opponentEntity =
						game.Entities.FirstOrDefault(
							e => e.Value.HasTag(GameTag.PLAYER_ID) && e.Value.GetTag(GameTag.PLAYER_ID) == game.Opponent.Id);

					var actionStartingCardId = match.Groups["cardId"].Value.Trim();
					var actionStartingEntityId = int.Parse(match.Groups["id"].Value);

					if(string.IsNullOrEmpty(actionStartingCardId))
					{
						if(game.Entities.TryGetValue(actionStartingEntityId, out Entity actionEntity))
							actionStartingCardId = actionEntity.CardId;
					}
					if(string.IsNullOrEmpty(actionStartingCardId))
						return;
					if(blockType == "TRIGGER")
					{
						switch(actionStartingCardId)
						{
							case CardIds.Collectible.Rogue.TradePrinceGallywix:
								AddKnownCardId(gameState, game.Entities[gameState.LastCardPlayed].CardId);
								AddKnownCardId(gameState, CardIds.NonCollectible.Neutral.TradePrinceGallywix_GallywixsCoinToken);
								break;
							case CardIds.Collectible.Shaman.WhiteEyes:
								AddKnownCardId(gameState, CardIds.NonCollectible.Shaman.WhiteEyes_TheStormGuardianToken);
								break;
							case CardIds.Collectible.Hunter.RaptorHatchling:
								AddKnownCardId(gameState, CardIds.NonCollectible.Hunter.RaptorHatchling_RaptorPatriarchToken);
								break;
							case CardIds.Collectible.Warrior.DirehornHatchling:
								AddKnownCardId(gameState, CardIds.NonCollectible.Warrior.DirehornHatchling_DirehornMatriarchToken);
								break;
							case CardIds.Collectible.Mage.FrozenClone:
								AddKnownCardId(gameState, GetTargetCardId(match), 2);
								break;
							case CardIds.Collectible.Shaman.Moorabi:
							case CardIds.Collectible.Rogue.SonyaShadowdancer:
								AddKnownCardId(gameState, GetTargetCardId(match));
								break;
							case CardIds.Collectible.Neutral.HoardingDragon:
								AddKnownCardId(gameState, CardIds.NonCollectible.Neutral.TheCoin, 2);
								break;
							case CardIds.Collectible.Priest.GildedGargoyle:
								AddKnownCardId(gameState, CardIds.NonCollectible.Neutral.TheCoin);
								break;
							case CardIds.Collectible.Druid.AstralTiger:
								AddKnownCardId(gameState, CardIds.Collectible.Druid.AstralTiger);
								break;
							case CardIds.Collectible.Rogue.Kingsbane:
								AddKnownCardId(gameState, CardIds.Collectible.Rogue.Kingsbane);
								break;
						}
					}
					else //POWER
					{
						switch(actionStartingCardId)
						{
							case CardIds.Collectible.Rogue.GangUp:
								AddKnownCardId(gameState, GetTargetCardId(match), 3);
								break;
							case CardIds.Collectible.Rogue.BeneathTheGrounds:
								AddKnownCardId(gameState, CardIds.NonCollectible.Rogue.BeneaththeGrounds_NerubianAmbushToken, 3);
								break;
							case CardIds.Collectible.Warrior.IronJuggernaut:
								AddKnownCardId(gameState, CardIds.NonCollectible.Warrior.IronJuggernaut_BurrowingMineToken);
								break;
							case CardIds.Collectible.Druid.Recycle:
							case CardIds.Collectible.Mage.ManicSoulcaster:
							case CardIds.Collectible.Neutral.ZolaTheGorgon:
								AddKnownCardId(gameState, GetTargetCardId(match));
								break;
							case CardIds.Collectible.Mage.ForgottenTorch:
								AddKnownCardId(gameState, CardIds.NonCollectible.Mage.ForgottenTorch_RoaringTorchToken);
								break;
							case CardIds.Collectible.Warlock.CurseOfRafaam:
								AddKnownCardId(gameState, CardIds.NonCollectible.Warlock.CurseofRafaam_CursedToken);
								break;
							case CardIds.Collectible.Neutral.AncientShade:
								AddKnownCardId(gameState, CardIds.NonCollectible.Neutral.AncientShade_AncientCurseToken);
								break;
							case CardIds.Collectible.Priest.ExcavatedEvil:
								AddKnownCardId(gameState, CardIds.Collectible.Priest.ExcavatedEvil);
								break;
							case CardIds.Collectible.Neutral.EliseStarseeker:
								AddKnownCardId(gameState, CardIds.NonCollectible.Neutral.EliseStarseeker_MapToTheGoldenMonkeyToken);
								break;
							case CardIds.NonCollectible.Neutral.EliseStarseeker_MapToTheGoldenMonkeyToken:
								AddKnownCardId(gameState, CardIds.NonCollectible.Neutral.EliseStarseeker_GoldenMonkeyToken);
								break;
							case CardIds.Collectible.Neutral.Doomcaller:
								AddKnownCardId(gameState, CardIds.NonCollectible.Neutral.Cthun);
								break;
							case CardIds.Collectible.Druid.JadeIdol:
								AddKnownCardId(gameState, CardIds.Collectible.Druid.JadeIdol, 3);
								break;
							case CardIds.NonCollectible.Hunter.TheMarshQueen_QueenCarnassaToken:
								AddKnownCardId(gameState, CardIds.NonCollectible.Hunter.TheMarshQueen_CarnassasBroodToken, 15);
								break;
							case CardIds.Collectible.Neutral.EliseTheTrailblazer:
								AddKnownCardId(gameState, CardIds.NonCollectible.Neutral.ElisetheTrailblazer_UngoroPackToken);
								break;
							case CardIds.Collectible.Mage.GhastlyConjurer:
								AddKnownCardId(gameState, CardIds.Collectible.Mage.MirrorImage);
								break;
							case CardIds.Collectible.Mage.DeckOfWonders:
								AddKnownCardId(gameState, CardIds.NonCollectible.Mage.DeckofWonders_ScrollOfWonderToken, 5);
								break;
							case CardIds.Collectible.Neutral.TheDarkness:
								AddKnownCardId(gameState, CardIds.NonCollectible.Neutral.TheDarkness_DarknessCandleToken, 3);
								break;
							case CardIds.Collectible.Rogue.FaldoreiStrider:
								AddKnownCardId(gameState, CardIds.NonCollectible.Rogue.FaldoreiStrider_SpiderAmbushEnchantment, 3);
								break;
							case CardIds.Collectible.Neutral.KingTogwaggle:
								AddKnownCardId(gameState, CardIds.NonCollectible.Neutral.KingTogwaggle_KingsRansomToken);
								break;
							case CardIds.NonCollectible.Neutral.TheCandle:
								AddKnownCardId(gameState, CardIds.NonCollectible.Neutral.TheCandle);
								break;
							default:
								if(playerEntity.Value != null && playerEntity.Value.GetTag(GameTag.CURRENT_PLAYER) == 1
									&& !gameState.PlayerUsedHeroPower
									|| opponentEntity.Value != null && opponentEntity.Value.GetTag(GameTag.CURRENT_PLAYER) == 1
									&& !gameState.OpponentUsedHeroPower)
								{
									var card = Hearthstone.Database.GetCardFromId(actionStartingCardId);
									if(card.Type == "Hero Power")
									{
										if(playerEntity.Value != null && playerEntity.Value.GetTag(GameTag.CURRENT_PLAYER) == 1)
										{
											gameState.GameHandler.HandlePlayerHeroPower(actionStartingCardId, gameState.GetTurnNumber());
											gameState.PlayerUsedHeroPower = true;
										}
										else if(opponentEntity.Value != null)
										{
											gameState.GameHandler.HandleOpponentHeroPower(actionStartingCardId, gameState.GetTurnNumber());
											gameState.OpponentUsedHeroPower = true;
										}
									}
								}
								break;
						}
					}
				}
				else if(logLine.Contains("BlockType=JOUST"))
					gameState.JoustReveals = 2;
				else if(logLine.Contains("BlockType=REVEAL_CARD"))
					gameState.JoustReveals = 1;
				else if(gameState.GameTriggerCount == 0 && logLine.Contains("BLOCK_START BlockType=TRIGGER Entity=GameEntity"))
					gameState.GameTriggerCount++;
			}
			else if(logLine.Contains("CREATE_GAME"))
				_tagChangeHandler.ClearQueuedActions();
			else if(logLine.Contains("BLOCK_END"))
			{
				if(gameState.GameTriggerCount < 10 && (game.GameEntity?.HasTag(GameTag.TURN) ?? false))
				{
					gameState.GameTriggerCount += 10;
					_tagChangeHandler.InvokeQueuedActions(game);
					gameState.SetupDone = true;
				}
				if(gameState.CurrentBlock?.Type == "JOUST" || gameState.CurrentBlock?.Type == "REVEAL_CARD")
				{
					//make sure there are no more queued actions that might depend on JoustReveals
					_tagChangeHandler.InvokeQueuedActions(game);
					gameState.JoustReveals = 0;
				}

				gameState.BlockEnd();
			}


			if(game.IsInMenu)
				return;
			if(!creationTag && gameState.DeterminedPlayers)
				_tagChangeHandler.InvokeQueuedActions(game);
			if(!creationTag)
				gameState.ResetCurrentEntity();
		}

		private static string GetTargetCardId(Match match)
		{
			var target = match.Groups["target"].Value.Trim();
			if(!target.StartsWith("[") || !LogConstants.PowerTaskList.EntityRegex.IsMatch(target))
				return null;
			var cardIdMatch = LogConstants.PowerTaskList.CardIdRegex.Match(target);
			return !cardIdMatch.Success ? null : cardIdMatch.Groups["cardId"].Value.Trim();
		}

		private static void AddKnownCardId(IHsGameState gameState, string cardId, int count = 1)
		{
			var blockId = gameState.CurrentBlock.Id;
			for(var i = 0; i < count; i++)
			{
				if(!gameState.KnownCardIds.ContainsKey(blockId))
					gameState.KnownCardIds[blockId] = new List<string>();
				gameState.KnownCardIds[blockId].Add(cardId);
			}
		}

		internal void Reset() => _tagChangeHandler.ClearQueuedActions();
	}
}
