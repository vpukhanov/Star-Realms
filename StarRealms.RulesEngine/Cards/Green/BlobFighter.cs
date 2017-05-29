using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class BlobFighter : Card, IAllied
    {
        public override string Name => "Слизень-истребитель";

        public override string Description => "Слизень-истребитель:\n+3 Урона\nФракция: Возьмите карту";

        public override string ImagePath => "/Resources/Cards/BlobFighter.jpg";

        public override int Price => 1;

        public override Faction Faction => Faction.Green;

        public void OnAllied(Game g)
        {
            g.CurrentPlayer.DrawCards(1);
        }

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddDamage(3);
        }
    }
}