using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;
using System.Collections.Generic;

namespace StarRealms.RulesEngine.Cards
{
    public class PatrolMech : Card, IAllied
    {
        public override string Name => "Патрульный Механизм";

        public override string Description => "Патрульный Механизм:\n+3 Торговли или +5 Урона\nФракция: Вы можете уничтожить карту в руке или в сбросе";

        public override string ImagePath => "/Resources/Cards/PatrolMech.jpg";

        public override int Price => 4;

        public override Faction Faction => Faction.Red;

        private static List<string> choices = new List<string>
        {
            "+3 Торговли",
            "+5 Урона"
        };

        public void OnAllied(Game g)
        {
            if (g.CurrentPlayer.ShouldScrapCardInHand(g))
            {
                g.CurrentPlayer.ScrapCardInHand(g);
            }
            else if (g.CurrentPlayer.ShouldScrapCardInDiscard(g))
            {
                g.CurrentPlayer.ScrapCardInDiscard(g);
            }
        }

        public override void OnPlay(Game g)
        {
            int choice = g.CurrentPlayer.MakeCustomChoice(g, PatrolMech.choices);

            switch (choice)
            {
                case 0:
                    g.CurrentPlayer.AddTrade(3);
                    break;

                case 1:
                    g.CurrentPlayer.AddDamage(5);
                    break;
            }
        }
    }
}