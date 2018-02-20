using System.Collections.Generic;


namespace HearthStone.DeckTracker.Hearthstone
{
	public class DeckState
	{
		public DeckState(IEnumerable<Card> remainingInDeck, IEnumerable<Card> removedFromDeck)
		{
			RemovedFromDeck = removedFromDeck;
			RemainingInDeck = remainingInDeck;
		}

		public IEnumerable<Card> RemainingInDeck { get; }
		public IEnumerable<Card> RemovedFromDeck { get; }
	}
}
