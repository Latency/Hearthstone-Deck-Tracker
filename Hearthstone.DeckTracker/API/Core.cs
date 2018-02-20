#region

using System.Windows.Controls;
using HearthStone.DeckTracker.Hearthstone;
using HearthStone.DeckTracker.Windows;

#endregion

namespace HearthStone.DeckTracker.API
{
	public class Core
	{
		public static GameV2 Game => DeckTracker.Core.Game;

		public static Canvas OverlayCanvas => DeckTracker.Core.Overlay.CanvasInfo;

		public static OverlayWindow OverlayWindow => DeckTracker.Core.Overlay;

		public static MainWindow MainWindow => DeckTracker.Core.MainWindow;
	}
}
