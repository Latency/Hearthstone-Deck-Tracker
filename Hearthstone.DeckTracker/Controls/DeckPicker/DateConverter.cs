#region

using System;
using System.Globalization;
using System.Windows.Data;
using HearthStone.DeckTracker.Enums;

#endregion

namespace HearthStone.DeckTracker.Controls.DeckPicker
{
	public class DateConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(!(value is DateTime))
				return string.Empty;
			var date = (DateTime)value;
			if(date == DateTime.MinValue)
				return "N/A";
			return date.ToString(EnumDescriptionConverter.GetDescription(Config.Instance.LastPlayedDateFormat), culture);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
	}
}
