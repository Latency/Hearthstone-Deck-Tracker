#region

using System.Windows;
using System.Windows.Controls;
using HearthStone.DeckTracker.Stats.CompiledStats;

#endregion

namespace HearthStone.DeckTracker.Controls.Stats.Arena
{
	/// <summary>
	/// Interaction logic for ArenaAdvancedCharts.xaml
	/// </summary>
	public partial class ArenaAdvancedCharts : UserControl
	{
		public ArenaAdvancedCharts()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e) => ArenaStats.Instance.UpdateExpensiveArenaStats();
	}
}
