using StarRealms.RulesEngine.Cards;
using StarRealms.RulesEngine.Modifiers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StarRealms.RulesEngine.Abstract
{
    /// <summary>
    /// Параметры события, происходящего при розыгрыше карты
    /// </summary>
    public class CardPlayedEventArgs
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="card">Разыгранная карта</param>
        public CardPlayedEventArgs(Card card)
        {
            this.PlayedCard = card;
        }

        /// <summary>
        /// Разыгранная карта
        /// </summary>
        public Card PlayedCard { get; private set; }
    }

    /// <summary>
    /// Абстрактный класс, определяющий игрока в игре.
    /// Игрок - сущность, имеющая руку карт, колоду карт и сброс, и
    /// умеющая делать ходы, подчиняясь правилам игры
    /// </summary>
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

        /// <summary>
        /// Событие, происходящее при разыгрывании карты данным игроком
        /// </summary>
        public event CardPlayedEventHandler CardPlayed;

        /// <summary>
        /// Событие, происходящее при изменении свойств игрока
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Количество власти (здоровья) игрока
        /// </summary>
        public int Authority { get; private set; } = 50;

        /// <summary>
        /// Урон, который в данный момент может нанести игрок
        /// </summary>
        public int AvailableDamage { get; private set; } = 0;

        /// <summary>
        /// Количество торговли (монет), которым в данный момент обладает игрок
        /// </summary>
        public int AvailableTrade { get; private set; } = 0;

        /// <summary>
        /// Количество карт, которое игрок должен будет сбросить в начале своего хода
        /// </summary>
        public int CardsToDiscard { get; set; } = 0;

        /// <summary>
        /// Колода игрока
        /// </summary>
        public ObservableCollection<Card> Deck { get; private set; }

        /// <summary>
        /// Сброс игрока
        /// </summary>
        public ObservableCollection<Card> Graveyard { get; private set; }

        /// <summary>
        /// Рука игрока
        /// </summary>
        public ObservableCollection<Card> Hand { get; private set; }

        /// <summary>
        /// Флаг, показывающий, что следующая покупка игрока на этом ходу
        /// будет бесплатной
        /// </summary>
        public bool NextPurchaseForFree { get; private set; } = false;

        /// <summary>
        /// Флаг, показывающий, что следующая покупка игрока на этом ходу
        /// будет добавлена на верх его колоды, а не в сброс
        /// </summary>
        public bool NextPurchaseOnTop { get; private set; } = false;

        /// <summary>
        /// Добавить власть игроку
        /// </summary>
        /// <param name="ammount">Количество добавляемой власти</param>
        public void AddAuthority(int ammount)
        {
            this.Authority += ammount;
            OnPropertyChanged("Authority");
        }

        /// <summary>
        /// Добавить карту в колоду игрока
        /// </summary>
        /// <param name="c">Добавляемая карта</param>
        public void AddCardToDeck(Card c)
        {
            this.Deck.Add(c);
        }

        /// <summary>
        /// Добавить карту в сброс игрока
        /// </summary>
        /// <param name="c">Добавляемая карта</param>
        public void AddCardToGraveyard(Card c)
        {
            this.Graveyard.Add(c);
        }

        /// <summary>
        /// Добавить карту в руку игрока
        /// </summary>
        /// <param name="c">Добавляемая карта</param>
        public void AddCardToHand(Card c)
        {
            this.Hand.Add(c);
        }

        /// <summary>
        /// Добавить доступный урон игроку
        /// </summary>
        /// <param name="damage">Количество урона</param>
        public void AddDamage(int damage)
        {
            this.AvailableDamage += damage;
            OnPropertyChanged("AvailableDamage");
        }

        /// <summary>
        /// Добавить доступную торговлю игроку
        /// </summary>
        /// <param name="trade">Количество торговли</param>
        public void AddTrade(int trade)
        {
            this.AvailableTrade += trade;
            OnPropertyChanged("AvailableTrade");
        }

        /// <summary>
        /// Начать ход игрока и обработать его.
        /// В начале хода игрок тянет 5 карт и сбрасывает необходимое количество
        /// </summary>
        /// <param name="g">Текущая игра</param>
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

        /// <summary>
        /// Сбросить карту из руки игрока в его сброс
        /// </summary>
        /// <param name="c">Сбрасываемая карта</param>
        public void DiscardCard(Card c)
        {
            this.Hand.Remove(c);
            this.Graveyard.Add(c);
        }

        /// <summary>
        /// Позволить игроку выбрать карту в руке и сбросить ее
        /// </summary>
        /// <param name="g">Текущая игра</param>
        public abstract void DiscardCardInHand(Game g);

        /// <summary>
        /// Взять из колоды игрока <b>num</b> карт. Если карт в колоде недостаточно,
        /// то в нее будет замешан сброс игрока
        /// </summary>
        /// <param name="num">Количество карт, которое необходимо взять</param>
        public void DrawCards(int num)
        {
            for (int i = 0; i < num; i++)
            {
                if (this.Deck.Count == 0)
                {
                    if (this.Graveyard.Count == 0)
                    {
                        // если карты закончились и в сбросе, то не тянем больше карт
                        break;
                    }

                    // Если карт в колоде не осталось, замешиваем сброс обратно в колоду
                    for (int j = 0; j < this.Graveyard.Count; j++)
                    {
                        this.Deck.Add(this.Graveyard[j]);
                    }
                    this.Deck.Shuffle();
                    this.Graveyard.Clear();
                }

                this.Hand.Add(this.Deck[0]);
                this.Deck.RemoveAt(0);
            }
        }

        /// <summary>
        /// Завершить ход игрока и обработать его.
        /// В конце хода игрока, обнуляются его урон и торговля, сбрасываются
        /// некоторые бонусы
        /// </summary>
        /// <param name="g">Текущая игра</param>
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

        /// <summary>
        /// Загрузить состояние игрока, например при загрузке игры
        /// </summary>
        /// <param name="deck">Колода игрока</param>
        /// <param name="hand">Рука игрока</param>
        /// <param name="graveyard">Сброс игрока</param>
        /// <param name="authority">Количество власти игрока</param>
        /// <param name="trade">Количество торговли игрока</param>
        /// <param name="damage">Количество урона игрока</param>
        /// <param name="nextFree">Флаг "Следующая покупка бесплатна"</param>
        /// <param name="nextTop">Флаг "Следующая покупка на верх колоды"</param>
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

        /// <summary>
        /// Позволить игроку сделать выбор одного из вариантов
        /// </summary>
        /// <param name="g">Текущая игра</param>
        /// <param name="choiceList">Список возможных выборов</param>
        /// <returns>Индекс сделанного выбора в списке <b>choiceList</b></returns>
        public abstract int MakeCustomChoice(Game g, List<string> choiceList);

        /// <summary>
        /// Сделать следующую покупку игрока на этом ходу бесплатной
        /// </summary>
        public void MakeNextPurchaseFree()
        {
            this.NextPurchaseForFree = true;
            OnPropertyChanged("NextPurchaseForFree");
        }

        /// <summary>
        /// Сделать так, чтобы следующая покупка игрока на этом ходу
        /// отправилась на верх его колоды, а не в сброс
        /// </summary>
        public void MakeNextShipOnTop()
        {
            this.NextPurchaseOnTop = true;
            OnPropertyChanged("NextPurchaseOnTop");
        }

        /// <summary>
        /// Разыграть карту и обработать это событие.
        /// При розыгрыше карты она отправляется в сброс, срабатывает ее
        /// эффект, может сработать ее эффект фракции, может быть предложено
        /// уничтожить ее для получения дополнительного эффекта.
        /// </summary>
        /// <param name="g">Текущая игра</param>
        /// <param name="c">Разыгрываемая карта</param>
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

        /// <summary>
        /// Приобрести карту.
        /// При приобретении с игрока списывается соответственное количество
        /// торговли, затем она отправляется в сброс (или на верх колоды)
        /// </summary>
        /// <param name="c">Приобретаемая карта</param>
        /// <returns>Была ли покупка успешной</returns>
        public bool PurchaseCard(Card c)
        {
            if (this.AvailableTrade < c.Price && !this.NextPurchaseForFree)
                return false;

            if (this.NextPurchaseOnTop)
            {
                // кладем карту на верх колоды
                this.Deck.Insert(0, c);
                this.NextPurchaseOnTop = false;
                OnPropertyChanged("NextPurchaseOnTop");
            }
            else
            {
                // кладем карту в наш сброс
                this.Graveyard.Add(c);
            }

            if (this.NextPurchaseForFree)
            {
                this.NextPurchaseForFree = false;
                OnPropertyChanged("NextPurchaseForFree");
            }
            else
            {
                this.RemoveTrade(c.Price);
            }

            return true;
        }

        /// <summary>
        /// Уменьшить власть игрока
        /// </summary>
        /// <param name="ammount">Количество власти</param>
        public void RemoveAuthority(int ammount)
        {
            this.Authority -= ammount;
            OnPropertyChanged("Authority");
        }

        /// <summary>
        /// Уменьшить доступный урон игрока
        /// </summary>
        /// <param name="damage">Количество урона</param>
        public void RemoveDamage(int damage)
        {
            this.AvailableDamage -= damage;
            OnPropertyChanged("AvailableDamage");
        }

        /// <summary>
        /// Уменьшить доступную торговлю игрока
        /// </summary>
        /// <param name="trade">Количество торговли</param>
        public void RemoveTrade(int trade)
        {
            this.AvailableTrade -= trade;
            OnPropertyChanged("AvailableTrade");
        }

        /// <summary>
        /// Уничтожить карту.
        /// При уничтожении карты она убирается из игры, активируется
        /// ее эффект "при уничтожении"
        /// </summary>
        /// <param name="g">Текущая игра</param>
        /// <param name="c">Уничтожаемая карта</param>
        public void ScrapCard(Game g, Card c)
        {
            IScrappable sCard = c as IScrappable;
            sCard.OnScrapped(g);
            this.Graveyard.Remove(c); // полностью уничтожаем карту
        }

        /// <summary>
        /// Игрок выбирает карту в сбросе и уничтожает ее
        /// </summary>
        /// <param name="g">Текущая игра</param>
        public abstract void ScrapCardInDiscard(Game g);

        /// <summary>
        /// Игрок выбирает карту в руке и уничтожает ее
        /// </summary>
        /// <param name="g">Текущая игра</param>
        public abstract void ScrapCardInHand(Game g);

        /// <summary>
        /// Игрок выбирает карту торгового ряда и уничтожает ее
        /// </summary>
        /// <param name="g">Текущая игра</param>
        public abstract void ScrapCardInTradeRow(Game g);

        /// <summary>
        /// Игрок определяет, хочет ли он уничтожить переданную карту
        /// </summary>
        /// <param name="c">Передаваемая карта</param>
        /// <returns>Результат определения - хочет ли игрок уничтожить карту</returns>
        public abstract bool ShouldScrap(Card c);

        /// <summary>
        /// Игрок определяет, хочет ли он уничтожить карту в своем сбросе
        /// </summary>
        /// <param name="g">Текущая игра</param>
        /// <returns>Результат определения - хочет ли игрок уничтожить карту в своем сбросе</returns>
        public abstract bool ShouldScrapCardInDiscard(Game g);

        /// <summary>
        /// Игрок определяет, хочет ли он уничтожить карту в своей руке
        /// </summary>
        /// <param name="g">Текущая игра</param>
        /// <returns>Результат определения - хочет ли игрок уничтожить карту в своей руке</returns>
        public abstract bool ShouldScrapCardInHand(Game g);

        /// <summary>
        /// Игрок определяет, хочет ли он уничтожить карту в торговом ряду
        /// </summary>
        /// <param name="g">Текущая игра</param>
        /// <returns>Результат определения - хочет ли игрок уничтожить карту в торговом ряду</returns>
        public abstract bool ShouldScrapCardInTradeRow(Game g);

        /// <summary>
        /// Добавить стартовые карты в колоду игрока
        /// </summary>
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

        /// <summary>
        /// Получить список карт данной фракции, разыгранных на этом ходу
        /// </summary>
        /// <param name="f">Выбранная фракция</param>
        /// <returns>Список карт данной фракции, разыгранных на этом ходу</returns>
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

        /// <summary>
        /// Обработать разыгранную карту, обладающую фракционной способностью
        /// </summary>
        /// <param name="g">Текущая игра</param>
        /// <param name="c">Разыгранная карта с фракционной способностью</param>
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

        /// <summary>
        /// Обработка разыгранной карты, принадлежащей какой-то фракции
        /// </summary>
        /// <param name="c">Разыгранная карта</param>
        private void OnPlayedFaction(Card c)
        {
            List<Card> factionCards = this.GetFactionList(c.Faction);
            factionCards.Add(c);
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Создать стартовое положение игрока
        /// </summary>
        private void SetupDeck()
        {
            this.Hand = new ObservableCollection<Card>();
            this.Deck = new ObservableCollection<Card>();
            this.Graveyard = new ObservableCollection<Card>();

            AddStartingCards();
        }
    }
}