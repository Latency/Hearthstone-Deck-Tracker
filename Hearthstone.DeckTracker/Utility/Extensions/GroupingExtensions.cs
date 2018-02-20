using System;
using System.Linq;
using HearthStone.DeckTracker.Stats;
using HearthStone.DeckTracker.Stats.CompiledStats;


namespace HearthStone.DeckTracker.Utility.Extensions
{
	public static class GroupingExtensions
	{
		public static ConstructedDeckStats ToConstructedDeckStats(this IGrouping<Guid, GameStats> grouping) 
			=> grouping != null ? new ConstructedDeckStats(grouping) : null;
	}
}
