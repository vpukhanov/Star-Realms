using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Cards;
using System.Collections.ObjectModel;

namespace StarRealms.RulesEngine
{
    /// <summary>
    /// Класс, определяющий торговый ряд игры.
    /// Торговый ряд - колода различных карт, 4 их которых видимы игрокам.
    /// Любой из игроков в течение своего хода может приобрести любую из этих четырех карт,
    /// если он имеет достаточно торговли. Тогда эта карта сразу заменяется на новую из колоды
    /// </summary>
    public class TradeRow
    {
        public TradeRow()
        {
            SetupDecks();
        }

        /// <summary>
        /// Карты, видимые игрокам
        /// </summary>
        public ObservableCollection<Card> CurrentCards { get; private set; }

        /// <summary>
        /// Колода карт торгового ряда
        /// </summary>
        public ObservableCollection<Card> Deck { get; private set; }

        /// <summary>
        /// Приобрести карту для одного из игроков
        /// </summary>
        /// <param name="p">Игрок - покупатель</param>
        /// <param name="c">Приобретаемая карта</param>
        public void PurchaseCard(Player p, Card c)
        {
            if (p.PurchaseCard(c))
            {
                this.RemoveCard(c);
            }
        }

        /// <summary>
        /// Удалить карту из торгового ряда и заменить ее на новую
        /// </summary>
        /// <param name="c">Удаляемая карта</param>
        public void RemoveCard(Card c)
        {
            this.CurrentCards.Remove(c);

            while (this.CurrentCards.Count < 4)
                this.DrawCard();
        }

        /// <summary>
        /// Собрать начальную колоду торгового ряда
        /// </summary>
        private void AddStartingCards()
        {
            // карты по 3 копии
            for (int i = 0; i < 3; i++)
            {
                this.Deck.Add(new TradeBot());
                this.Deck.Add(new MissileBot());
                this.Deck.Add(new SupplyBot());
                this.Deck.Add(new ImperialFighter());
                this.Deck.Add(new ImperialFrigate());
                this.Deck.Add(new SurveyShip());
                this.Deck.Add(new FederationShuttle());
                this.Deck.Add(new Cutter());
                this.Deck.Add(new BlobFighter());
                this.Deck.Add(new TradePod());
            }

            // карты по 2 копии
            for (int i = 0; i < 2; i++)
            {
                this.Deck.Add(new PatrolMech());
                this.Deck.Add(new Corvette());
                this.Deck.Add(new EmbassyYacht());
                this.Deck.Add(new Freighter());
                this.Deck.Add(new BattlePod());
                this.Deck.Add(new Ram());
                this.Deck.Add(new BlobDestroyer());
            }

            // карты по 1 копии
            this.Deck.Add(new BattleMech());
            this.Deck.Add(new MissileMech());
            this.Deck.Add(new Battlecruiser());
            this.Deck.Add(new Dreadnaught());
            this.Deck.Add(new CommandShip());
            this.Deck.Add(new TradeEscort());
            this.Deck.Add(new Flagship());
            this.Deck.Add(new BattleBlob());
            this.Deck.Add(new BlobCarrier());
            this.Deck.Add(new Mothership());

            this.Deck.Shuffle();
        }

        /// <summary>
        /// Сделать одну из карт колоды торгового ряда доступной для игроков
        /// </summary>
        private void DrawCard()
        {
            if (this.Deck.Count == 0)
            {
                this.AddStartingCards();
            }

            this.CurrentCards.Add(this.Deck[0]);
            this.Deck.RemoveAt(0);
        }

        /// <summary>
        /// Создать стартовое положение торгового ряда
        /// </summary>
        private void SetupDecks()
        {
            this.CurrentCards = new ObservableCollection<Card>();
            this.Deck = new ObservableCollection<Card>();

            AddStartingCards();

            for (int i = 0; i < 4; i++)
            {
                this.DrawCard();
            }
        }
    }
}