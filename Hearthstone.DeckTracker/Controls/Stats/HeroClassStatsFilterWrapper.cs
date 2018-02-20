using System.Windows;
using System.Windows.Media.Imaging;
using HearthStone.DeckTracker.Enums;
using HearthStone.DeckTracker.Utility;


namespace HearthStone.DeckTracker.Controls.Stats
{
	public class HeroClassStatsFilterWrapper
	{
		public HeroClassStatsFilterWrapper(HeroClassStatsFilter heroClass)
		{
			HeroClass = heroClass;
		}

		public HeroClassStatsFilter HeroClass { get; }

		public BitmapImage ClassImage => ImageCache.GetClassIcon(HeroClass.ToString());

		public Visibility ImageVisibility => HeroClass == HeroClassStatsFilter.All ? Visibility.Collapsed : Visibility.Visible;

		public override bool Equals(object obj)
		{
			var wrapper = obj as HeroClassStatsFilterWrapper;
			return wrapper != null && HeroClass.Equals(wrapper.HeroClass);
		}

		public override int GetHashCode() => HeroClass.GetHashCode();
	}
}
