using System.Xml.Serialization;


namespace HearthStone.DeckTracker.Stats
{
	public class TrackedCard
	{
		[XmlAttribute]
		public string Id { get; set; }

		[XmlAttribute]
		public int Count { get; set; }

		[XmlAttribute]
		public int Unconfirmed { get; set; }

		public bool ShouldSerializeUnconfirmed() => Unconfirmed > 0;

		public TrackedCard()
		{
			
		}

		public TrackedCard(string id, int count, int unconfirmed = 0)
		{
			Id = id;
			Count = count;
			Unconfirmed = unconfirmed;
		}
	}
}
