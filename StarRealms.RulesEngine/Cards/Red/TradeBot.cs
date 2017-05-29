using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class TradeBot : Card, IAllied
    {
        public override string Name => "Торговый Бот";

        public override string Description => "Торговый Бот:\n+1 Торговля\nВы можете уничтожить карту в руке или в сбросе\nФракция: +2 Урона";

        public override string ImagePath => "/Resources/Cards/TradeBot.jpg";

        public override int Price => 1;

        public override Faction Faction => Faction.Red;

        public void OnAllied(Game g)
        {
            g.CurrentPlayer.AddDamage(2);
        }

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddTrade(1);

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