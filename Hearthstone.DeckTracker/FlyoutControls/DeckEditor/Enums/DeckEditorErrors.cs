using System;
using HearthStone.DeckTracker.Enums;


namespace HearthStone.DeckTracker.FlyoutControls.DeckEditor.Enums
{
	[Flags]
	public enum DeckEditorErrors
	{
		[LocDescription("Enum_DeckEditorErrors_NameRequired")]
		NameRequired = 1
	}
}
