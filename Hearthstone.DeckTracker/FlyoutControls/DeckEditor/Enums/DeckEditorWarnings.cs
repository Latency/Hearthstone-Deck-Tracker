using System;
using HearthStone.DeckTracker.Enums;


namespace HearthStone.DeckTracker.FlyoutControls.DeckEditor.Enums
{
	[Flags]
	public enum DeckEditorWarnings
	{
		[LocDescription("Enum_DeckEditorWarnings_LessThan30")]
		LessThan30Cards = 1,
		[LocDescription("Enum_DeckEditorWarnings_MoreThan30")]
		MoreThan30Cards = 1 << 1,
		[LocDescription("Enum_DeckEditorWarnings_NameAlreadyExists")]
		NameAlreadyExists = 1 << 2,
	}
}
