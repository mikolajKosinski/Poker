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

        public FigureMatcher()
        {
            hand = new List<Card>();
            flop = new List<Card>();
            matchedFigures = new Dictionary<string, PokerFigure>();
        }

        public Dictionary<string, PokerFigure> CheckHand()
        {
            CheckPair();
            CheckThreeOfKind();
            CheckFourOfKind();

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
