using StarRealms.RulesEngine.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace StarRealms.RulesEngine.Players
{
    public class BotPlayer : Player
    {
        private Game currentGame = null;

        private Random random = new Random();
        private DispatcherTimer timer = new DispatcherTimer();

        public BotPlayer(Game g)
        {
            this.currentGame = g;

            this.timer.Interval = new TimeSpan(0, 0, 1);
            this.timer.Tick += MakeDescision;

            // бот потянет карты в начале своего хода
        }

        public override void BeginTurn(Game g)
        {
            base.BeginTurn(g);

            // Начинаем выполнять действия
            this.timer.Start();
        }

        public override void DiscardCardInHand(Game g)
        {
            // TODO: может стоит сделать по другому
            int cardToDiscard = this.random.Next(this.Hand.Count);
            this.DiscardCard(this.Hand[cardToDiscard]);
        }

        public override int MakeCustomChoice(Game g, List<string> choiceList)
        {
            // TODO: может стоит сделать по другому
            return this.random.Next(choiceList.Count);
        }

        public override void ScrapCardInDiscard(Game g)
        {
            Card cardToScrap = this.PickRandomCard(this.Graveyard);
            this.Graveyard.Remove(cardToScrap);
        }

        public override void ScrapCardInHand(Game g)
        {
            Card cardToScrap = this.PickRandomCard(this.Hand);
            this.Hand.Remove(cardToScrap);
        }

        public override void ScrapCardInTradeRow(Game g)
        {
            Card cardToScrap = this.PickRandomCard(g.TradeRow.CurrentCards);
            g.TradeRow.ScrapCard(cardToScrap);
        }

        public override bool ShouldScrap(Card c)
        {
            return this.DecideYesNo();
        }

        public override bool ShouldScrapCardInDiscard(Game g)
        {
            return this.Graveyard.Count > 0 && this.DecideYesNo();
        }

        public override bool ShouldScrapCardInHand(Game g)
        {
            return this.Hand.Count > 0 && this.DecideYesNo();
        }

        public override bool ShouldScrapCardInTradeRow(Game g)
        {
            return this.DecideYesNo();
        }

        private void BuyBestCard(Game g)
        {
            // на данный момент считаем, что чем дороже, тем лучше
            Card bestCard =
                g.TradeRow.CurrentCards.Where(c => this.AvailableTrade >= c.Price || this.NextPurchaseForFree)
                                       .OrderByDescending(c => c.Price).FirstOrDefault();

            if (bestCard != null)
            {
                g.TradeRow.PurchaseCard(this, bestCard);
            }
        }

        private bool CanBuySomething(Game g)
        {
            // если мы можем сделать бесплатную покупку, то мы отвечаем да
            if (this.NextPurchaseForFree)
            {
                return true;
            }

            // ищем доступные карты
            var availableCards =
                from c in g.TradeRow.CurrentCards
                where this.AvailableTrade >= c.Price
                select c;

            // отвечаем, нашлись ли такие карты
            return availableCards.Count() > 0;
        }

        private bool DecideYesNo()
        {
            return this.random.Next(2) == 1;
        }

        private void MakeDescision(object sender, EventArgs args)
        {
            // если есть карты в руке, разыгрываем одну
            if (this.Hand.Count > 0)
            {
                this.PlayCard(this.currentGame, this.Hand[0]);
                return;
            }

            // смотрим, можем ли что-то купить
            if (this.CanBuySomething(this.currentGame))
            {
                // покупаем лучшую карту
                this.BuyBestCard(this.currentGame);
                return;
            }

            // если все сыграли и не можем ничего купить, наносим урон оппоненту
            if (this.AvailableDamage > 0)
            {
                this.currentGame.Human.RemoveAuthority(this.AvailableDamage);
                this.RemoveDamage(this.AvailableDamage);
                return;
            }

            // если сделали все возможное, останавливаем таймер и передаем ход сопернику
            this.timer.Stop();
            this.currentGame.NextTurn();
        }
        private Card PickRandomCard(IList<Card> availableCards)
        {
            return availableCards[this.random.Next(availableCards.Count)];
        }
    }
}