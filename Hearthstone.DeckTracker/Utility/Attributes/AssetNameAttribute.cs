using System;


namespace HearthStone.DeckTracker.Utility.Attributes
{
	public class AssetNameAttribute : Attribute
	{
		public string Name { get; }

		public AssetNameAttribute(string name)
		{
			Name = name;
		}
	}
}
