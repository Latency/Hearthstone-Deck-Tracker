#region

using System;

#endregion

namespace HearthStone.DeckTracker.Utility.Logging
{
	[Flags]
	public enum LogType
	{
		Debug,
		Info,
		Warning,
		Error
	}
}
