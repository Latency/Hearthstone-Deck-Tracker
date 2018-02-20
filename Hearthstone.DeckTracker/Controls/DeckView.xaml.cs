using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HearthStone.Database.Enums;
using HearthStone.DeckTracker.Hearthstone;
using HearthStone.DeckTracker.Utility;
using HearthStone.DeckTracker.Utility.Extensions;
using CardIds = HearthStone.Database.CardIds;


namespace HearthStone.DeckTracker.Controls
{
	public partial class DeckView
	{
		private readonly string _allTags;

		public DeckView(Deck deck, bool deckOnly = false)
		{
			InitializeComponent();
			_allTags = deck.TagList.ToLowerInvariant().Replace("-", "");
			ListViewPlayer.Update(deck.Cards.ToSortedCardList(), true);

			if(deckOnly)
			{
				DeckTitleContainer.Visibility = Visibility.Collapsed;
				DeckFormatPanel.Visibility = Visibility.Collapsed;
				SetDustPanel.Visibility = Visibility.Collapsed;
				BrandContainer.Visibility = Visibility.Collapsed;
			}
			else
			{
				DeckTitlePanel.Background = DeckHeaderBackground(deck.Class);
				LblDeckTitle.Text = deck.Name;
				LblDeckTag.Text = GetTagText(deck);
				LblDeckFormat.Text = GetFormatText(deck);
				LblDustCost.Text = TotalDust(deck).ToString();
				ShowFormatIcon(deck);
				SetIcons.Update(deck);
			}
		}

		private ImageBrush DeckHeaderBackground(string deckClass)
		{
			var heroId = ClassToID(deckClass);
			var drawingGroup = new DrawingGroup();
			var img = ImageCache.GetCardImage(Hearthstone.Database.GetCardFromId(heroId));
			drawingGroup.Children.Add(new ImageDrawing(img, new Rect(54, 0, 130, 34)));
			drawingGroup.Children.Add(new ImageDrawing(new BitmapImage(new Uri(
				"Images/Themes/Bars/dark/fade.png", UriKind.Relative)), new Rect(0, 0, 183, 34)));

			return new ImageBrush {
				ImageSource = new DrawingImage(drawingGroup),
				AlignmentX = AlignmentX.Left,
				Stretch = Stretch.UniformToFill
			};
		}

		private string GetTagText(Deck deck)
		{
			var predefined = new List<string>() {
				"Midrange",
				"Aggro",
				"Control",
				"Tempo",
				"Combo"
			};

			if(deck.Tags.Count > 0)
				foreach(var tag in predefined)
					if(_allTags.Contains(tag.ToLowerInvariant()))
						return tag;

			return deck.Class;
		}

		private string GetFormatText(Deck deck)
		{
			if(deck.IsArenaDeck)
				return "Arena";
			if(_allTags.Contains("brawl"))
				return "Brawl";
			if(_allTags.Contains("adventure") || _allTags.Contains("pve"))
				return "Adventure";
			if(deck.IsDungeonDeck)
				return "Dungeon";
			if(deck.StandardViable)
				return "Standard";
			return "Wild";
		}

		private void ShowFormatIcon(Deck deck)
		{
			RectIconStandard.Visibility = Visibility.Collapsed;
			RectIconWild.Visibility = Visibility.Collapsed;
			RectIconArena.Visibility = Visibility.Collapsed;
			RectIconBrawl.Visibility = Visibility.Collapsed;
			RectIconAdventure.Visibility = Visibility.Collapsed;

			if(deck.IsArenaDeck)
				RectIconArena.Visibility = Visibility.Visible;
			else if(_allTags.Contains("brawl"))
				RectIconBrawl.Visibility = Visibility.Visible;
			else if(_allTags.Contains("adventure") || _allTags.Contains("pve") || deck.IsDungeonDeck)
				RectIconAdventure.Visibility = Visibility.Visible;
			else if(deck.StandardViable)
				RectIconStandard.Visibility = Visibility.Visible;
			else
				RectIconWild.Visibility = Visibility.Visible;
		}

		private int TotalDust(Deck deck)
		{
			var nonCraftableSets = new[]
			{
				CardSet.KARA,
				CardSet.NAXX,
				CardSet.BRM,
				CardSet.LOE,
				CardSet.CORE
			}.Select(HearthDbConverter.SetConverter).ToList();
			var nonCraftableCards = new List<string>() {
				CardIds.Collectible.Neutral.Cthun,
				CardIds.Collectible.Neutral.BeckonerOfEvil
			};

			return deck.Cards
				.Where(c => !nonCraftableSets.Contains(c.Set) && !nonCraftableCards.Contains(c.Id))
				.Sum(c => c.DustCost * c.Count);
		}

		private string ClassToID(string klass)
		{
			switch(klass.ToLowerInvariant())
			{
				case "druid":
					return CardIds.Collectible.Druid.MalfurionStormrage;
				case "hunter":
					return CardIds.Collectible.Hunter.Rexxar;
				case "mage":
					return CardIds.Collectible.Mage.JainaProudmoore;
				case "paladin":
					return CardIds.Collectible.Paladin.UtherLightbringer;
				case "priest":
					return CardIds.Collectible.Priest.AnduinWrynn;
				case "rogue":
					return CardIds.Collectible.Rogue.ValeeraSanguinar;
				case "shaman":
					return CardIds.Collectible.Shaman.Thrall;
				case "warlock":
					return CardIds.Collectible.Warlock.Guldan;
				case "warrior":
				default:
					return CardIds.Collectible.Warrior.GarroshHellscream;
			}
		}
	}
}
