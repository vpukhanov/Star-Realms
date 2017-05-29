using StarRealms.RulesEngine;
using StarRealms.RulesEngine.Cards;

namespace StarRealms.Mocks
{
    public class GameMockData : Game
    {
        public GameMockData()
        {
            // карты бота
            this.Bot.AddCardToHand(new TestCard());
            this.Bot.AddCardToHand(new TestCard());
            this.Bot.AddCardToHand(new TestCard());

            this.Bot.AddCardToDeck(new TestCard());
            this.Bot.AddCardToDeck(new TestCard());

            this.Bot.AddCardToGraveyard(new TestCard());

            // карты человека
            this.Human.AddCardToHand(new TestCard());
            this.Human.AddCardToHand(new TestCard());
            this.Human.AddCardToHand(new TestCard());

            this.Human.AddCardToDeck(new TestCard());
            this.Human.AddCardToDeck(new TestCard());

            this.Human.AddCardToGraveyard(new TestCard());

            // торговый ряд
            this.TradeRow.CurrentCards.Add(new TestCard());
            this.TradeRow.CurrentCards.Add(new TestCard());
            this.TradeRow.CurrentCards.Add(new TestCard());
            this.TradeRow.CurrentCards.Add(new TestCard());

            this.TradeRow.Deck.Add(new TestCard());

            this.Human.MakeNextPurchaseFree();
            this.Human.MakeNextShipOnTop();
        }
    }
}