using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class BattlePod : Card, IAllied
    {
        public override string Name => "Боевая капсула";

        public override string Description => "Боевая капсула:\n+4 Урона\nВы можете уничтожить карту в торговом ряду\nФракция: +2 Урона";

        public override string ImagePath => "/Resources/Cards/BattlePod.jpg";

        public override int Price => 2;

        public override Faction Faction => Faction.Green;

        public void OnAllied(Game g)
        {
            g.CurrentPlayer.AddDamage(2);
        }

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddDamage(4);
            if (g.CurrentPlayer.ShouldScrapCardInTradeRow(g))
            {
                g.CurrentPlayer.ScrapCardInTradeRow(g);
            }
        }
    }
}