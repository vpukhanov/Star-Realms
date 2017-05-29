using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class BattleBlob : Card, IAllied, IScrappable
    {
        public override string Name => "Слизень-линкор";

        public override string Description => "Слизень-линкор:\n+8 Урона\nФракция: Возьмите карту\nУничтожение: +4 Урона";

        public override string ImagePath => "/Resources/Cards/BattleBlob.jpg";

        public override int Price => 6;

        public override Faction Faction => Faction.Green;

        public void OnAllied(Game g)
        {
            g.CurrentPlayer.DrawCards(1);
        }

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddDamage(8);
        }

        public void OnScrapped(Game g)
        {
            g.CurrentPlayer.AddDamage(4);
        }
    }
}