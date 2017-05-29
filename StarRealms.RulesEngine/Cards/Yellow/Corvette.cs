using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class Corvette : Card, IAllied
    {
        public override string Name => "Корвет";

        public override string Description => "Корвет:\n+1 Урон, возьмите карту\n";

        public override string ImagePath => "/Resources/Cards/Corvette.jpg";

        public override int Price => 2;

        public override Faction Faction => Faction.Yellow;

        public void OnAllied(Game g)
        {
            g.CurrentPlayer.AddDamage(2);
        }

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddDamage(1);
            g.CurrentPlayer.DrawCards(1);
        }
    }
}