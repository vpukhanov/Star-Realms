using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Cards;
using StarRealms.RulesEngine.Players;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace StarRealms.RulesEngine
{
    /// <summary>
    /// Класс, обозначающий отдельную игру.
    /// Игра имеет двух игроков, которые поочередно выполняют ходы, пока
    /// власть одного из них не опустится до нуля
    /// </summary>
    public class Game : INotifyPropertyChanged
    {
        public Game()
        {
            this.Bot = new BotPlayer(this);

            CurrentPlayer = Human;
            this.Human.Hand.CollectionChanged += OnPlayerHandStateChanged;

            this.Human.PropertyChanged += CheckGameOver;
            this.Bot.PropertyChanged += CheckGameOver;

            this.Human.CardPlayed += OnCardPlayed;
            this.Bot.CardPlayed += OnCardPlayed;
        }

        /// <summary>
        /// Событие, происходящее при завершении игры
        /// </summary>
        public event Action<Player> OnGameOver;

        /// <summary>
        /// Событие, происходящее при изменении свойств игры
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Игрок - бот
        /// </summary>
        public Player Bot { get; }

        /// <summary>
        /// Игрок, который в данный момент делает ход
        /// </summary>
        public Player CurrentPlayer { get; private set; }

        /// <summary>
        /// Игрок - человек
        /// </summary>
        public Player Human { get; } = new HumanPlayer();

        /// <summary>
        /// Показывает, может ли игрок-человек завершить ход
        /// </summary>
        public bool HumanCanEndTurn => IsHumanTurn && Human.Hand.Count == 0;

        /// <summary>
        /// Показывает, является ли текущий ход ходом человека
        /// </summary>
        public bool IsHumanTurn => CurrentPlayer == Human;

        /// <summary>
        /// Игрок - соперник. Тот игрок, которому не принадлежит текущий ход
        /// </summary>
        public Player OpposingPlayer
        {
            get
            {
                return CurrentPlayer == Human ? Bot : Human;
            }
        }

        /// <summary>
        /// Список разыгранных за всю игру карт
        /// </summary>
        public ObservableCollection<Card> PlayedCards { get; private set; } = new ObservableCollection<Card>();

        /// <summary>
        /// Торговый ряд текущей игры
        /// </summary>
        public TradeRow TradeRow { get; } = new TradeRow();

        /// <summary>
        /// Метод, загружающий сохраненное состояние игры
        /// </summary>
        /// <param name="info">Сохраненное состояние игры в формате XML</param>
        public void LoadGame(string info)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
            SaveGameData r;

            using (TextReader reader = new StringReader(info))
            {
                r = (SaveGameData)serializer.Deserialize(reader);
            }

            this.Human.LoadInfo(r.HumanDeck, r.HumanHand, r.HumanGraveyard, r.HumanAuthority, r.HumanTrade, r.HumanDamage, r.HumanNextPurchaseFree, r.HumanNextShipTop);
            this.Bot.LoadInfo(r.BotDeck, r.BotHand, r.BotGraveyard, r.BotAuthority, r.BotTrade, r.BotDamage, r.BotNextPurchaseFree, r.BotNextShipTop);

            this.PlayedCards.Clear();
            Utilities.CopyFromListToObservableCollection<Card>(this.PlayedCards, r.PlayedCards);
        }

        /// <summary>
        /// Завершить ход текущего игрока и передать ход его оппоненту
        /// </summary>
        public void NextTurn()
        {
            this.CurrentPlayer.EndTurn(this);

            this.CurrentPlayer = this.CurrentPlayer == this.Human ? this.Bot : this.Human;

            OnPropertyChanged("CurrentPlayer");
            OnPropertyChanged("IsHumanTurn");
            OnPropertyChanged("HumanCanEndTurn");

            this.CurrentPlayer.BeginTurn(this);
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Сохранить текущее состояние игры.
        /// </summary>
        /// <returns>Состояние игры в формате XML</returns>
        public string SaveGame()
        {
            SaveGameData data = new SaveGameData(this.Human as HumanPlayer, this.Bot as BotPlayer, this.TradeRow, this.PlayedCards);

            XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));

            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, data);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Проверить, завершена ли игра.
        /// Игра завершена только в тот момент, когда власть одного из игроков
        /// опускается до нуля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckGameOver(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Authority")
            {
                // если власть какого-то игрока снизилась до 0, сообщаем об этом
                if (this.Human.Authority <= 0)
                {
                    this.OnGameOver?.Invoke(this.Human);
                }
                else if (this.Bot.Authority <= 0)
                {
                    this.OnGameOver?.Invoke(this.Bot);
                }
            }
        }

        /// <summary>
        /// Обработать розыгрыш карты одним из игроков
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnCardPlayed(Player sender, CardPlayedEventArgs args)
        {
            this.PlayedCards.Insert(0, args.PlayedCard);
        }

        /// <summary>
        /// Обработать изменение количества карт в руке игрока
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPlayerHandStateChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("HumanCanEndTurn");
        }
    }

    /// <summary>
    /// Класс, предназначенный для сохранения состояния игры.
    /// Требует сбора информации из разных частей игры.
    /// </summary>
    [Serializable]
    public class SaveGameData
    {
        public int BotAuthority, BotTrade, BotDamage, BotDiscard;

        public bool BotNextPurchaseFree, BotNextShipTop;

        public int HumanAuthority, HumanTrade, HumanDamage, HumanDiscard;

        [XmlArrayItem("Scout", typeof(Scout))]
        [XmlArrayItem("Viper", typeof(Viper))]
        [XmlArrayItem("CommandShip", typeof(CommandShip))]
        [XmlArrayItem("Cutter", typeof(Cutter))]
        [XmlArrayItem("EmbassyYacht", typeof(EmbassyYacht))]
        [XmlArrayItem("FederationShuttle", typeof(FederationShuttle))]
        [XmlArrayItem("Flagship", typeof(Flagship))]
        [XmlArrayItem("Freighter", typeof(Freighter))]
        [XmlArrayItem("TradeEscort", typeof(TradeEscort))]
        [XmlArrayItem("BattleBlob", typeof(BattleBlob))]
        [XmlArrayItem("BattlePod", typeof(BattlePod))]
        [XmlArrayItem("BlobCarrier", typeof(BlobCarrier))]
        [XmlArrayItem("BlobDestroyer", typeof(BlobDestroyer))]
        [XmlArrayItem("BlobFighter", typeof(BlobFighter))]
        [XmlArrayItem("Mothership", typeof(Mothership))]
        [XmlArrayItem("Ram", typeof(Ram))]
        [XmlArrayItem("TradePod", typeof(TradePod))]
        [XmlArrayItem("BattleMech", typeof(BattleMech))]
        [XmlArrayItem("MissileBot", typeof(MissileBot))]
        [XmlArrayItem("MissileMech", typeof(MissileMech))]
        [XmlArrayItem("PatrolMech", typeof(PatrolMech))]
        [XmlArrayItem("SupplyBot", typeof(SupplyBot))]
        [XmlArrayItem("TradeBot", typeof(TradeBot))]
        [XmlArrayItem("Battlecruiser", typeof(Battlecruiser))]
        [XmlArrayItem("Corvette", typeof(Corvette))]
        [XmlArrayItem("Dreadnaught", typeof(Dreadnaught))]
        [XmlArrayItem("ImperialFighter", typeof(ImperialFighter))]
        [XmlArrayItem("ImperialFrigate", typeof(ImperialFrigate))]
        [XmlArrayItem("SurveyShip", typeof(SurveyShip))]
        [XmlArrayItem("Explorer", typeof(Explorer))]
        public List<Card> HumanHand, HumanDeck, HumanGraveyard, BotHand, BotDeck, BotGraveyard, PlayedCards;

        public bool HumanNextPurchaseFree, HumanNextShipTop;
        public List<Card> TradeRowDeck, TradeRowCurrent;

        public SaveGameData()
        {
        }

        public SaveGameData(HumanPlayer human, BotPlayer bot, TradeRow row, ObservableCollection<Card> history)
        {
            HumanHand = human.Hand.ToList();
            HumanDeck = human.Deck.ToList();
            HumanGraveyard = human.Graveyard.ToList();
            HumanAuthority = human.Authority;
            HumanTrade = human.AvailableTrade;
            HumanDamage = human.AvailableDamage;
            HumanDiscard = human.CardsToDiscard;
            HumanNextPurchaseFree = human.NextPurchaseForFree;
            HumanNextShipTop = human.NextPurchaseOnTop;

            BotHand = bot.Hand.ToList();
            BotDeck = bot.Deck.ToList();
            BotGraveyard = bot.Graveyard.ToList();
            BotAuthority = bot.Authority;
            BotTrade = bot.AvailableTrade;
            BotDamage = bot.AvailableDamage;
            BotDiscard = bot.CardsToDiscard;
            BotNextPurchaseFree = bot.NextPurchaseForFree;
            BotNextShipTop = bot.NextPurchaseOnTop;

            TradeRowDeck = row.Deck.ToList();
            TradeRowCurrent = row.CurrentCards.ToList();

            PlayedCards = history.ToList();
        }
    }
}