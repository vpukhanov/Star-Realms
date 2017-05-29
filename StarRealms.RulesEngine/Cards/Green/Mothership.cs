using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class Mothership : Card, IAllied
    {
        public override string Name => "Авиабаза";

        public override string Description => "Авиабаза:\n+6 Урона, возьмите карту\nФракция: Возьмите карту";

        public override string ImagePath => "/Resources/Cards/Mothership.jpg";

        public override int Price => 7;

        public override Faction Faction => Faction.Green;

        public void OnAllied(Game g)
        {
            g.CurrentPlayer.DrawCards(1);
        }

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddTrade(6);
            g.CurrentPlayer.DrawCards(1);
        }
    }
}