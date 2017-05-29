using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class ImperialFrigate : Card, IAllied, IScrappable
    {
        public override string Name => "Имперский Фрегат";

        public override string Description => "Имперский Фрегат:\n+4 Урона\nВаш оппонент должен сбросить карту\nФракция: +2 Урона\nУничтожение: Возьмите карту";

        public override string ImagePath => "/Resources/Cards/ImperialFrigate.jpg";

        public override int Price => 3;

        public override Faction Faction => Faction.Yellow;

        public void OnAllied(Game g)
        {
            g.CurrentPlayer.AddDamage(2);
        }

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddDamage(4);
            g.OpposingPlayer.CardsToDiscard++;
        }

        public void OnScrapped(Game g)
        {
            g.CurrentPlayer.DrawCards(1);
        }
    }
}