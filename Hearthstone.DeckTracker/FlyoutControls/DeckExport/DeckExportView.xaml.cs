using HearthStone.DeckTracker.Hearthstone;


namespace HearthStone.DeckTracker.FlyoutControls.DeckExport
{
	public partial class DeckExportView
	{
		public DeckExportView()
		{
			InitializeComponent();
		}

		public Deck Deck
		{
			get { return ((DeckExportViewModel)DataContext).Deck; }
			set { ((DeckExportViewModel)DataContext).Deck = value; }
		}
	}
}
