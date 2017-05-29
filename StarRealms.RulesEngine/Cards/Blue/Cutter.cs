using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class Cutter : Card, IAllied
    {
        public override string Name => "Катер";

        public override string Description => "Катер:\n+4 Власти, +2 Торговли\nФракция: +4 Урона";

        public override string ImagePath => "/Resources/Cards/Cutter.jpg";

        public override int Price => 2;

        public override Faction Faction => Faction.Blue;

        public void OnAllied(Game g)
        {
            g.CurrentPlayer.AddDamage(4);
        }

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddAuthority(4);
            g.CurrentPlayer.AddTrade(2);
        }
    }
}