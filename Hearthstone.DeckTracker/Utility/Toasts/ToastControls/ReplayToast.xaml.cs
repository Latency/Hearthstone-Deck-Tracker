#region

using System.Threading.Tasks;
using System.Windows.Input;
using HearthStone.DeckTracker.HsReplay;
using HearthStone.DeckTracker.Properties;
using HearthStone.DeckTracker.Stats;
using HearthStone.DeckTracker.Utility.Extensions;

#endregion

namespace HearthStone.DeckTracker.Utility.Toasts.ToastControls
{
	/// <summary>
	/// Interaction logic for ReplayToast.xaml
	/// </summary>
	public partial class ReplayToast
	{
		private readonly GameStats _game;

		public ReplayToast([NotNull] GameStats game)
		{
			InitializeComponent();
			_game = game;
		}

		private async void BorderReplay_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			ToastManager.ForceCloseToast(this);
			await Task.Delay(500);
			ReplayLauncher.ShowReplay(_game, true).Forget();
		}

		private void BorderReplay_OnMouseEnter(object sender, MouseEventArgs e)
		{
			if(Cursor != Cursors.Wait)
				Cursor = Cursors.Hand;
		}

		private void BorderReplay_OnMouseLeave(object sender, MouseEventArgs e)
		{
			if(Cursor != Cursors.Wait)
				Cursor = Cursors.Arrow;
		}
	}
}
