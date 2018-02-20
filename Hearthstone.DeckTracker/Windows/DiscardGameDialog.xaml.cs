#region

using System.Windows;
using HearthStone.DeckTracker.Enums;
using HearthStone.DeckTracker.Stats;

#endregion

namespace HearthStone.DeckTracker.Windows
{
	/// <summary>
	/// Interaction logic for DiscardGameDialog.xaml
	/// </summary>
	public partial class DiscardGameDialog
	{
		public DiscardGameDialog(GameStats game)
		{
			InitializeComponent();
			LblGameInfo.Content = game.ToString();
		}

		public DiscardGameDialogResult Result { get; private set; }

		private void BtnDiscard_Click(object sender, RoutedEventArgs e)
		{
			Result = DiscardGameDialogResult.Discard;
			Close();
		}

		private void BtnKeep_Click(object sender, RoutedEventArgs e)
		{
			Result = DiscardGameDialogResult.Keep;
			Close();
		}

		private void BtnMoveToOther_Click(object sender, RoutedEventArgs e)
		{
			Result = DiscardGameDialogResult.MoveToOther;
			Close();
		}
	}
}
