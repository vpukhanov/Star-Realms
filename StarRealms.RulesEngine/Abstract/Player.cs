using StarRealms.RulesEngine.Cards;
using StarRealms.RulesEngine.Modifiers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StarRealms.RulesEngine.Abstract
{
    public class CardPlayedEventArgs
    {
        public CardPlayedEventArgs(Card card)
        {
            this.PlayedCard = card;
        }

        public Card PlayedCard { get; private set; }
    }

    public abstract class Player : INotifyPropertyChanged
    {
        private List<Card> playedBlueCards = new List<Card>();
        private List<Card> playedGreenCards = new List<Card>();
        private List<Card> playedRedCards = new List<Card>();
        private List<Card> playedYellowCards = new List<Card>();
        public Player()
        {
            SetupDeck();
        }

        public delegate void CardPlayedEventHandler(Player sender, CardPlayedEventArgs args);

        public event CardPlayedEventHandler CardPlayed;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Authority { get; private set; } = 50;
        public int AvailableDamage { get; private set; } = 0;
        public int AvailableTrade { get; private set; } = 0;
        public int CardsToDiscard { get; set; } = 0;
        public ObservableCollection<Card> Deck { get; private set; }
        public ObservableCollection<Card> Graveyard { get; private set; }
        public ObservableCollection<Card> Hand { get; private set; }
        public bool NextPurchaseForFree { get; private set; } = false;
        public bool NextPurchaseOnTop { get; private set; } = false;
        public void AddAuthority(int ammount)
        {
            this.Authority += ammount;
            OnPropertyChanged("Authority");
        }

        public void AddCardToDeck(Card c)
        {
            this.Deck.Add(c);
        }

        public void AddCardToGraveyard(Card c)
        {
            this.Graveyard.Add(c);
        }

        public void AddCardToHand(Card c)
        {
            this.Hand.Add(c);
        }

        public void AddDamage(int damage)
        {
            this.AvailableDamage += damage;
            OnPropertyChanged("AvailableDamage");
        }

        public void AddTrade(int trade)
        {
            this.AvailableTrade += trade;
            OnPropertyChanged("AvailableTrade");
        }

        public virtual void BeginTurn(Game g)
        {
            // тянем 5 карт
            this.DrawCards(5);

            if (this.CardsToDiscard > 0)
            {
                // сбрасываем столько карт, сколько нужно
                while (this.CardsToDiscard > 0 && this.Hand.Count > 0)
                {
                    this.DiscardCardInHand(g);
                    this.CardsToDiscard--;
                }
            }
        }

        public void DiscardCard(Card c)
        {
            this.Hand.Remove(c);
            this.Graveyard.Add(c);
        }

        public abstract void DiscardCardInHand(Game g);

        public void DrawCards(int num)
        {
            for (int i = 0; i < num; i++)
            {
                if (this.Deck.Count <= 0)
                {
                    // Если карт в колоде не осталось, замешиваем сброс обратно в колоду
                    for (int j = 0; j < this.Graveyard.Count; j++)
                    {
                        this.Deck.Add(this.Graveyard[j]);
                    }
                    this.Deck.Shuffle();
                    this.Graveyard.Clear();
                }

                // если карты совсем закончились, не тянем больше
                if (this.Deck.Count == 0)
                {
                    break;
                }

                this.Hand.Add(this.Deck[0]);
                this.Deck.RemoveAt(0);
            }
        }

        public virtual void EndTurn(Game g)
        {
            this.RemoveTrade(this.AvailableTrade);
            this.RemoveDamage(this.AvailableDamage);

            this.CardsToDiscard = 0;

            this.NextPurchaseForFree = false;
            this.NextPurchaseOnTop = false;

            OnPropertyChanged("NextPurchaseForFree");
            OnPropertyChanged("NextPurchaseOnTop");

            playedBlueCards.Clear();
            playedGreenCards.Clear();
            playedRedCards.Clear();
            playedYellowCards.Clear();
        }

        public void LoadInfo(List<Card> deck, List<Card> hand, List<Card> graveyard, int authority, int trade, int damage, bool nextFree, bool nextTop)
        {
            this.Deck.Clear();
            Utilities.CopyFromListToObservableCollection<Card>(this.Deck, deck);

            this.Hand.Clear();
            Utilities.CopyFromListToObservableCollection<Card>(this.Hand, hand);

            this.Graveyard.Clear();
            Utilities.CopyFromListToObservableCollection<Card>(this.Graveyard, graveyard);

            this.AddAuthority(authority);
            this.RemoveAuthority(this.Authority - authority);

            this.RemoveTrade(this.AvailableTrade);
            this.AddTrade(trade);

            this.RemoveDamage(this.AvailableDamage);
            this.AddDamage(damage);

            this.NextPurchaseOnTop = nextTop;
            this.NextPurchaseForFree = nextFree;

            OnPropertyChanged("NextPurchaseOnTop");
            OnPropertyChanged("NextPurchaseForFree");
        }

        public abstract int MakeCustomChoice(Game g, List<string> choiceList);

        public void MakeNextPurchaseFree()
        {
            this.NextPurchaseForFree = true;
            OnPropertyChanged("NextPurchaseForFree");
        }

        public void MakeNextShipOnTop()
        {
            this.NextPurchaseOnTop = true;
            OnPropertyChanged("NextPurchaseOnTop");
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void PlayCard(Game g, Card c)
        {
            this.CardPlayed?.Invoke(this, new CardPlayedEventArgs(c));

            this.DiscardCard(c);
            c.OnPlay(g);

            if (c.Faction != Faction.None)
            {
                OnPlayedFaction(c);
            }

            if (c is IAllied)
            {
                OnPlayedAllied(g, c);
            }

            if (c is IScrappable && this.ShouldScrap(c))
                this.ScrapCard(g, c);
        }

        public bool PurchaseCard(Card c)
        {
            if (this.AvailableTrade < c.Price && !this.NextPurchaseForFree)
                return false;

            if (this.NextPurchaseOnTop)
            {
                // кладем карту на верх колоды
                this.Deck.Insert(0, c);
                this.NextPurchaseOnTop = false;
            }
            else
            {
                // кладем карту в наш сброс
                this.Graveyard.Add(c);
            }

            if (this.NextPurchaseForFree)
            {
                this.NextPurchaseForFree = false;
            }
            else
            {
                this.RemoveTrade(c.Price);
            }

            return true;
        }

        public void RemoveAuthority(int ammount)
        {
            this.Authority -= ammount;
            OnPropertyChanged("Authority");
        }

        public void RemoveDamage(int damage)
        {
            this.AvailableDamage -= damage;
            OnPropertyChanged("AvailableDamage");
        }

        public void RemoveTrade(int trade)
        {
            this.AvailableTrade -= trade;
            OnPropertyChanged("AvailableTrade");
        }

        public void ScrapCard(Game g, Card c)
        {
            IScrappable sCard = c as IScrappable;
            sCard.OnScrapped(g);
            this.Graveyard.Remove(c); // полностью уничтожаем карту
        }

        public abstract void ScrapCardInDiscard(Game g);

        public abstract void ScrapCardInHand(Game g);

        public abstract void ScrapCardInTradeRow(Game g);

        public abstract bool ShouldScrap(Card c);

        public abstract bool ShouldScrapCardInDiscard(Game g);

        public abstract bool ShouldScrapCardInHand(Game g);

        public abstract bool ShouldScrapCardInTradeRow(Game g);

        private void AddStartingCards()
        {
            // Каждый игрок начинает игру с 8 Скаутами в колоде
            for (int i = 0; i < 8; i++)
            {
                this.Deck.Add(new Scout());
            }

            // ... и с 2 Вайперами в колоде
            for (int i = 0; i < 2; i++)
            {
                this.Deck.Add(new Viper());
            }

            // перемешиваем колоду
            this.Deck.Shuffle();
        }

        private List<Card> GetFactionList(Faction f)
        {
            switch (f)
            {
                case Faction.Blue:
                    return playedBlueCards;

                case Faction.Green:
                    return playedGreenCards;

                case Faction.Red:
                    return playedRedCards;

                case Faction.Yellow:
                    return playedYellowCards;
            }
            return null;
        }

        private void OnPlayedAllied(Game g, Card c)
        {
            List<Card> factionList = this.GetFactionList(c.Faction);

            // активируем способность этой фракции для сыгранной карты если это
            // не первая сыгранная карта этой фракции
            if (factionList.Count >= 2)
            {
                IAllied aCard = c as IAllied;
                aCard.OnAllied(g);

                // если это ровно вторая сыгранная карта этой фракции, то активируем
                // также фракционную способность первой такой сыгранной карты (если она есть)
                if (factionList.Count == 2)
                {
                    IAllied firstCard = factionList[0] as IAllied;
                    if (firstCard != null)
                    {
                        firstCard.OnAllied(g);
                    }
                }
            }
        }

        private void OnPlayedFaction(Card c)
        {
            List<Card> factionCards = this.GetFactionList(c.Faction);
            factionCards.Add(c);
        }

        private void SetupDeck()
        {
            this.Hand = new ObservableCollection<Card>();
            this.Deck = new ObservableCollection<Card>();
            this.Graveyard = new ObservableCollection<Card>();

            AddStartingCards();
        }
    }
}