using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class TradeEscort : Card, IAllied
    {
        public override string Name => "Торговый Конвой";

        public override string Description => "Торговый Конвой:\n+4 Власти, +4 Урона\nФракция: Возьмите карту";

        public override string ImagePath => "/Resources/Cards/TradeEscort.jpg";

        public override int Price => 5;

        public override Faction Faction => Faction.Blue;

        public void OnAllied(Game g)
        {
            g.CurrentPlayer.DrawCards(1);
        }

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddAuthority(4);
            g.CurrentPlayer.AddDamage(4);
        }
    }
}