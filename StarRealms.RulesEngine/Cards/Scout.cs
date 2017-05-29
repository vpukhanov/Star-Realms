using StarRealms.RulesEngine.Abstract;

namespace StarRealms.RulesEngine.Cards
{
    public class Scout : Card
    {
        public override string Name => "Cкаут";

        public override string ImagePath => "/Resources/Cards/Scout.jpg";

        public override int Price => 0;

        public override string Description => "Скаут:\n+1 Торговля";

        public override Faction Faction => Faction.None;

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddTrade(1);
        }
    }
}