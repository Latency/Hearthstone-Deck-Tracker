using System;
using System.Globalization;
using System.Windows.Data;


namespace HearthStone.DeckTracker.Controls.Stats.Converters
{
	public class InverseBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(targetType != typeof(bool))
				return null;
			return !(bool)value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(targetType != typeof(bool))
				return null;
			return !(bool)value;
		}
	}
}
