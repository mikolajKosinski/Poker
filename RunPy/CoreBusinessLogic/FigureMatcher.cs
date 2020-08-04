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
        private List<string> order;

        public FigureMatcher()
        {
            hand = new List<ICard>();
            flop = new List<ICard>();
            matchedFigures = new Dictionary<string, PokerFigure>();
            order = new List<string> {"2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
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

            var pair = GetGroup(tempHand, 2);
            var cardList = new List<ICard>();
            foreach (var card in pair)
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

        private string GetCardFigure(ICard card)
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
        }

        private void CheckFlush()
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

            if (!tempHand.GroupBy(x => x.Name.Last())
                        .Where(group => group.Count() >= 5)
                        .ToList()
                        .Any()) return;
            var key = tempHand.GroupBy(x => x.Name.Last())
                        .Where(group => group.Count() >= 5)
                        .Select(group => group.Key)
                        .First();
            cardList = tempHand.Where(x => x.Name.Last() == key).ToList();
            matchedFigures.Add("Straight Flush", new PokerFigure("Flush", cardList, 100));

            if (NotInOrder(cardList)) return;

            if(cardList.Any(x => x.Name.Contains("12")))
            {
                matchedFigures.Add("Royal Flush", new PokerFigure("Straight Flush", cardList, 100));
            }
            else
            {
                matchedFigures.Add("Straight Flush", new PokerFigure("Flush", cardList, 100));
            }
        }

        private bool NotInOrder(List<ICard> tempHand)
        {
            tempHand = tempHand.OrderBy(x => Convert.ToInt32(GetCardFigure(x))).ToList();
            for (int q = 0; q < tempHand.Count - 1; q++)
            {
                var firstCard = Convert.ToInt32(GetCardFigure(tempHand[q]));
                var nextCard = Convert.ToInt32(GetCardFigure(tempHand[q + 1]));

                if (firstCard != nextCard - 1) return true;
            }

            return false;
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

        private bool CheckGroupCount(List<ICard> cards, int elements)
        {
            return cards.GroupBy(x => x.Name[0])
                        .Where(group => group.Count() == elements)
                        .Select(group => group)
                        .Any();
        }

        private IGrouping<char, ICard> GetGroup(List<ICard> cards, int elements)
        {         
            return cards.GroupBy(x => x.Name[0])
                        .Where(group => group.Count() == elements)
                        .Select(group => group)
                        .ToList()[0];
        }
    }
}
