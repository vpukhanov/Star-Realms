using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class MissileMech : Card, IAllied
    {
        public override string Name => "Ракетный Механизм";

        public override string Description => "Ракетный Механизм:\n+6 Урона\nВы можете уничтожить выбранную базу\nФракция: Возьмите карту";

        public override string ImagePath => "/Resources/Cards/MissileMech.jpg";

        public override int Price => 6;

        public override Faction Faction => Faction.Red;

        public void OnAllied(Game g)
        {
            g.CurrentPlayer.DrawCards(1);
        }

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddDamage(6);
            // TODO: Дополнить, когда будут сделаны базы
        }
    }
}