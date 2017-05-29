using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Cards;
using System.Collections.ObjectModel;

namespace StarRealms.RulesEngine
{
    public class TradeRow
    {
        public TradeRow()
        {
            SetupDecks();
        }

        public ObservableCollection<Card> CurrentCards { get; private set; }
        public ObservableCollection<Card> Deck { get; private set; }
        public void PurchaseCard(Player p, Card c)
        {
            if (p.PurchaseCard(c))
            {
                this.ScrapCard(c);
            }
        }

        public void ScrapCard(Card c)
        {
            this.CurrentCards.Remove(c);

            while (this.CurrentCards.Count < 4)
                this.DrawCard();
        }

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

        private void DrawCard()
        {
            if (this.Deck.Count == 0)
            {
                this.AddStartingCards();
            }

            this.CurrentCards.Add(this.Deck[0]);
            this.Deck.RemoveAt(0);
        }

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