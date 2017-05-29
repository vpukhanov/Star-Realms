using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class BlobCarrier : Card, IAllied
    {
        public override string Name => "Слизень-авианосец";

        public override string Description => "Слизень-авианосец:\n+7 Урона\nФракция: Приобретите следующий корабль бесплатно и поместите его на верх вашей колоды";

        public override string ImagePath => "/Resources/Cards/BlobCarrier.jpg";

        public override int Price => 6;

        public override Faction Faction => Faction.Green;

        public void OnAllied(Game g)
        {
            g.CurrentPlayer.MakeNextPurchaseFree();
            g.CurrentPlayer.MakeNextShipOnTop();
        }

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddDamage(7);
        }
    }
}