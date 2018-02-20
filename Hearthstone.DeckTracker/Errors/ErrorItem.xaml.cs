#region

using System.Windows;
using System.Windows.Controls;

#endregion

namespace HearthStone.DeckTracker.Errors
{
	/// <summary>
	/// Interaction logic for ErrorItem.xaml
	/// </summary>
	public partial class ErrorItem : UserControl
	{
		public ErrorItem()
		{
			InitializeComponent();
		}

		private void BtnCloseNews_OnClick(object sender, RoutedEventArgs e)
		{
			var error = DataContext as Error;
			if(error != null)
				ErrorManager.RemoveError(error);
		}
	}
}
