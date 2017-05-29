using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class Dreadnaught : Card, IScrappable
    {
        public override string Name => "Дредноут";

        public override string Description => "Дредноут:\n+7 Урона, возьмите карту\nУничтожение: +5 Урона";

        public override string ImagePath => "/Resources/Cards/Dreadnaught.jpg";

        public override int Price => 7;

        public override Faction Faction => Faction.Yellow;

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddDamage(7);
            g.CurrentPlayer.DrawCards(1);
        }

        public void OnScrapped(Game g)
        {
            g.CurrentPlayer.AddDamage(5);
        }
    }
}