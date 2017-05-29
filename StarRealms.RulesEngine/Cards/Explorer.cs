using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class Explorer : Card, IScrappable
    {
        public override string Name => "Исследователь";

        public override string ImagePath => "/Resources/Cards/Explorer.jpg";

        public override int Price => 2;

        public override string Description => "Исследователь:\n+2 Торговли\nУничтожение: +2 Урона";

        public override Faction Faction => Faction.None;

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddTrade(2);
        }

        public void OnScrapped(Game g)
        {
            g.CurrentPlayer.AddDamage(2);
        }
    }
}