using HearthStone.DeckTracker.Hearthstone;


namespace HearthStone.DeckTracker.API
{
	public class AttackInfo
	{
		public Card Attacker { get; }
		public Card Defender { get; }

		public AttackInfo(Card attacker, Card defender)
		{
			Attacker = attacker;
			Defender = defender;
		}
	}
}
