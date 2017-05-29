using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class Ram : Card, IAllied, IScrappable
    {
        public override string Name => "Таран";

        public override string Description => "Таран:\n+5 Урона\nФракция: +2 урона\nУничтожение: +3 Торговли";

        public override string ImagePath => "/Resources/Cards/Ram.jpg";

        public override int Price => 3;

        public override Faction Faction => Faction.Green;

        public void OnAllied(Game g)
        {
            g.CurrentPlayer.AddDamage(2);
        }

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddDamage(5);
        }

        public void OnScrapped(Game g)
        {
            g.CurrentPlayer.AddTrade(3);
        }
    }
}