using System.Collections.Generic;
using System.Linq;
using HearthStone.DeckTracker.Hearthstone;


namespace HearthStone.DeckTracker.Utility.Extensions
{
	public static class CardListExtensions
	{
		public static List<Card> ToSortedCardList(this IEnumerable<Card> cards)
			=> cards.OrderByDescending(x => x.HideStats).ThenBy(x => x.Cost).ThenBy(x => x.LocalizedName).ToArray().ToList();
	}
}
