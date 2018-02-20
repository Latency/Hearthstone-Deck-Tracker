using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using HearthStone.DeckTracker.Properties;


namespace HearthStone.DeckTracker.Utility.Updating
{
	public class StatusBarHelper : INotifyPropertyChanged
	{
		private Visibility _visibility = Visibility.Collapsed;

		public Visibility Visibility
		{
			get { return _visibility; }
			set
			{
				_visibility = value;
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
