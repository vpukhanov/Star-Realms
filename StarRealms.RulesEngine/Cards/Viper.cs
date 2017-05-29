using StarRealms.RulesEngine.Abstract;

namespace StarRealms.RulesEngine.Cards
{
    public class Viper : Card
    {
        public override string Name => "Вайпер";

        public override string ImagePath => "/Resources/Cards/Viper.jpg";

        public override int Price => 0;

        public override string Description => "Вайпер:\n+1 Урон";

        public override Faction Faction => Faction.None;

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddDamage(1);
        }
    }
}