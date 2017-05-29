using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class SupplyBot : Card, IAllied
    {
        public override string Name => "Бот-снабженец";

        public override string Description => "Бот-снабженец:\n+2 Торговли\nВы можете уничтожить карту в руке или в сбросе\nФракция: +2 Урона";

        public override string ImagePath => "/Resources/Cards/SupplyBot.jpg";

        public override int Price => 3;

        public override Faction Faction => Faction.Red;

        public void OnAllied(Game g)
        {
            g.CurrentPlayer.AddDamage(2);
        }

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddTrade(2);

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