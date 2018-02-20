using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HearthStone.Database.Deckstrings;
using HearthStone.DeckTracker.Hearthstone;
using HearthStone.DeckTracker.Utility.Logging;
using HtmlAgilityPack;
using Deck = HearthStone.DeckTracker.Hearthstone.Deck;

namespace HearthStone.DeckTracker.Importing
{
	public class MetaTagImporter
	{
		public static async Task<Deck> TryFindDeck(string url)
		{
			try
			{
				var doc = await ImportingHelper.GetHtmlDoc(url);
				var deck = new Deck();
				var metaNodes = doc.DocumentNode.SelectNodes("//meta");
				if(!metaNodes.Any())
					return null;
				deck.Name = GetMetaProperty(metaNodes, "x-hearthstone:deck");
				deck.Url = GetMetaProperty(metaNodes, "x-hearthstone:deck:url") ?? url;

				var deckString = GetMetaProperty(metaNodes, "x-hearthstone:deck:deckstring");
				if(!string.IsNullOrEmpty(deckString))
				{
					try
					{
						var hearthDbDeck = DeckSerializer.Deserialize(deckString);
						var fromDeckString = HearthDbConverter.FromHearthDbDeck(hearthDbDeck);
						fromDeckString.Name = deck.Name;
						fromDeckString.Url = deck.Url;
						return fromDeckString;
						
					}
					catch(Exception e)
					{
						Log.Error(e);
					}
				}

				var heroId = GetMetaProperty(metaNodes, "x-hearthstone:deck:hero");
				if(!string.IsNullOrEmpty(heroId))
					deck.Class = Hearthstone.Database.GetCardFromId(heroId).PlayerClass;
				var cardList = GetMetaProperty(metaNodes, "x-hearthstone:deck:cards").Split(',');
				foreach(var idGroup in cardList.GroupBy(x => x))
				{
					var card = Hearthstone.Database.GetCardFromId(idGroup.Key);
					card.Count = idGroup.Count();
					deck.Cards.Add(card);
					if(deck.Class == null && card.IsClassCard)
						deck.Class = card.PlayerClass;
				}
				return deck;
			}
			catch(Exception e)
			{
				Log.Error(e);
				return null;
			}
		}

		private static string GetMetaProperty(HtmlNodeCollection nodes, string prop) 
			=> HttpUtility.HtmlDecode(nodes.FirstOrDefault(x => x.Attributes["property"]?.Value == prop)?.Attributes["content"]?.Value);
	}
}
