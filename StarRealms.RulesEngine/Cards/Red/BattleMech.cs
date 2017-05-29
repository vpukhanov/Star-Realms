using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class BattleMech : Card, IAllied
    {
        public override string Name => "Боевой Механизм";

        public override string Description => "Боевой Механизм:\n+4 Урона\nВы можете уничтожить карту в руке или в сбросе\nФракция: возьмите карту";

        public override string ImagePath => "/Resources/Cards/BattleMech.jpg";

        public override int Price => 5;

        public override Faction Faction => Faction.Red;

        public void OnAllied(Game g)
        {
            g.CurrentPlayer.DrawCards(1);
        }

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddDamage(4);

            if (g.CurrentPlayer.ShouldScrapCardInHand(g))
            {
                g.CurrentPlayer.ScrapCardInHand(g);
            }
            else if (g.CurrentPlayer.ShouldScrapCardInDiscard(g))
            {
                g.CurrentPlayer.ScrapCardInDiscard(g);
            }
        }
    }
}