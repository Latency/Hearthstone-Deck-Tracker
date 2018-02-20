#region

using System.ComponentModel;

#endregion

namespace HearthStone.DeckTracker.Enums
{
	public enum FilterDeckMode
	{
		[Description("With deck")]
		WithDeck,

		[Description("Without deck")]
		WithoutDeck,

		[Description("All")]
		All
	}
}
