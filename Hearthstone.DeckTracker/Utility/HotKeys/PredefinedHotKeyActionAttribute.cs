#region

using System;

#endregion

namespace HearthStone.DeckTracker.Utility.HotKeys
{
	public class PredefinedHotKeyActionAttribute : Attribute
	{
		public PredefinedHotKeyActionAttribute(string title, string description)
		{
			Title = title;
			Description = description;
		}

		public string Title { get; set; }
		public string Description { get; set; }
	}
}
