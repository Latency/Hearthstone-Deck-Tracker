#region

using System.Collections.Generic;
using System.Linq;
using HearthStone.Database.Enums;
using HearthStone.DeckTracker.Hearthstone.Entities;

#endregion

namespace HearthStone.DeckTracker.Utility.BoardDamage
{
	public class PlayerBoard
	{
		// TODO: optimize this somehow
		public PlayerBoard(List<Entity> list, bool activeTurn)
		{
			Cards = new List<IBoardEntity>();
			var filtered = Filter(list);
			var weapon = GetWeapon(filtered);
			foreach(var card in filtered)
			{
				if(card.IsHero)
				{
					Hero = new BoardHero(card, weapon, activeTurn);
					Cards.Add(Hero);
				}
				else
					Cards.Add(new BoardCard(card, activeTurn));
			}
		}

		public BoardHero Hero { get; }

		public List<IBoardEntity> Cards { get; }

		public int Damage => Cards.Where(x => x.Include).Sum(x => x.Attack);

		public Entity GetWeapon(List<Entity> list)
		{
			var weapons = list.Where(x => x.IsWeapon).ToList();
			return weapons.Count == 1 ? weapons[0] : list.FirstOrDefault(x => x.HasTag(GameTag.JUST_PLAYED) && x.GetTag(GameTag.JUST_PLAYED) == 1);
		}

		public override string ToString() => $"(H:{Hero?.Health ?? 0} A:{Damage})";

		private List<Entity> Filter(List<Entity> cards)
			=>
				cards.Where(x => x != null && x.GetTag(GameTag.CARDTYPE) != (int)CardType.PLAYER &&
							x.GetTag(GameTag.CARDTYPE) != (int)CardType.ENCHANTMENT && x.GetTag(GameTag.CARDTYPE) != (int)CardType.HERO_POWER
							&& x.GetTag(GameTag.ZONE) != (int)Zone.SETASIDE && x.GetTag(GameTag.ZONE) != (int)Zone.GRAVEYARD).ToList();
	}
}
