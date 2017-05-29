using StarRealms.RulesEngine.Abstract;

namespace StarRealms.RulesEngine.Cards
{
    public class TestCard : Card
    {
        public override string Name => "Test Card";
        public override string ImagePath => "/Resources/Cards/Scout.jpg";

        public override int Price => 0;

        public override string Description => "Test Card";

        public override Faction Faction => Faction.None;

        public override void OnPlay(Game g)
        {
        }
    }
}