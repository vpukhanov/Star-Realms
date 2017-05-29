using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class Flagship : Card, IAllied
    {
        public override string Name => "Флагман";

        public override string Description => "Флагман:\n+5 Урона, возьмите карту\nФракция: +5 Власти";

        public override string ImagePath => "/Resources/Cards/Flagship.jpg";

        public override int Price => 6;

        public override Faction Faction => Faction.Blue;

        public void OnAllied(Game g)
        {
            g.CurrentPlayer.AddAuthority(5);
        }

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddDamage(5);
            g.CurrentPlayer.DrawCards(1);
        }
    }
}