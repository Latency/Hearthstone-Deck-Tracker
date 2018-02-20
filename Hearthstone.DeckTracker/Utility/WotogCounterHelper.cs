#region

using System;
using System.Linq;
using HearthStone.Database;
using HearthStone.Database.Enums;
using HearthStone.DeckTracker.Enums;
using HearthStone.DeckTracker.Hearthstone.Entities;

#endregion

namespace HearthStone.DeckTracker.Utility
{
	public static class WotogCounterHelper
	{
		public static Entity PlayerCthun => Core.Game.Player.PlayerEntities.FirstOrDefault(x => x.CardId == CardIds.Collectible.Neutral.Cthun && x.Info.OriginalZone != null);
		public static Entity PlayerCthunProxy => Core.Game.Player.PlayerEntities.FirstOrDefault(x => x.CardId == CardIds.NonCollectible.Neutral.Cthun);
		public static Entity PlayerYogg => Core.Game.Player.PlayerEntities.FirstOrDefault(x => x.CardId == CardIds.Collectible.Neutral.YoggSaronHopesEnd && x.Info.OriginalZone != null);
		public static Entity PlayerArcaneGiant => Core.Game.Player.PlayerEntities.FirstOrDefault(x => x.CardId == CardIds.Collectible.Neutral.ArcaneGiant && x.Info.OriginalZone != null);
		public static Entity OpponentCthun => Core.Game.Opponent.PlayerEntities.FirstOrDefault(x => x.CardId == CardIds.Collectible.Neutral.Cthun);
		public static Entity OpponentCthunProxy => Core.Game.Opponent.PlayerEntities.FirstOrDefault(x => x.CardId == CardIds.NonCollectible.Neutral.Cthun);
		public static bool PlayerSeenCthun => Core.Game.PlayerEntity?.HasTag(GameTag.SEEN_CTHUN) ?? false;
		public static bool OpponentSeenCthun => Core.Game.OpponentEntity?.HasTag(GameTag.SEEN_CTHUN) ?? false;
		public static bool? CthunInDeck => DeckContains(CardIds.Collectible.Neutral.Cthun);
		public static bool? YoggInDeck => DeckContains(CardIds.Collectible.Neutral.YoggSaronHopesEnd);
		public static bool? ArcaneGiantInDeck => DeckContains(CardIds.Collectible.Neutral.ArcaneGiant);

		public static bool PlayerSeenJade => Core.Game.PlayerEntity?.HasTag(GameTag.JADE_GOLEM) ?? false;
		public static int PlayerNextJadeGolem => PlayerSeenJade ? Math.Min(Core.Game.PlayerEntity.GetTag(GameTag.JADE_GOLEM) + 1, 30) : 1;

		public static bool OpponentSeenJade => Core.Game.OpponentEntity?.HasTag(GameTag.JADE_GOLEM) ?? false;
		public static int OpponentNextJadeGolem => OpponentSeenJade ? Math.Min(Core.Game.OpponentEntity.GetTag(GameTag.JADE_GOLEM) + 1, 30) : 1;

		public static bool ShowPlayerCthunCounter => !Core.Game.IsInMenu && (Config.Instance.PlayerCthunCounter == DisplayMode.Always
					|| Config.Instance.PlayerCthunCounter == DisplayMode.Auto && PlayerSeenCthun);

		public static bool ShowPlayerSpellsCounter => !Core.Game.IsInMenu && (
			Config.Instance.PlayerSpellsCounter == DisplayMode.Always
				|| (Config.Instance.PlayerSpellsCounter == DisplayMode.Auto && YoggInDeck.HasValue && (PlayerYogg != null || YoggInDeck.Value))
				|| (Config.Instance.PlayerSpellsCounter == DisplayMode.Auto && ArcaneGiantInDeck.HasValue && (PlayerArcaneGiant != null || ArcaneGiantInDeck.Value))
			);

		public static bool ShowPlayerJadeCounter => !Core.Game.IsInMenu && (Config.Instance.PlayerJadeCounter == DisplayMode.Always
					|| Config.Instance.PlayerJadeCounter == DisplayMode.Auto && PlayerSeenJade);

		public static bool ShowOpponentCthunCounter => !Core.Game.IsInMenu && (Config.Instance.OpponentCthunCounter == DisplayMode.Always
					|| Config.Instance.OpponentCthunCounter == DisplayMode.Auto && OpponentSeenCthun);

		public static bool ShowOpponentSpellsCounter => !Core.Game.IsInMenu && Config.Instance.OpponentSpellsCounter == DisplayMode.Always;

		public static bool ShowOpponentJadeCounter => !Core.Game.IsInMenu && (Config.Instance.OpponentJadeCounter == DisplayMode.Always
					|| Config.Instance.OpponentJadeCounter == DisplayMode.Auto && OpponentSeenJade);

		private static bool? DeckContains(string cardId) => DeckList.Instance.ActiveDeck?.Cards.Any(x => x.Id == cardId);

	}
}
