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
            var cardList = new List<ICard>();

            foreach (var card in tempHand)
            {
                card.Name = card.Name.Replace("J", "11");
                card.Name = card.Name.Replace("Q", "12");
                card.Name = card.Name.Replace("K", "13");
                card.Name = card.Name.Replace("A", "14");
            }

            tempHand = tempHand.OrderBy(c => Convert.ToInt32(GetCardFigure(c))).ToList();

            for (int q = 0; q < tempHand.Count - 1; q++)
            {
                if (cardList.Count > 4) continue;

                var firstCard = Convert.ToInt32(GetCardFigure(tempHand[q]));
                var nextCard = Convert.ToInt32(GetCardFigure(tempHand[q + 1]));

                if (firstCard == nextCard - 1)
                {
                    if (!cardList.Contains(tempHand[q])) cardList.Add(tempHand[q]);
                    if (!cardList.Contains(tempHand[q + 1])) cardList.Add(tempHand[q + 1]);
                }
            }

            if(cardList.Count > 4) matchedFigures.Add("Straight", new PokerFigure("Straight", cardList, 100));

            //var tempHand = hand.Concat(flop).ToList();
            //var cardsInStraight = new List<string>();

            //if (tempHand.Count < 5) return;

            //var cardOrder = new List<string>(order);
            //var cardList = new List<ICard>();

            //foreach (var card in tempHand)
            //{
            //    var cardFigure = GetCardFigure(card);
            //    var index = cardOrder.ToList().IndexOf(cardFigure);
            //    if (index == -1)
            //    {
            //        continue;
            //    }
            //    cardList.Add(card);
            //    cardsInStraight.Add(cardOrder[index]);
            //    cardOrder[index] = string.Empty;                
            //}

            //var emptyNumbers = new List<int>();

            //for (int q = 0; q < 13; q++)
            //{
            //    if (cardOrder[q] == string.Empty)
            //    { 
            //        emptyNumbers.Add(q); 
            //    }
            //}

            //bool gotStragiht = false;

            //for(int q=0; q<2; q++)
            //{
            //    if (emptyNumbers[q] == emptyNumbers[q + 1] - 1
            //        && emptyNumbers[q+1] == emptyNumbers[q + 2] - 1
            //        && emptyNumbers[q+2] == emptyNumbers[q + 3] - 1
            //        && emptyNumbers[q+3] == emptyNumbers[q + 4] - 1)
            //        gotStragiht = true;
            //}

            //var elementsToRemove = new List<int>();

            //if(gotStragiht)
            //{
            //    for(int q=0; q<emptyNumbers.Count-1; q++)
            //    {
            //        if(emptyNumbers[q] != emptyNumbers[q+1] -1)
            //        {
            //            elementsToRemove.Add(emptyNumbers[q+1]);
            //        }
            //    }
            //}
            
            //foreach(var item in elementsToRemove)
            //{
            //    cardOrder.RemoveAt(item);
            //}

            //foreach(var card in cardOrder.Where(p => p != string.Empty))
            //{
            //    var cardToRemove = tempHand.First(c => c.Name.StartsWith(card));
            //    tempHand.Remove(cardToRemove);
            //}

            //matchedFigures.Add("ThreeOfKind", new PokerFigure("ThreeOfKind", cardList, 100));
        }
        
        private bool CheckStraight(List<int> cards)
        {
            for (int q = 0; q < 5; q++)
            {
                if (cards[q] != (cards[q + 1] - 1)) return false;
            }
            return true;
        }

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
