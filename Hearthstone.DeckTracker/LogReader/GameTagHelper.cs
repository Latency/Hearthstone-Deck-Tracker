#region

using System;
using HearthStone.Database.Enums;

#endregion

namespace HearthStone.DeckTracker.LogReader
{
	public class GameTagHelper
	{
		public static int ParseTag(GameTag tag, string rawValue)
		{
			switch(tag)
			{
				case GameTag.ZONE:
					return (int)ParseEnum<Zone>(rawValue);
				case GameTag.MULLIGAN_STATE:
					return (int)ParseEnum<Mulligan>(rawValue);
				case GameTag.PLAYSTATE:
					return (int)ParseEnum<PlayState>(rawValue);
				case GameTag.CARDTYPE:
					return (int)ParseEnum<CardType>(rawValue);
				case GameTag.CLASS:
					return (int)ParseEnum<CardClass>(rawValue);
				case GameTag.STATE:
					return (int)ParseEnum<State>(rawValue);
				case GameTag.STEP:
					return (int)ParseEnum<Step>(rawValue);
				default:
					int value;
					int.TryParse(rawValue, out value);
					return value;
			}
		}

		public static TEnum ParseEnum<TEnum>(string value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			TEnum tEnum;
			if(Enum.TryParse(value, out tEnum))
				return tEnum;
			int i;
			if(int.TryParse(value, out i) && Enum.IsDefined(typeof(TEnum), i))
				tEnum = (TEnum)(object)i;
			return tEnum;
		}
	}
}
