using System;


namespace HearthStone.DeckTracker.Utility.Extensions
{
	public static class VersionExtensions
	{
		public static string ToVersionString(this Version version, bool includeRef = false) => $"{version?.Major}.{version?.Minor}.{version?.Build}{(includeRef ? "." + version?.Revision : "")}";
	}
}
