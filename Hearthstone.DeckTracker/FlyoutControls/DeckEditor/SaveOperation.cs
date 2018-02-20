using HearthStone.DeckTracker.Hearthstone;
using HearthStone.DeckTracker.Utility;


namespace HearthStone.DeckTracker.FlyoutControls.DeckEditor
{
	public class SaveOperation
	{
		private const string CurrentVersionLoc = "MainWindow_DeckBuilder_Button_Save_Current";
		private const string SaveAsLoc = "MainWindow_DeckBuilder_Label_SaveAs";

		public SerializableVersion Version { get; set; }

		public bool IsCurrent { get; set; }

		public static SaveOperation Current(Deck deck) => new SaveOperation { Version = deck.Version, IsCurrent = true};

		public static SaveOperation MajorIncrement(Deck deck) => new SaveOperation
		{
			Version = SerializableVersion.IncreaseMajor(deck.Version)
		};

		public static SaveOperation MinorIncrement(Deck deck) => new SaveOperation
		{
			Version = SerializableVersion.IncreaseMinor(deck.Version)
		};

		public override string ToString() =>
			$"{LocUtil.Get(SaveAsLoc)} {Version.ShortVersionString}{(IsCurrent ? " " + LocUtil.Get(CurrentVersionLoc) : "")}";
	}
}
