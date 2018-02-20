#region

using System.ComponentModel;
using System.Runtime.CompilerServices;
using HearthStone.DeckTracker.Controls.Stats.Arena.Charts;
using HearthStone.DeckTracker.Properties;

#endregion

namespace HearthStone.DeckTracker.Controls.Stats.Arena
{
	/// <summary>
	/// Interaction logic for ArenaRuns.xaml
	/// </summary>
	public partial class ArenaRuns : INotifyPropertyChanged
	{
		private object _chartWinsControl = new ChartWins();

		public ArenaRuns()
		{
			InitializeComponent();
		}

		public object ChartWinsControl
		{
			get { return _chartWinsControl; }
			set
			{
				_chartWinsControl = value;
				OnPropertyChanged();
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
