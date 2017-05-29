using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class SurveyShip : Card, IScrappable
    {
        public override string Name => "Исследовательский Корабль";

        public override string Description => "Исследовательский Корабль:\n+1 Торговля, возьмите карту\nУничтожение: Ваш оппонент должен сбросить карту";

        public override string ImagePath => "/Resources/Cards/SurveyShip.jpg";

        public override int Price => 3;

        public override Faction Faction => Faction.Yellow;

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddTrade(1);
            g.CurrentPlayer.DrawCards(1);
        }

        public void OnScrapped(Game g)
        {
            g.OpposingPlayer.CardsToDiscard++;
        }
    }
}