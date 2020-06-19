using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreBusinessLogic
{
    public class FigureMatcher : IFigureMatcher
    {
        private List<Card> cardsOnFlop;
        private List<Card> cardsOnHand;
        private Dictionary<PokerFigure, int> possibleFigures;

        public FigureMatcher()
        {
            cardsOnHand = new List<Card>();
            cardsOnFlop = new List<Card>();
            possibleFigures = new Dictionary<PokerFigure, int>();
        }

        public Dictionary<PokerFigure, int> CheckHand()
        {
            var possibilities = new Dictionary<PokerFigure, int>();
            cardsOnHand.ForEach(card =>
            {
                possibilities.Add(new PokerFigure("Pair"), CheckSinglePair(card));
            });

            return possibilities;
        }

        public void AddCardToFlop(string name)
        {
            cardsOnFlop.Add(new Card(name));
        }

        public void AddCardToHand(string name)
        {
            cardsOnHand.Add(new Card(name));
        }

        private int CheckSinglePair(Card card)
        {
            if (cardsOnFlop.Any(c => c.Name[0] == card.Name[0])) return 100;
            if (cardsOnFlop.Count == 0) return 2;
            if (cardsOnHand.Count == 1) return 50;
            if (cardsOnHand.Any(c => c.ID != card.ID && c.Name[0] == card.Name[0])) return 100;

            return 0;
        }
    }
}
