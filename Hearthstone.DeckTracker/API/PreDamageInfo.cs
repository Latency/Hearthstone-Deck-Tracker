using HearthStone.DeckTracker.Hearthstone.Entities;


namespace HearthStone.DeckTracker.API
{
	public class PredamageInfo
	{
		public Entity Entity { get; }
		public int Value { get; }

		public PredamageInfo(Entity entity, int value)
		{
			Entity = entity;
			Value = value;
		}
	}
}
