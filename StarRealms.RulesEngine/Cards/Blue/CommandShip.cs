using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class CommandShip : Card, IAllied
    {
        public override string Name => "Командное Судно";

        public override string Description => "Командное Судно:\n+4 Власти, +5 Урона\nВозьмите 2 карты\nФракция: Вы можете уничтожить выбранную базу";

        public override string ImagePath => "/Resources/Cards/CommandShip.jpg";

        public override int Price => 8;

        public override Faction Faction => Faction.Blue;

        public void OnAllied(Game g)
        {
            // TODO: переделать после добавления баз
        }

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddAuthority(4);
            g.CurrentPlayer.AddDamage(5);
            g.CurrentPlayer.DrawCards(2);
        }
    }
}