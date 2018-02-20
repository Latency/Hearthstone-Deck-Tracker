using System.ComponentModel;
using System.Runtime.CompilerServices;
using HearthStone.DeckTracker.Properties;


namespace HearthStone.DeckTracker.Utility.MVVM
{
	public class ViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
