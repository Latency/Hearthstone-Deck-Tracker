#region

using System.Windows;

#endregion

namespace Hearthstone_Deck_Tracker.Errors
{
	/// <summary>
	/// Interaction logic for ErrorItem.xaml
	/// </summary>
	public partial class ErrorItem {
		public ErrorItem()
		{
			InitializeComponent();
		}

		private void BtnCloseNews_OnClick(object sender, RoutedEventArgs e)
		{
		    if(DataContext is Error error)
				ErrorManager.RemoveError(error);
		}
	}
}
