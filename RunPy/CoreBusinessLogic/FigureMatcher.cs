using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreBusinessLogic
{
    public class FigureMatcher : IFigureMatcher
    {
        private List<Card> cardsOnTable;
        private List<Card> cardsOnHand;
        private Dictionary<PokerFigure, int> possibleFigures;

        public FigureMatcher()
        {
            cardsOnHand = new List<Card>();
            cardsOnTable = new List<Card>();
            possibleFigures = new Dictionary<PokerFigure, int>();
        }

        public Dictionary<PokerFigure, int> CheckHand()
        {
            var possibilities = new Dictionary<PokerFigure, int>();
            cardsOnHand.ForEach(card =>
            {
                possibilities.Add(new PokerFigure("Pair"), CheckPair(card));
            });

            return possibilities;
        }

        public void AddCardToTable(string name)
        {
            cardsOnTable.Add(new Card(name));
        }

        public void AddCardToHand(string name)
        {
            cardsOnHand.Add(new Card(name));
        }

        private int CheckPair(Card card)
        {
            if (cardsOnTable.Any(c => c.Name[0] == card.Name[0])) return 100;
            if (cardsOnTable.Count < 3) return 50;
            if (cardsOnHand.Count < 3) return 50;
            if (cardsOnHand.Any(c => c.ID != card.ID && c.Name[0] == card.Name[0])) return 100;

            return 0;
        }
    }
}
