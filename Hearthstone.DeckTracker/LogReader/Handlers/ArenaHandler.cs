#region

using HearthStone.DeckTracker.Enums.Hearthstone;
using HearthStone.DeckTracker.Hearthstone;
using HearthStone.DeckTracker.LogReader.Interfaces;
using HearthStone.Watcher.LogReader;

#endregion

namespace HearthStone.DeckTracker.LogReader.Handlers
{
	public class ArenaHandler
	{
		public void Handle(LogLine logLine, IHsGameState gameState, IGame game)
		{
			if(logLine.Line.Contains("IN_REWARDS") && game.CurrentMode == Mode.DRAFT)
				Watchers.ArenaWatcher.Update();
		}
	}
}
