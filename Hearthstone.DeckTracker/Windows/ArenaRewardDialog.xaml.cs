#region

using System.Windows;
using HearthStone.DeckTracker.Hearthstone;
using HearthStone.DeckTracker.Stats.CompiledStats;

#endregion

namespace HearthStone.DeckTracker.Windows
{
	/// <summary>
	/// Interaction logic for ArenaRewardDialog.xaml
	/// </summary>
	public partial class ArenaRewardDialog
	{
		private readonly Deck _deck;

		public ArenaRewardDialog(Deck deck)
		{
			_deck = deck;
			InitializeComponent();
			ArenaRewards.LoadArenaReward(deck.ArenaReward);
		}

		private async void ArenaRewards_OnSave(object sender, RoutedEventArgs e)
		{
			if(!ArenaRewards.Validate(out var warning))
			{
				await MessageDialogs.ShowMessage(this, "Error", warning);
				return;
			}
			_deck.ArenaReward = ArenaRewards.Reward;
			DeckList.Save();
			ArenaStats.Instance.UpdateArenaRewards();
			ArenaStats.Instance.UpdateArenaRuns();
			Close();
		}
	}
}
