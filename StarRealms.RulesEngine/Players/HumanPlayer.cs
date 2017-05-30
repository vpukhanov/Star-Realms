using StarRealms.RulesEngine.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarRealms.RulesEngine.Players
{
    public class HumanPlayer : Player
    {
        public HumanPlayer()
        {
            // Игрок в начинает игру с тремя картами в руке
            this.DrawCards(3);
        }

        public override void BeginTurn(Game g)
        {
            if (this.CardsToDiscard > 0)
            {
                Utilities.userNotifyMethod(String.Format("Вам нужно сбросить {0} {1}.",
                    this.CardsToDiscard, Utilities.GetPlural(this.CardsToDiscard, "карту", "карты", "карт")));
            }

            base.BeginTurn(g);
        }

        public override void DiscardCardInHand(Game g)
        {
            Card cardToDiscard = Utilities.userCardChoiceMethod(this.Hand.ToList());
            this.DiscardCard(cardToDiscard);
        }

        public override int MakeCustomChoice(Game g, List<string> choiceList)
        {
            return Utilities.userStringChoiceMethod(choiceList);
        }

        public override void ScrapCardInDiscard(Game g)
        {
            Card cardToScrap = Utilities.userCardChoiceMethod(this.Graveyard.ToList());
            this.Graveyard.Remove(cardToScrap);
        }

        public override void ScrapCardInHand(Game g)
        {
            Card cardToScrap = Utilities.userCardChoiceMethod(this.Hand.ToList());
            this.Hand.Remove(cardToScrap);
        }

        public override void ScrapCardInTradeRow(Game g)
        {
            Card cardToScrap = Utilities.userCardChoiceMethod(g.TradeRow.CurrentCards.ToList());
            g.TradeRow.RemoveCard(cardToScrap);
        }

        public override bool ShouldScrap(Card c)
        {
            return Utilities.userYesNoChoiceMethod("Уничтожить эту карту?\n" + c.Description, "Уничтожение карты");
        }

        public override bool ShouldScrapCardInDiscard(Game g)
        {
            if (this.Graveyard.Count > 0)
            {
                return Utilities.userYesNoChoiceMethod("Хотите уничтожить карту в сбросе?", "Уничтожение карты в сбросе");
            }
            else
            {
                return false;
            }
        }

        public override bool ShouldScrapCardInHand(Game g)
        {
            if (this.Hand.Count > 0)
            {
                return Utilities.userYesNoChoiceMethod("Хотите уничтожить карту в руке?", "Уничтожение карты в руке");
            }
            else
            {
                return false;
            }
        }

        public override bool ShouldScrapCardInTradeRow(Game g)
        {
            return Utilities.userYesNoChoiceMethod("Хотите уничтожить карту в торговом ряду?", "Уничтожение карты в торговом ряду");
        }
    }
}