using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Modifiers;

namespace StarRealms.RulesEngine.Cards
{
    public class BlobDestroyer : Card, IAllied
    {
        public override string Name => "Слизень-эсминец";

        public override string Description => "Слизень-эсминец:\n+6 Урона\nВы можете уничтожить выбранную базу и/или уничтожить карту в торговом ряду";

        public override string ImagePath => "/Resources/Cards/BlobDestroyer.jpg";

        public override int Price => 4;

        public override Faction Faction => Faction.Green;

        public void OnAllied(Game g)
        {
            // TODO: добавить возможность уничтожения базы (после добавления баз)
            if (g.CurrentPlayer.ShouldScrapCardInTradeRow(g))
            {
                g.CurrentPlayer.ScrapCardInTradeRow(g);
            }
        }

        public override void OnPlay(Game g)
        {
            g.CurrentPlayer.AddDamage(6);
        }
    }
}