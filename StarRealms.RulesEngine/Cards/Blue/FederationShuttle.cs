using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class FederationShuttle : Card, IAllied
    {
        public override string Name => "Шаттл Федерации";

        public override string Description => "Шаттл Федерации:\n+2 Торговли\nФракция: +4 Власти";

        public override string ImagePath => "/Resources/Cards/FederationShuttle.jpg";

        public override int Price => 1;

        public override Faction Faction => Faction.Blue;

        public void OnAllied(Game g)
        {
            g.CurrentPlayer.AddAuthority(4);
        }

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddTrade(2);
        }
    }
}