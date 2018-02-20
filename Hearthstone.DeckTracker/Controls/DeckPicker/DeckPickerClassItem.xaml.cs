#region

using System.Windows.Media.Imaging;
using HearthStone.DeckTracker.Enums;
using HearthStone.DeckTracker.Utility;

#endregion

namespace HearthStone.DeckTracker.Controls.DeckPicker
{
	/// <summary>
	/// Interaction logic for DeckPickerClassItem.xaml
	/// </summary>
	public partial class DeckPickerClassItem
	{
		public DeckPickerClassItem()
		{
			InitializeComponent();
		}

		public static int Small => 24;

		public static int Big => 36;

		public BitmapImage ClassImage
		{
			get
			{
				var heroClass = DataContext as HeroClassAll?;
				return heroClass == null ? new BitmapImage() : ImageCache.GetClassIcon(heroClass.Value);
			}
		}

		public void OnSelected()
		{
		}

		public void OnDelselected()
		{
		}
	}
}
