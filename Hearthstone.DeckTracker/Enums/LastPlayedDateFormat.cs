using System.ComponentModel;


namespace HearthStone.DeckTracker.Enums
{
	public enum LastPlayedDateFormat
	{
		[Description("dd/MM/yyyy")]
		DayMonthYear,
		[Description("MM/dd/yyyy")]
		MonthDayYear
	}
}
