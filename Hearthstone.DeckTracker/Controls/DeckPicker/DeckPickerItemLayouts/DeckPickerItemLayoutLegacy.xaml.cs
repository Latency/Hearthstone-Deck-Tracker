#region

using System.Windows.Controls;
using System.Windows.Input;
using HearthStone.DeckTracker.Hearthstone;

#endregion

namespace HearthStone.DeckTracker.Controls.DeckPicker.DeckPickerItemLayouts
{
	/// <summary>
	/// Interaction logic for DeckPickerItemLayoutLegacy.xaml
	/// </summary>
	public partial class DeckPickerItemLayoutLegacy : UserControl
	{
		public DeckPickerItemLayoutLegacy()
		{
			InitializeComponent();
		}

		private void UseButton_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			var deck = DataContext as Deck;
			if(deck == null)
				return;
			if(deck.Equals(DeckList.Instance.ActiveDeck))
				return;
			Core.MainWindow.DeckPickerList.SelectDeck(deck);
			Core.MainWindow.SelectDeck(deck, true);
			Core.MainWindow.DeckPickerList.RefreshDisplayedDecks();
		}
	}
}
