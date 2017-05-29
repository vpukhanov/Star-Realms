using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class MissileBot : Card, IAllied
    {
        public override string Name => "Ракетный Бот";

        public override string Description => "Ракетный Бот:\n+2 Урона\nВы можете уничтожить карту в руке или в сбросе\nФракция: +2 Урона";

        public override string ImagePath => "/Resources/Cards/MissileBot.jpg";

        public override int Price => 2;

        public override Faction Faction => Faction.Red;

        public void OnAllied(Game g)
        {
            g.CurrentPlayer.AddDamage(2);
        }

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddDamage(2);

            if (g.CurrentPlayer.ShouldScrapCardInHand(g))
            {
                g.CurrentPlayer.ScrapCardInHand(g);
            }
            else if (g.CurrentPlayer.ShouldScrapCardInDiscard(g))
            {
                g.CurrentPlayer.ScrapCardInDiscard(g);
            }
        }
    }
}