using System;


namespace HearthStone.DeckTracker.Utility.Updating
{
	internal static partial class Updater
	{
		private static DateTime _lastUpdateCheck = DateTime.Now;
		public static StatusBarHelper StatusBar { get; } = new StatusBarHelper();
	}
}
