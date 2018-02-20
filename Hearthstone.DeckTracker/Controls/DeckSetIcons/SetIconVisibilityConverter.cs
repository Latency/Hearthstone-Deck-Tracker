using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using HearthStone.Database.Enums;
using HearthStone.DeckTracker.Hearthstone;
using Type = System.Type;

namespace HearthStone.DeckTracker.Controls.DeckSetIcons
{
	public class SetIconVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(!(value is Deck deck))
				return Visibility.Collapsed;
			if(!Enum.TryParse(parameter?.ToString(), out CardSet set))
				return Visibility.Collapsed;
			return deck.ContainsSet(set) ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
