using System.Windows.Controls;
using HearthStone.DeckTracker.Hearthstone;


namespace HearthStone.DeckTracker.FlyoutControls.DeckHistory
{
	public partial class DeckHistoryView : UserControl
	{
		public DeckHistoryView()
		{
			InitializeComponent();
		}

		public Deck Deck
		{
			get => ((DeckHistoryViewModel)DataContext).Deck;
			set => ((DeckHistoryViewModel)DataContext).Deck = value;
		}
	}
}
