using System.Collections.Generic;
using System.Linq;
using HearthStone.Database.Enums;
using HearthStone.DeckTracker.Enums;
using HearthStone.DeckTracker.Enums.Hearthstone;
using HearthStone.DeckTracker.HsReplay;
using HearthStone.DeckTracker.Importing;
using HearthStone.DeckTracker.Utility;
using HearthStone.Mirror;
using HearthStone.Mirror.Objects;
using HearthStone.Watcher;
using HearthStone.Watcher.Providers;


namespace HearthStone.DeckTracker.Hearthstone
{
	public static class Watchers
	{
		static Watchers()
		{
			ArenaWatcher.OnCompleteDeck += (sender, args) => DeckManager.AutoImportArena(Config.Instance.SelectedArenaImportingBehaviour ?? ArenaImportingBehaviour.AutoImportSave, args.Info);
			PackWatcher.NewPackEventHandler += (sender, args) => PackUploader.UploadPack(args.PackId, args.Cards);
			DungeonRunWatcher.DungeonRunMatchStarted += DeckManager.DungeonRunMatchStarted;
			DungeonRunWatcher.DungeonInfoChanged += DeckManager.UpdateDungeonRunDeck;
			FriendlyChallengeWatcher.OnFriendlyChallenge += OnFriendlyChallenge;
		}

		internal static void Stop()
		{
			ArenaWatcher.Stop();
			PackWatcher.Stop();
			DungeonRunWatcher.Stop();
			FriendlyChallengeWatcher.Stop();
		}

		internal static void OnFriendlyChallenge(object sender, Watcher.EventArgs.FriendlyChallengeEventArgs args)
		{
			if(args.DialogVisible && Config.Instance.FlashHsOnFriendlyChallenge)
				User32.FlashHs();
		}

		public static ArenaWatcher ArenaWatcher { get; } = new ArenaWatcher(new HearthMirrorArenaProvider());
		public static PackOpeningWatcher PackWatcher { get; } = new PackOpeningWatcher(new HearthMirrorPackProvider());
		public static DungeonRunWatcher DungeonRunWatcher { get; } = new DungeonRunWatcher(new GameDataProvider());
		public static FriendlyChallengeWatcher FriendlyChallengeWatcher { get; } = new FriendlyChallengeWatcher(new HearthMirrorFriendlyChallengeProvider());
	}

	public class GameDataProvider : IGameDataProvider
	{
		public bool InAiMatch => Core.Game.CurrentMode == Mode.GAMEPLAY && Core.Game.MatchInfo?.GameType == (int)GameType.GT_VS_AI;
		public bool InAdventureScreen => Core.Game.CurrentMode == Mode.ADVENTURE;
		public string OpponentHeroId => Core.Game.Opponent.Board.FirstOrDefault(x => x.IsHero)?.CardId;
	}

	public class HearthMirrorPackProvider : IPackProvider
	{
		public List<Mirror.Objects.Card> GetCards() => Reflection.GetPackCards();
		public int GetPackId() => Reflection.GetLastOpenedBoosterId();
	}

	public class HearthMirrorArenaProvider : IArenaProvider
	{
		public ArenaInfo GetArenaInfo() => DeckImporter.FromArena(false);
		public Mirror.Objects.Card[] GetDraftChoices() => Reflection.GetArenaDraftChoices()?.ToArray();
	}

	public class HearthMirrorFriendlyChallengeProvider : IFriendlyChallengeProvider
	{
		public bool DialogVisible => Reflection.IsFriendlyChallengeDialogVisible();
	}
}
