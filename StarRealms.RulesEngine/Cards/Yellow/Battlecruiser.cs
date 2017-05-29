using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class Battlecruiser : Card, IAllied, IScrappable
    {
        public override string Name => "Крейсер";

        public override string Description => "Крейсер:\n+5 Урона, возьмите карту\nФракция: Ваш оппонент должен сбросить карту\nУничтожение: Возьмите карту. Вы можете уничтожить выбранную базу";

        public override string ImagePath => "/Resources/Cards/Battlecruiser.jpg";

        public override int Price => 6;

        public override Faction Faction => Faction.Yellow;

        public void OnAllied(Game g)
        {
            g.OpposingPlayer.CardsToDiscard++;
        }

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddDamage(5);
            g.CurrentPlayer.DrawCards(1);
        }

        public void OnScrapped(Game g)
        {
            g.CurrentPlayer.DrawCards(1);
            // TODO: дополнить, когда будут готовы базы
        }
    }
}