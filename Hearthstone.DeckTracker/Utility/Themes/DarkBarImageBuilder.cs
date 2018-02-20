using System.Windows;
using HearthStone.DeckTracker.Hearthstone;
using HearthStone.DeckTracker.Utility.Extensions;


namespace HearthStone.DeckTracker.Utility.Themes
{
	public class DarkBarImageBuilder : CardBarImageBuilder
	{
		private readonly Rect _fadeRect = new Rect(34, 0, 183, 34);
		public DarkBarImageBuilder(Card card, string dir) : base(card, dir)
		{
		}

		protected override void AddFadeOverlay() => AddFadeOverlay(_fadeRect, true);

		protected override void AddCardImage() => AddCardImage(ImageRect, true);

		protected override void AddCountText() => AddCountText(CountTextRect.Move(2, 0));
	}
}
