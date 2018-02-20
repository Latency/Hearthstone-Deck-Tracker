using HearthStone.DeckTracker.Hearthstone;
using HearthStone.DeckTracker.Utility.MVVM;


namespace HearthStone.DeckTracker.Controls.DeckSetIcons
{
	public class DeckSetIconsViewModel : ViewModel
	{
		private Deck _deck;

		public Deck Deck
		{
			get => _deck;
			set
			{
				_deck = value;
				OnPropertyChanged();
			}
		}
	}
}
