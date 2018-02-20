using System.Windows.Controls;
using System.Windows.Input;
using HearthStone.DeckTracker.Utility;
using HearthStone.DeckTracker.Utility.MVVM;


namespace HearthStone.DeckTracker.Windows.MainWindowControls
{
	public class NewsBarViewModel : ViewModel
	{
		private TextBlock _newsContent;
		private string _indexContent;

		public ICommand PreviousItemCommand => new Command(NewsManager.PreviousNewsItem);
		public ICommand NextItemCommand => new Command(NewsManager.NextNewsItem);
		public ICommand CloseCommand => new Command(NewsManager.ToggleNewsVisibility);

		public TextBlock NewsContent
		{
			get => _newsContent;
			set
			{
				_newsContent = value;
				OnPropertyChanged();
			}
		}

		public string IndexContent
		{
			get => _indexContent;
			set
			{
				_indexContent = value;
				OnPropertyChanged();
			}
		}
	}
}
