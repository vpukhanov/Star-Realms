using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class TradePod : Card, IAllied
    {
        public override string Name => "Торговая капсула";

        public override string Description => "Торговая капсула:\n+3 Торговли\nФракция: +2 Урона";

        public override string ImagePath => "/Resources/Cards/TradePod.jpg";

        public override int Price => 2;

        public override Faction Faction => Faction.Green;

        public void OnAllied(Game g)
        {
            g.CurrentPlayer.AddDamage(2);
        }

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddTrade(3);
        }
    }
}