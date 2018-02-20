using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using HearthStone.DeckTracker.Utility;


namespace HearthStone.DeckTracker.Hearthstone
{
	public class DungeonRun
	{
		public static Deck GetDefaultDeck(string playerClass)
		{
			var cards = GetCards(playerClass);
			if(cards == null)
				return null;
			var deck = new Deck
			{
				Cards = new ObservableCollection<Card>(cards.Select(Database.GetCardFromId)),
				Class = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(playerClass.ToLowerInvariant()),
				IsDungeonDeck = true,
				LastEdited = DateTime.Now
			};
			deck.Name = Helper.ParseDeckNameTemplate(Config.Instance.DungeonRunDeckNameTemplate, deck);
			return deck;
		}

		private static List<string> GetCards(string playerClass)
		{
			switch(playerClass.ToUpperInvariant())
			{
				case "ROGUE":
					return DefaultDecks.Rogue;
				case "WARRIOR":
					return DefaultDecks.Warrior;
				case "SHAMAN":
					return DefaultDecks.Shaman;
				case "PALADIN":
					return DefaultDecks.Paladin;
				case "HUNTER":
					return DefaultDecks.Hunter;
				case "DRUID":
					return DefaultDecks.Druid;
				case "WARLOCK":
					return DefaultDecks.Warlock;
				case "MAGE":
					return DefaultDecks.Mage;
				case "PRIEST":
					return DefaultDecks.Priest;
			}
			return null;
		}

		public static bool IsDungeonBoss(string cardId) => cardId != null && cardId.Contains("LOOT") && cardId.Contains("BOSS");

		private static class DefaultDecks
		{
			public static List<string> Rogue => new List<string>
			{
				HearthStone.Database.CardIds.Collectible.Rogue.Backstab,
				HearthStone.Database.CardIds.Collectible.Rogue.DeadlyPoison,
				HearthStone.Database.CardIds.Collectible.Rogue.PitSnake,
				HearthStone.Database.CardIds.Collectible.Rogue.SinisterStrike,
				HearthStone.Database.CardIds.Collectible.Neutral.GilblinStalker,
				HearthStone.Database.CardIds.Collectible.Rogue.UndercityHuckster,
				HearthStone.Database.CardIds.Collectible.Rogue.Si7Agent,
				HearthStone.Database.CardIds.Collectible.Rogue.UnearthedRaptor,
				HearthStone.Database.CardIds.Collectible.Rogue.Assassinate,
				HearthStone.Database.CardIds.Collectible.Rogue.Vanish,
			};

			public static List<string> Warrior = new List<string>
			{
				HearthStone.Database.CardIds.Collectible.Warrior.Warbot,
				HearthStone.Database.CardIds.Collectible.Neutral.AmaniBerserker,
				HearthStone.Database.CardIds.Collectible.Warrior.CruelTaskmaster,
				HearthStone.Database.CardIds.Collectible.Warrior.HeroicStrike,
				HearthStone.Database.CardIds.Collectible.Warrior.Bash,
				HearthStone.Database.CardIds.Collectible.Warrior.FieryWarAxe,
				HearthStone.Database.CardIds.Collectible.Neutral.HiredGun,
				HearthStone.Database.CardIds.Collectible.Neutral.RagingWorgen,
				HearthStone.Database.CardIds.Collectible.Neutral.DreadCorsair,
				HearthStone.Database.CardIds.Collectible.Warrior.Brawl,
			};

			public static List<string> Shaman = new List<string>
			{
				HearthStone.Database.CardIds.Collectible.Shaman.AirElemental,
				HearthStone.Database.CardIds.Collectible.Shaman.LightningBolt,
				HearthStone.Database.CardIds.Collectible.Shaman.FlametongueTotem,
				HearthStone.Database.CardIds.Collectible.Neutral.MurlocTidehunter,
				HearthStone.Database.CardIds.Collectible.Shaman.StormforgedAxe,
				HearthStone.Database.CardIds.Collectible.Shaman.LightningStorm,
				HearthStone.Database.CardIds.Collectible.Shaman.UnboundElemental,
				HearthStone.Database.CardIds.Collectible.Neutral.DefenderOfArgus,
				HearthStone.Database.CardIds.Collectible.Shaman.Hex,
				HearthStone.Database.CardIds.Collectible.Shaman.FireElemental,
			};

			public static List<string> Paladin = new List<string>
			{
				HearthStone.Database.CardIds.Collectible.Paladin.BlessingOfMight,
				HearthStone.Database.CardIds.Collectible.Neutral.GoldshireFootman,
				HearthStone.Database.CardIds.Collectible.Paladin.NobleSacrifice,
				HearthStone.Database.CardIds.Collectible.Paladin.ArgentProtector,
				HearthStone.Database.CardIds.Collectible.Paladin.Equality,
				HearthStone.Database.CardIds.Collectible.Paladin.HolyLight,
				HearthStone.Database.CardIds.Collectible.Neutral.EarthenRingFarseer,
				HearthStone.Database.CardIds.Collectible.Paladin.Consecration,
				HearthStone.Database.CardIds.Collectible.Neutral.StormwindKnight,
				HearthStone.Database.CardIds.Collectible.Paladin.TruesilverChampion,
			};

			public static List<string> Hunter = new List<string>
			{
				HearthStone.Database.CardIds.Collectible.Hunter.HuntersMark,
				HearthStone.Database.CardIds.Collectible.Neutral.StonetuskBoar,
				HearthStone.Database.CardIds.Collectible.Neutral.DireWolfAlpha,
				HearthStone.Database.CardIds.Collectible.Hunter.ExplosiveTrap,
				HearthStone.Database.CardIds.Collectible.Hunter.AnimalCompanion,
				HearthStone.Database.CardIds.Collectible.Hunter.DeadlyShot,
				HearthStone.Database.CardIds.Collectible.Hunter.EaglehornBow,
				HearthStone.Database.CardIds.Collectible.Neutral.JunglePanther,
				HearthStone.Database.CardIds.Collectible.Hunter.UnleashTheHounds,
				HearthStone.Database.CardIds.Collectible.Neutral.OasisSnapjaw,
			};

			public static List<string> Druid = new List<string>
			{
				HearthStone.Database.CardIds.Collectible.Druid.EnchantedRaven,
				HearthStone.Database.CardIds.Collectible.Druid.PowerOfTheWild,
				HearthStone.Database.CardIds.Collectible.Druid.TortollanForager,
				HearthStone.Database.CardIds.Collectible.Druid.MountedRaptor,
				HearthStone.Database.CardIds.Collectible.Druid.Mulch,
				HearthStone.Database.CardIds.Collectible.Neutral.ShadeOfNaxxramas,
				HearthStone.Database.CardIds.Collectible.Druid.KeeperOfTheGrove,
				HearthStone.Database.CardIds.Collectible.Druid.SavageCombatant,
				HearthStone.Database.CardIds.Collectible.Druid.Swipe,
				HearthStone.Database.CardIds.Collectible.Druid.DruidOfTheClaw,
			};

			public static List<string> Warlock = new List<string>
			{
				HearthStone.Database.CardIds.Collectible.Warlock.Corruption,
				HearthStone.Database.CardIds.Collectible.Warlock.MortalCoil,
				HearthStone.Database.CardIds.Collectible.Warlock.Voidwalker,
				HearthStone.Database.CardIds.Collectible.Neutral.KnifeJuggler,
				HearthStone.Database.CardIds.Collectible.Neutral.SunfuryProtector,
				HearthStone.Database.CardIds.Collectible.Warlock.DrainLife,
				HearthStone.Database.CardIds.Collectible.Neutral.ImpMaster,
				HearthStone.Database.CardIds.Collectible.Neutral.DarkIronDwarf,
				HearthStone.Database.CardIds.Collectible.Warlock.Hellfire,
				HearthStone.Database.CardIds.Collectible.Warlock.Doomguard,
			};

			public static List<string> Mage = new List<string>
			{
				HearthStone.Database.CardIds.Collectible.Mage.ArcaneMissiles,
				HearthStone.Database.CardIds.Collectible.Mage.ManaWyrm,
				HearthStone.Database.CardIds.Collectible.Neutral.Doomsayer,
				HearthStone.Database.CardIds.Collectible.Mage.Frostbolt,
				HearthStone.Database.CardIds.Collectible.Mage.SorcerersApprentice,
				HearthStone.Database.CardIds.Collectible.Neutral.EarthenRingFarseer,
				HearthStone.Database.CardIds.Collectible.Mage.IceBarrier,
				HearthStone.Database.CardIds.Collectible.Neutral.ChillwindYeti,
				HearthStone.Database.CardIds.Collectible.Mage.Fireball,
				HearthStone.Database.CardIds.Collectible.Mage.Blizzard,
			};

			public static List<string> Priest = new List<string>
			{
				HearthStone.Database.CardIds.Collectible.Priest.HolySmite,
				HearthStone.Database.CardIds.Collectible.Priest.NorthshireCleric,
				HearthStone.Database.CardIds.Collectible.Priest.PotionOfMadness,
				HearthStone.Database.CardIds.Collectible.Neutral.FaerieDragon,
				HearthStone.Database.CardIds.Collectible.Priest.MindBlast,
				HearthStone.Database.CardIds.Collectible.Priest.ShadowWordPain,
				HearthStone.Database.CardIds.Collectible.Priest.DarkCultist,
				HearthStone.Database.CardIds.Collectible.Priest.AuchenaiSoulpriest,
				HearthStone.Database.CardIds.Collectible.Priest.Lightspawn,
				HearthStone.Database.CardIds.Collectible.Priest.HolyNova,
			};
		}
	}
}
