using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CoreBusinessLogic
{
    public class FigureMatcher : IFigureMatcher
    {
        private List<ICard> flop;
        private List<ICard> hand;
        private Dictionary<string, PokerFigure> matchedFigures;

        public FigureMatcher()
        {
            hand = new List<ICard>();
            flop = new List<ICard>();
            matchedFigures = new Dictionary<string, PokerFigure>();
        }

        public Dictionary<string, PokerFigure> CheckHand()
        {
            CheckPair();
            CheckThreeOfKind();
            CheckFourOfKind();
            CheckStraight();
            CheckFlush();
            CheckFull();

            return matchedFigures;
        }

        public void AddCardToFlop(CardFigure figure, CardColor color)
        {
            flop.Add(new Card(figure, color));
        }

        public void AddCardToHand(CardFigure figure, CardColor color)
        {
            hand.Add(new Card(figure, color));
        }

        private void CheckPair()
        {
            var tempHand = hand.Concat(flop).ToList();
            if (!CheckGroupCount(tempHand, 2)) return;
            var pair = GetGroup(tempHand, 2);
            var name = "Pair";
            var number = 1;
            while (pair.Any())
            {
                matchedFigures.Add($"{name}{number}", new PokerFigure("Pair", pair, 100));
                tempHand = tempHand.Except(pair).ToList();
                number++;
                pair = GetGroup(tempHand, 2);
            }
            
            //if(pair.Any())
            //{
            //    matchedFigures.Add("Pair", new PokerFigure("Pair", pair, 100));
            //    tempHand = tempHand.Except(pair).ToList();
            //}
            //pair = GetGroup(tempHand, 2);
            //if(pair.Any())
            //{
            //    matchedFigures.Add("SecondPair", new PokerFigure("Pair", pair, 100));
            //}
        }

        private void CheckThreeOfKind()
        {
            var tempHand = hand.Concat(flop).ToList();

            if (!CheckGroupCount(tempHand, 3)) return;

            var threeOf = GetGroup(tempHand, 3);
            var cardList = new List<ICard>();
            foreach (var card in threeOf)
            {
                cardList.Add(tempHand.First(c => c.ID == card.ID));
            }
            matchedFigures.Add("ThreeOfKind", new PokerFigure("ThreeOfKind", cardList, 100));
        }

        private void CheckFourOfKind()
        {
            var tempHand = hand.Concat(flop).ToList();

            if (!CheckGroupCount(tempHand, 4)) return;

            var fourOf = GetGroup(tempHand, 4);
            var cardList = new List<ICard>();
            foreach (var card in fourOf)
            {
                cardList.Add(tempHand.First(c => c.ID == card.ID));
            }
            matchedFigures.Add("FourOfKind", new PokerFigure("FourOfKind", cardList, 100));
        }

        private CardFigure GetCardFigure(ICard card)
        {
            return card.Figure;
        }

        private void CheckStraight()
        {
            var tempHand = hand.Concat(flop).ToList();
            var cardList = new List<ICard>();
            tempHand = tempHand.OrderBy(c => c.Figure).ToList();

            if (cardList.Count < 5) return;
            if (NotInOrder(tempHand)) return;

            matchedFigures.Add("Straight", new PokerFigure("Straight", cardList, 100));
        }

        private void CheckFlush()
        {
            var tempHand = hand.Concat(flop).ToList();
            var cardList = new List<ICard>();

            if (!tempHand.GroupBy(x => x.Color)
                        .Where(group => group.Count() >= 5)
                        .ToList()
                        .Any()) return;
            var key = tempHand.GroupBy(x => x.Color)
                        .Where(group => group.Count() >= 5)
                        .Select(group => group.Key)
                        .First();
            cardList = tempHand.Where(x => x.Color == key).ToList();
            matchedFigures.Add("Flush", new PokerFigure("Flush", cardList, 100));            

            if (NotInOrder(cardList)) return;

            if (cardList.Any(x => x.Figure == CardFigure._As))
            {
                matchedFigures.Add("Royal Flush", new PokerFigure("Royal Flush", cardList, 100));
            }
            else
            {
                matchedFigures.Add("Straight Flush", new PokerFigure("Straight Flush", cardList, 100));
            }
        }

        private void CheckFull()
        {
            var tempHand = hand.Concat(flop).ToList();

            if (!CheckGroupCount(tempHand, 2)) return;
            if (!CheckGroupCount(tempHand, 3)) return;

            var pair = GetGroup(tempHand, 2);
            var threeOfKind = GetGroup(tempHand, 3);

            if (!pair.Any() || !threeOfKind.Any()) return;

            var cardList = new List<ICard>();
            var full = pair.Concat(threeOfKind).ToList();
            foreach (var card in full)
            {
                cardList.Add(tempHand.First(c => c.ID == card.ID));
            }
            matchedFigures.Add("Full", new PokerFigure("Full", cardList, 100));
        }

        private bool NotInOrder(List<ICard> tempHand)
        {
            tempHand = tempHand.OrderBy(x => x.Figure).ToList();
            for (int q = 0; q < tempHand.Count - 1; q++)
            {
                var firstCard = tempHand[q].Figure;
                var nextCard = tempHand[q + 1].Figure;

                if (firstCard != nextCard - 1) return true;
            }

            return false;
        }

        private bool CheckGroupCount(List<ICard> cards, int elements)
        {
            return cards.GroupBy(x => x.Figure)
                        .Where(group => group.Count() == elements)
                        .Select(group => group)
                        .Any();
        }

        private List<ICard> GetGroup(List<ICard> cards, int groupCount)
        {
            var groups = cards
                .GroupBy(u => u.Figure)
                .Select(grp => grp.ToList())
                .ToList();
            if (groups.Any(x => x.Count == groupCount)) return groups.First(x => x.Count == groupCount);
            return new List<ICard>();
        }
    }
}
