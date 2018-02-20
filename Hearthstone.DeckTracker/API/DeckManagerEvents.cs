#region

using System.Collections.Generic;
using HearthStone.DeckTracker.Hearthstone;

#endregion

namespace HearthStone.DeckTracker.API
{
	public class DeckManagerEvents
	{
		public static readonly ActionList<Deck> OnDeckUpdated = new ActionList<Deck>();
		public static readonly ActionList<Deck> OnDeckCreated = new ActionList<Deck>();
		public static readonly ActionList<Deck> OnDeckSelected = new ActionList<Deck>();
		public static readonly ActionList<IEnumerable<Deck>> OnDeckDeleted = new ActionList<IEnumerable<Deck>>();
	}
}
