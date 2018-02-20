using System.Windows.Input;
using HearthStone.DeckTracker.FlyoutControls.Options;
using HearthStone.DeckTracker.Utility;


namespace HearthStone.DeckTracker.Controls.Information
{
	public partial class TwitchExtensionInfo
	{
		public TwitchExtensionInfo()
		{
			InitializeComponent();
		}

		public ICommand OptionsCommand => new Command(() =>
		{
			AdvancedOptions.Instance.Show = true;
			Core.MainWindow.Options.TreeViewItemStreamingTwitchExtension.IsSelected = true;
			Core.MainWindow.FlyoutOptions.IsOpen = true;
		});

		public ICommand SetupGuideCommand => new Command(() => Helper.TryOpenUrl("https://hsdecktracker.net/twitch/setup/"));
	}
}
