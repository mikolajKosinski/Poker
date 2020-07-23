using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CoreBusinessLogic
{
    public class FigureMatcher : IFigureMatcher
    {
        private List<Card> flop;
        private List<Card> hand;
        private Dictionary<string, PokerFigure> matchedFigures;
        private List<string> order;

        public FigureMatcher()
        {
            hand = new List<Card>();
            flop = new List<Card>();
            matchedFigures = new Dictionary<string, PokerFigure>();
            order = new List<string> {"2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        }

        public Dictionary<string, PokerFigure> CheckHand()
        {
            CheckPair();
            CheckThreeOfKind();
            CheckFourOfKind();
            CheckStraight();


            return matchedFigures;
        }

        public void AddCardToFlop(string name)
        {
            flop.Add(new Card(name));
        }

        public void AddCardToHand(string name)
        {
            hand.Add(new Card(name));
        }

        private void CheckPair()
        {
            var tempHand = hand.Concat(flop).ToList();

            if (!CheckGroupCount(tempHand, 2)) return;

            var twoOf = GetGroup(tempHand, 2);
            var cards = twoOf;
            var cardList = new List<ICard>();
            foreach (var card in twoOf)
            {
                cardList.Add(tempHand.First(c => c.ID == card.ID));
            }
            matchedFigures.Add("Pair", new PokerFigure("Pair", cardList, 100));
        }

        private void CheckThreeOfKind()
        {
            var tempHand = hand.Concat(flop).ToList();

            if (!CheckGroupCount(tempHand, 3)) return;

            var threeOf = GetGroup(tempHand, 3);
            var cards = threeOf;
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

        private string GetCardFigure(Card card)
        {
            if (card.Name.Length == 2) return card.Name[0].ToString();
            return card.Name.Substring(0, 2);
        }

        private void CheckStraight()
        {
            var tempHand = hand.Concat(flop).ToList();

            if (tempHand.Count < 5) return;

            var cardOrder = new List<string>(order);

            foreach(var card in tempHand)
            {
                var co = cardOrder.FirstOrDefault(c => c == GetCardFigure(card));
                var index = cardOrder.ToList().IndexOf(co);
                if (index == -1) continue;
                cardOrder[index] = string.Empty;
            }

            var emptyNumbers = new List<int>();
            //foreach (var element in cardOrder)
            //{
            //    if (element == string.Empty)
            //    {
            //        emptyNumbers.Add(cardOrder.IndexOf(element));
            //    }
            //}
            
            for (int q = 0; q < 13; q++)
            {
                if (cardOrder[q] == string.Empty) emptyNumbers.Add(q);
            }

            Console.WriteLine();

            for (int q = 0; q < 6; q++)
            {
                //var nextOne = emptyNumbers[q + 1];
                if (emptyNumbers[q] != (emptyNumbers[q +1] -1))
                {
                    return;
                }
            }

            Console.WriteLine();
            //tempHand.ForEach(x => cardOrder.First(c => c == x.Name[0].ToString()) = string.Empty);
        }

        //private bool CompareCardFigure(Card card, int number)
        //{
        //    var figure = card.Name[0];

        //    if (!Char.IsDigit(figure)) return false;

        //    if (figure == 0) return false;

        //    return (int)card.Name[0] == number;
        //}

        private bool CheckGroupCount(List<Card> cards, int elements)
        {
            return cards.GroupBy(x => x.Name[0])
                        .Where(group => group.Count() >= elements)
                        .Select(group => group)
                        .Any();
        }

        private IGrouping<char, Card> GetGroup(List<Card> cards, int elements)
        {
            return cards.GroupBy(x => x.Name[0])
                        .Where(group => group.Count() >= elements)
                        .Select(group => group)
                        .ToList()[0];
        }
    }
}
