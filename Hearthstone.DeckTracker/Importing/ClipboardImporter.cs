using System;
using System.Threading.Tasks;
using System.Windows;
using HearthStone.Database.Deckstrings;
using HearthStone.DeckTracker.Hearthstone;
using HearthStone.DeckTracker.Utility.Logging;
using Deck = HearthStone.DeckTracker.Hearthstone.Deck;

namespace HearthStone.DeckTracker.Importing
{
	public static class ClipboardImporter
	{
		public static async Task<Deck> Import()
		{
			try
			{
				var clipboard = Clipboard.GetText();
				var validUrl = Uri.TryCreate(clipboard, UriKind.Absolute, out Uri uriResult)
								&& (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
				if(validUrl)
					return await DeckImporter.Import(clipboard);

				try
				{
					var hearthDbDeck = DeckSerializer.Deserialize(clipboard);
					var deck = HearthDbConverter.FromHearthDbDeck(hearthDbDeck);
					if(deck != null)
						return deck;
				}
				catch(Exception e)
				{
					Log.Error(e);
				}

				if(StringImporter.IsValidImportString(clipboard))
					return StringImporter.Import(clipboard);
				return JsonDeckImporter.Import(clipboard);
			}
			catch(Exception ex)
			{
				Log.Error(ex);
				return null;
			}
		}
	}
}
