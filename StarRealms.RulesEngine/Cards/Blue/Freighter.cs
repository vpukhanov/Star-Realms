using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class Freighter : Card, IAllied
    {
        public override string Name => "Торговое Судно";

        public override string Description => "Торговое Судно:\n+4 Торговли\nФракция: Положите следующий купленный в этот ход корабль на верх своей колоды";

        public override string ImagePath => "/Resources/Cards/Freighter.jpg";

        public override int Price => 4;

        public override Faction Faction => Faction.Blue;

        public void OnAllied(Game g)
        {
            g.CurrentPlayer.MakeNextShipOnTop();
        }

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddTrade(4);
        }
    }
}