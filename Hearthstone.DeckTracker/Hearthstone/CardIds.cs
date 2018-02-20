#region

using System.Collections.Generic;

#endregion

namespace HearthStone.DeckTracker.Hearthstone
{
	public class CardIds
	{
		// todo: conditional deathrattle summons: Voidcaller
		public static readonly Dictionary<string, int> DeathrattleSummonCardIds = new Dictionary<string, int>
		{
			{HearthStone.Database.CardIds.Collectible.Druid.MountedRaptor, 1},
			{HearthStone.Database.CardIds.Collectible.Hunter.InfestedWolf, 2},
			{HearthStone.Database.CardIds.Collectible.Hunter.KindlyGrandmother, 1},
			{HearthStone.Database.CardIds.Collectible.Hunter.RatPack, 2},
			{HearthStone.Database.CardIds.Collectible.Hunter.SavannahHighmane, 2},
			{HearthStone.Database.CardIds.Collectible.Rogue.Anubarak, 1},
			{HearthStone.Database.CardIds.Collectible.Rogue.JadeSwarmer, 1},
			{HearthStone.Database.CardIds.Collectible.Warlock.Dreadsteed, 1},
			{HearthStone.Database.CardIds.Collectible.Warlock.PossessedVillager, 1},
			{HearthStone.Database.CardIds.Collectible.Warlock.Voidcaller, 1}, //false negative better than false positive
			{HearthStone.Database.CardIds.Collectible.Neutral.AyaBlackpaw, 1},
			{HearthStone.Database.CardIds.Collectible.Neutral.CairneBloodhoof, 1},
			{HearthStone.Database.CardIds.Collectible.Neutral.DevilsaurEgg, 1},
			{HearthStone.Database.CardIds.Collectible.Neutral.Eggnapper, 2},
			{HearthStone.Database.CardIds.Collectible.Neutral.HarvestGolem, 1},
			{HearthStone.Database.CardIds.Collectible.Neutral.HauntedCreeper, 2},
			{HearthStone.Database.CardIds.Collectible.Neutral.InfestedTauren, 1},
			{HearthStone.Database.CardIds.Collectible.Neutral.NerubianEgg, 1},
			{HearthStone.Database.CardIds.Collectible.Neutral.PilotedShredder, 1},
			{HearthStone.Database.CardIds.Collectible.Neutral.PilotedSkyGolem, 1},
			{HearthStone.Database.CardIds.Collectible.Neutral.SatedThreshadon, 3},
			{HearthStone.Database.CardIds.Collectible.Neutral.SludgeBelcher, 1},
			{HearthStone.Database.CardIds.Collectible.Neutral.SneedsOldShredder, 1},
			{HearthStone.Database.CardIds.Collectible.Neutral.TwilightSummoner, 1},
			{HearthStone.Database.CardIds.Collectible.Neutral.WobblingRunts, 3},
		};

		public static readonly Dictionary<string, string> HeroIdDict = new Dictionary<string, string>
		{
			{HearthStone.Database.CardIds.Collectible.Warrior.GarroshHellscream, "Warrior"},
			{HearthStone.Database.CardIds.Collectible.Shaman.Thrall, "Shaman"},
			{HearthStone.Database.CardIds.Collectible.Rogue.ValeeraSanguinar, "Rogue"},
			{HearthStone.Database.CardIds.Collectible.Paladin.UtherLightbringer, "Paladin"},
			{HearthStone.Database.CardIds.Collectible.Hunter.Rexxar, "Hunter"},
			{HearthStone.Database.CardIds.Collectible.Druid.MalfurionStormrage, "Druid"},
			{HearthStone.Database.CardIds.Collectible.Warlock.Guldan, "Warlock"},
			{HearthStone.Database.CardIds.Collectible.Mage.JainaProudmoore, "Mage"},
			{HearthStone.Database.CardIds.Collectible.Priest.AnduinWrynn, "Priest"},
			{HearthStone.Database.CardIds.Collectible.Warlock.LordJaraxxus, "Jaraxxus"},
			{HearthStone.Database.CardIds.Collectible.Neutral.MajordomoExecutus, "Ragnaros the Firelord"}
		};

		public static readonly Dictionary<string, string> HeroNameDict = new Dictionary<string, string>
		{
			{"Warrior", HearthStone.Database.CardIds.Collectible.Warrior.GarroshHellscream},
			{"Shaman", HearthStone.Database.CardIds.Collectible.Shaman.Thrall},
			{"Rogue", HearthStone.Database.CardIds.Collectible.Rogue.ValeeraSanguinar},
			{"Paladin", HearthStone.Database.CardIds.Collectible.Paladin.UtherLightbringer},
			{"Hunter", HearthStone.Database.CardIds.Collectible.Hunter.Rexxar},
			{"Druid", HearthStone.Database.CardIds.Collectible.Druid.MalfurionStormrage},
			{"Warlock", HearthStone.Database.CardIds.Collectible.Warlock.Guldan},
			{"Mage", HearthStone.Database.CardIds.Collectible.Mage.JainaProudmoore},
			{"Priest", HearthStone.Database.CardIds.Collectible.Priest.AnduinWrynn}
		};

		public static class Secrets
		{
			public static List<string> ArenaExcludes = new List<string>
			{
				Hunter.Snipe
			};

			public static List<string> FastCombat = new List<string>
			{
				Hunter.FreezingTrap,
				Hunter.ExplosiveTrap,
				Hunter.Misdirection,
				Paladin.NobleSacrifice,
				Mage.Vaporize
			};

			public static class Hunter
			{
				public static List<string> All => new List<string> {BearTrap, CatTrick, DartTrap, ExplosiveTrap, FreezingTrap, HiddenCache, Misdirection, Snipe, SnakeTrap, VenomstrikeTrap, WanderingMonster};
				public static string BearTrap => HearthStone.Database.CardIds.Collectible.Hunter.BearTrap;
				public static string CatTrick => HearthStone.Database.CardIds.Collectible.Hunter.CatTrick;
				public static string DartTrap => HearthStone.Database.CardIds.Collectible.Hunter.DartTrap;
				public static string ExplosiveTrap => HearthStone.Database.CardIds.Collectible.Hunter.ExplosiveTrap;
				public static string FreezingTrap => HearthStone.Database.CardIds.Collectible.Hunter.FreezingTrap;
				public static string HiddenCache => HearthStone.Database.CardIds.Collectible.Hunter.HiddenCache;
				public static string Misdirection => HearthStone.Database.CardIds.Collectible.Hunter.Misdirection;
				public static string Snipe => HearthStone.Database.CardIds.Collectible.Hunter.Snipe;
				public static string SnakeTrap => HearthStone.Database.CardIds.Collectible.Hunter.SnakeTrap;
				public static string VenomstrikeTrap => HearthStone.Database.CardIds.Collectible.Hunter.VenomstrikeTrap;
				public static string WanderingMonster => HearthStone.Database.CardIds.Collectible.Hunter.WanderingMonster;
			}

			public static class Mage
			{
				public static List<string> All => new List<string> {Counterspell, Duplicate, Effigy, ExplosiveRunes, FrozenClone, IceBarrier, IceBlock, ManaBind, MirrorEntity, PotionOfPolymorph, Spellbender, Vaporize};
				public static string Counterspell => HearthStone.Database.CardIds.Collectible.Mage.Counterspell;
				public static string Duplicate => HearthStone.Database.CardIds.Collectible.Mage.Duplicate;
				public static string Effigy => HearthStone.Database.CardIds.Collectible.Mage.Effigy;
				public static string ExplosiveRunes => HearthStone.Database.CardIds.Collectible.Mage.ExplosiveRunes;
				public static string FrozenClone => HearthStone.Database.CardIds.Collectible.Mage.FrozenClone;
				public static string IceBarrier => HearthStone.Database.CardIds.Collectible.Mage.IceBarrier;
				public static string IceBlock => HearthStone.Database.CardIds.Collectible.Mage.IceBlock;
				public static string ManaBind => HearthStone.Database.CardIds.Collectible.Mage.ManaBind;
				public static string MirrorEntity => HearthStone.Database.CardIds.Collectible.Mage.MirrorEntity;
				public static string PotionOfPolymorph => HearthStone.Database.CardIds.Collectible.Mage.PotionOfPolymorph;
				public static string Spellbender => HearthStone.Database.CardIds.Collectible.Mage.Spellbender;
				public static string Vaporize => HearthStone.Database.CardIds.Collectible.Mage.Vaporize;
			}

			public static class Paladin
			{
				public static List<string> All => new List<string> {Avenge, CompetitiveSpirit, EyeForAnEye, GetawayKodo, NobleSacrifice, Redemption, Repentance, SacredTrial};
				public static string Avenge => HearthStone.Database.CardIds.Collectible.Paladin.Avenge;
				public static string CompetitiveSpirit => HearthStone.Database.CardIds.Collectible.Paladin.CompetitiveSpirit;
				public static string EyeForAnEye => HearthStone.Database.CardIds.Collectible.Paladin.EyeForAnEye;
				public static string GetawayKodo => HearthStone.Database.CardIds.Collectible.Paladin.GetawayKodo;
				public static string NobleSacrifice => HearthStone.Database.CardIds.Collectible.Paladin.NobleSacrifice;
				public static string Redemption => HearthStone.Database.CardIds.Collectible.Paladin.Redemption;
				public static string Repentance => HearthStone.Database.CardIds.Collectible.Paladin.Repentance;
				public static string SacredTrial => HearthStone.Database.CardIds.Collectible.Paladin.SacredTrial;
			}

			public static class Rogue
			{
				public static List<string> All => new List<string> {CheatDeath, Evasion, SuddenBetrayal};
				public static string CheatDeath => HearthStone.Database.CardIds.Collectible.Rogue.CheatDeath;
				public static string Evasion => HearthStone.Database.CardIds.Collectible.Rogue.Evasion;
				public static string SuddenBetrayal => HearthStone.Database.CardIds.Collectible.Rogue.SuddenBetrayal;
			}
		}
	}
}
