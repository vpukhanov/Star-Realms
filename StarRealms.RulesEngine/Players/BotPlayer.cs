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
            g.TradeRow.RemoveCard(cardToScrap);
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

        /// <summary>
        /// Приобрести лучшую доступную карту в торговом ряду
        /// </summary>
        /// <remarks>
        /// На данный момент лучшей считается самая дорогая доступная карта
        /// </remarks>
        /// <param name="g">Текущая игра</param>
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

        /// <summary>
        /// Определить, возможно ли приобрести одну из карт торгового ряда
        /// </summary>
        /// <param name="g">Текущая игра</param>
        /// <returns>Возможно ли приобрести одну из карт торгового ряда</returns>
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

        /// <summary>
        /// Временный метод, решить "да или нет"
        /// </summary>
        /// <remarks>
        /// Сейчас это производится случайным выбором
        /// </remarks>
        /// <returns>Решение - да (<c>true</c>) или нет (<c>false</c>)</returns>
        private bool DecideYesNo()
        {
            return this.random.Next(2) == 1;
        }

        /// <summary>
        /// Сделать один шаг в текущем ходу
        /// </summary>
        /// <remarks>
        /// На данный момент, поведение бота такого:
        /// <ol>
        ///     <li>Если есть карты в руке, разыграть одну</li>
        ///     <li>Если можем купить что-то в торговом ряду, покупаем</li>
        ///     <li>Если есть доступный урон, наносим его противнику</li>
        ///     <li>Передаем ход противнику</li>
        /// </ol>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="args"></param>
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

        /// <summary>
        /// Выбрать случайную карту из списка
        /// </summary>
        /// <param name="availableCards">Список карт</param>
        /// <returns>Выбранная карта</returns>
        private Card PickRandomCard(IList<Card> availableCards)
        {
            return availableCards[this.random.Next(availableCards.Count)];
        }
    }
}