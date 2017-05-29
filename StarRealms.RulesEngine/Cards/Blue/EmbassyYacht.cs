using StarRealms.RulesEngine.Abstract;

namespace StarRealms.RulesEngine.Cards
{
    public class EmbassyYacht : Card
    {
        public override string Name => "Яхта Посольства";

        public override string Description => "Яхта Посольства:\n+3 Власти, +2 Торговли\nЕсли у вас есть две или больше баз, возьмите 2 карты";

        public override string ImagePath => "/Resources/Cards/EmbassyYacht.jpg";

        public override int Price => 3;

        public override Faction Faction => Faction.Blue;

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddAuthority(3);
            g.CurrentPlayer.AddTrade(2);
            // TODO: после добавления баз, активировать доп. эффект
        }
    }
}