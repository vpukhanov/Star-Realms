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
    public class Game : INotifyPropertyChanged
    {
        public Game()
        {
            this.Bot = new BotPlayer(this);

            CurrentPlayer = Human;
            this.Human.Hand.CollectionChanged += UpdatePlayerHandState;

            this.Human.PropertyChanged += CheckGameOver;
            this.Bot.PropertyChanged += CheckGameOver;

            this.Human.CardPlayed += OnCardPlayed;
            this.Bot.CardPlayed += OnCardPlayed;
        }

        public event Action<Player> OnGameOver;

        public event PropertyChangedEventHandler PropertyChanged;

        public Player Bot { get; }
        public Player CurrentPlayer { get; private set; }
        public Player Human { get; } = new HumanPlayer();
        public bool IsPlayerTurn => CurrentPlayer == Human;

        public Player OpposingPlayer
        {
            get
            {
                return CurrentPlayer == Human ? Bot : Human;
            }
        }

        public ObservableCollection<Card> PlayedCards { get; private set; } = new ObservableCollection<Card>();
        public bool PlayerCanEndTurn => IsPlayerTurn && Human.Hand.Count == 0;
        public TradeRow TradeRow { get; } = new TradeRow();
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

        public void NextTurn()
        {
            this.CurrentPlayer.EndTurn(this);

            this.CurrentPlayer = this.CurrentPlayer == this.Human ? this.Bot : this.Human;

            OnPropertyChanged("CurrentPlayer");
            OnPropertyChanged("IsPlayerTurn");
            OnPropertyChanged("PlayerCanEndTurn");

            this.CurrentPlayer.BeginTurn(this);
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

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

        private void CheckGameOver(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Authority")
            {
                // если власть какого-то игрока снизилась до 0, сообщаем об этом
                // и делаем невозможным продолжение игры
                if (this.Human.Authority <= 0)
                {
                    this.OnGameOver?.Invoke(this.Human);
                    this.CurrentPlayer = this.Bot;
                }
                else if (this.Bot.Authority <= 0)
                {
                    this.OnGameOver?.Invoke(this.Bot);
                    this.CurrentPlayer = this.Bot;
                }
            }
        }

        private void OnCardPlayed(Player sender, CardPlayedEventArgs args)
        {
            this.PlayedCards.Insert(0, args.PlayedCard);
        }
        private void UpdatePlayerHandState(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("PlayerCanEndTurn");
        }
    }

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