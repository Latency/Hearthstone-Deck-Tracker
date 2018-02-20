using HearthStone.DeckTracker.Hearthstone;


namespace HearthStone.DeckTracker.Windows.MainWindowControls
{
	public partial class DeckChartsView
	{
		public DeckChartsView()
		{
			InitializeComponent();
		}

		public void SetDeck(Deck deck) => ((DeckChartsViewModel)DataContext).Deck = deck;

		public void ReloadUI() => ((DeckChartsViewModel)DataContext).Update();
	}
}
