#region

using System;
using HearthStone.DeckTracker.Enums;
using HearthStone.DeckTracker.Hearthstone;
using HearthStone.DeckTracker.LogReader.Interfaces;
using HearthStone.DeckTracker.Utility.Logging;

#endregion

namespace HearthStone.DeckTracker.LogReader.Handlers
{
	public class RachelleHandler
	{
		public void Handle(string logLine, IHsGameState gameState, IGame game)
		{
			if(!LogConstants.GoldProgressRegex.IsMatch(logLine) || (DateTime.Now - gameState.LastGameStart) <= TimeSpan.FromSeconds(10)
				|| game.CurrentGameMode == GameMode.Spectator)
				return;
			var rawWins = LogConstants.GoldProgressRegex.Match(logLine).Groups["wins"].Value;
			if(!int.TryParse(rawWins, out int wins))
				return;
			var timeZone = GetTimeZoneInfo(game.CurrentRegion);
			if(timeZone != null)
				UpdateGoldProgress(wins, game, timeZone);
		}

		private TimeZoneInfo GetTimeZoneInfo(Region region)
		{
			try
			{
				switch(region)
				{
					case Region.EU:
						return TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
					case Region.US:
						return TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
					case Region.ASIA:
						return TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time");
					case Region.CHINA:
						return TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
					default:
						Log.Error($"Could not get TimeZoneInfo for Region {region}");
						return null;
				}
			}
			catch(Exception ex)
			{
				Log.Error("Error determining region: " + ex);
			}
			return null;
		}

		private void UpdateGoldProgress(int wins, IGame game, TimeZoneInfo timeZone)
		{
			try
			{
				var region = (int)game.CurrentRegion - 1;
				var date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone).Date;
				if(Config.Instance.GoldProgressLastReset[region].Date != date)
				{
					Config.Instance.GoldProgressTotal[region] = 0;
					Config.Instance.GoldProgressLastReset[region] = date;
				}
				Config.Instance.GoldProgress[region] = wins == 3 ? 0 : wins;
				if(wins == 3)
					Config.Instance.GoldProgressTotal[region] += 10;
				Config.Save();
			}
			catch(Exception ex)
			{
				Log.Error("Error updating GoldProgress: " + ex);
			}
		}
	}
}
