﻿#region

using System.Windows;
using System.Windows.Controls;
using HearthStone.DeckTracker.Hearthstone;

#endregion

namespace HearthStone.DeckTracker.FlyoutControls
{
	/// <summary>
	/// Interaction logic for DeckNotes.xaml
	/// </summary>
	public partial class DeckNotes
	{
		private Deck _currentDeck;
		private bool _noteChanged;

		public DeckNotes()
		{
			InitializeComponent();
		}

		public void SetDeck(Deck deck)
		{
			SaveDeck();
			_currentDeck = deck;
			Textbox.Text = deck.Note;
			_noteChanged = false;
			BtnSave.IsEnabled = false;
		}

		private void Textbox_TextChanged(object sender, TextChangedEventArgs e)
		{
			_currentDeck.Note = Textbox.Text;
			_currentDeck.Edited();
			_noteChanged = true;
			BtnSave.IsEnabled = true;
		}

		private void BtnSave_Click(object sender, RoutedEventArgs e) => SaveDeck();

		public void SaveDeck()
		{
			if(!_noteChanged || _currentDeck == null)
				return;
			DeckList.Save();
			_noteChanged = false;
			BtnSave.IsEnabled = false;
			Core.MainWindow.DeckPickerList.UpdateDecks();
		}
	}
}
