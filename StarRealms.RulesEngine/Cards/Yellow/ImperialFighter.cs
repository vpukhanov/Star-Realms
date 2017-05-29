using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class ImperialFighter : Card, IAllied
    {
        public override string Name => "Имперский Истребитель";

        public override string Description => "Имперский Истребитель:\n+2 Урона\nВаш оппонент должен сбросить карту\nФракция: +2 Урона";

        public override string ImagePath => "/Resources/Cards/ImperialFighter.jpg";

        public override int Price => 1;

        public override Faction Faction => Faction.Yellow;

        public void OnAllied(Game g)
        {
            g.CurrentPlayer.AddDamage(2);
        }

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddDamage(2);
            g.OpposingPlayer.CardsToDiscard++;
        }
    }
}