using System;
using System.Collections.Generic;
using System.Linq;
using HearthStone.DeckTracker.Utility.Logging;
using HearthStone.Replay;


namespace HearthStone.DeckTracker.HsReplay
{
	public static class PackDataGenerator
	{
		public static PackData Generate(int packId, IEnumerable<CardData> cardData)
		{
			var cards = cardData.ToArray();
			if(cards.Length != 5)
			{
				Log.Error("Invalid card count: " + cards.Length);
				return null;
			}
			var accId = Mirror.Reflection.GetAccountId();
			if(accId == null || accId.Hi == 0 || accId.Lo == 0)
			{
				Log.Error("Could not get account id");
				return null;
			}
			var data = new PackData
			{
				AccountHi = accId.Hi,
				AccountLo = accId.Lo,
				BoosterType = packId,
				Date = DateTime.Now.ToString("o"),
				Cards = cards

			};
			return data;
		}
	}
}
