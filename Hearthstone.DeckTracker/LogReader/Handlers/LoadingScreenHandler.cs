#region

using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using HearthStone.DeckTracker.Enums.Hearthstone;
using HearthStone.DeckTracker.Hearthstone;
using HearthStone.DeckTracker.Importing;
using HearthStone.DeckTracker.LogReader.Interfaces;
using HearthStone.DeckTracker.Utility.Analytics;
using HearthStone.DeckTracker.Utility.Extensions;
using HearthStone.DeckTracker.Utility.Logging;
using HearthStone.Mirror;
using HearthStone.Mirror.Enums;
using HearthStone.Watcher.LogReader;

#endregion

namespace HearthStone.DeckTracker.LogReader.Handlers
{
	public class LoadingScreenHandler
	{
		private DateTime _lastAutoImport;
		private bool _checkedMirrorStatus;
		public event Action OnHearthMirrorCheckFailed;

		public void Handle(LogLine logLine, IHsGameState gameState, IGame game)
		{
			var match = LogConstants.GameModeRegex.Match(logLine.Line);
			if(match.Success)
			{
				game.CurrentMode = GetMode(match.Groups["curr"].Value);
				game.PreviousMode = GetMode(match.Groups["prev"].Value);

				if((DateTime.Now - logLine.Time).TotalSeconds < 5 && _lastAutoImport < logLine.Time
					&& game.CurrentMode == Mode.TOURNAMENT)
				{
					_lastAutoImport = logLine.Time;
					var decks = DeckImporter.FromConstructed();
					if(decks.Any() && (Config.Instance.ConstructedAutoImportNew || Config.Instance.ConstructedAutoUpdate))
					{
						DeckManager.ImportDecks(decks, false, Config.Instance.ConstructedAutoImportNew,
							Config.Instance.ConstructedAutoUpdate);
					}
				}

				if(game.PreviousMode == Mode.GAMEPLAY && game.CurrentMode != Mode.GAMEPLAY)
					gameState.GameHandler.HandleInMenu();

				if(game.CurrentMode == Mode.HUB && !_checkedMirrorStatus && (DateTime.Now - logLine.Time).TotalSeconds < 5)
				{
					CheckMirrorStatus();
					if(CollectionHelper.IsAwaitingUpdate)
						CollectionHelper.TryUpdateCollection().Forget();
				}

				if(game.CurrentMode == Mode.DRAFT)
					Watchers.ArenaWatcher.Run();
				else
					Watchers.ArenaWatcher.Stop();

				if(game.CurrentMode == Mode.PACKOPENING)
					Watchers.PackWatcher.Run();
				else
					Watchers.PackWatcher.Stop();

				if(game.CurrentMode == Mode.TAVERN_BRAWL)
					Core.Game.CacheBrawlInfo();

				if(game.CurrentMode == Mode.ADVENTURE || game.PreviousMode == Mode.ADVENTURE && game.CurrentMode == Mode.GAMEPLAY)
					Watchers.DungeonRunWatcher.Run();
				else
					Watchers.DungeonRunWatcher.Stop();

				if(Config.Instance.FlashHsOnFriendlyChallenge)
				{
					if(game.PlayerChallengeable)
						Watchers.FriendlyChallengeWatcher.Run();
					else
						Watchers.FriendlyChallengeWatcher.Stop(); 
				}

				API.GameEvents.OnModeChanged.Execute(game.CurrentMode);
			}
			else if(logLine.Line.Contains("Gameplay.Start"))
			{
				gameState.Reset();
				gameState.GameHandler.HandleGameStart(logLine.Time);
			}
		}

		private async void CheckMirrorStatus()
		{
			_checkedMirrorStatus = true;
			Status status;
			while((status = Status.GetStatus()).MirrorStatus == MirrorStatus.ProcNotFound)
				await Task.Delay(1000);
			Log.Info($"Mirror status: {status.MirrorStatus}");
			if(status.MirrorStatus != MirrorStatus.Error)
				return;
			Log.Error(status.Exception);
			if(!(status.Exception is Win32Exception))
			{
				Log.Info("Not a Win32Exception - Process probably exited. Checking again later.");
				_checkedMirrorStatus = false;
				return;
			}
			Influx.OnUnevenPermissions();
			OnHearthMirrorCheckFailed?.Invoke();
		}

		private Mode GetMode(string modeString) => Enum.TryParse(modeString, out Mode mode) ? mode : Mode.INVALID;
	}
}
