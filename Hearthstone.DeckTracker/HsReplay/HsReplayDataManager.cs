using HearthStone.DeckTracker.HsReplay.Data;


namespace HearthStone.DeckTracker.HsReplay
{
	public static class HsReplayDataManager
	{
		public static HsReplayDecks Decks { get; } = new HsReplayDecks();
		public static HsReplayWinrates Winrates { get; } = new HsReplayWinrates();
	}
}
