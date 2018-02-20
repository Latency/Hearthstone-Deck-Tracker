#region

using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HearthStone.DeckTracker.Hearthstone;
using HearthStone.DeckTracker.Utility.Logging;
using Newtonsoft.Json;

#endregion

namespace HearthStone.DeckTracker.Importing.Websites
{
	public static class Tempostorm
	{
		public static async Task<Deck> Import(string url)
		{
			try
			{
				// check url looks correct
				var match = Regex.Match(url, "/decks/([^/]+)$");
				// get deck name from url, and post the json request
				if(match.Success && match.Groups.Count == 2)
				{
					var slug = match.Groups[1].ToString();
					var param = "{\"where\":{\"slug\":\""
						+ slug
						+ "\"},\"fields\":{},\"include\":[{\"relation\":\"cards\",\"scope\":{\"include\":[\"card\"]}}]}";
					var data = await ImportingHelper.JsonRequest("https://tempostorm.com/api/decks/findOne?filter=" + param);

					//parse json
					var jsonObject = JsonConvert.DeserializeObject<dynamic>(data);
					if(jsonObject.error == null)
					{
						var deck = new Deck();

						deck.Name = jsonObject.name.ToString();
						var cards = jsonObject.cards;

						foreach(var item in cards)
						{
							var card = Hearthstone.Database.GetCardFromName(item.card.name.ToString());
							card.Count = item.cardQuantity.ToString().Equals("2") ? 2 : 1;
							deck.Cards.Add(card);
							if(string.IsNullOrEmpty(deck.Class) && card.PlayerClass != "Neutral")
								deck.Class = card.PlayerClass;
						}

						return deck;
					}
					throw new Exception("JSON request failed for '" + slug + "'.");
				}
				throw new Exception("The url (" + url + ") is not a vaild TempoStorm deck.");
			}
			catch(Exception e)
			{
				Log.Error(e);
				return null;
			}
		}
	}
}
