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

        public Dictionary<string, PokerFigure> CheckHand()
        {
            var possibilities = new Dictionary<string, PokerFigure>();
            cardsOnHand.ForEach(card =>
            {
                var pair = CheckPair(card);
                var name = possibilities.Keys.Contains("Pair") ? "SecondPair" : "Pair";
                possibilities.Add(name, pair);
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

        private PokerFigure CheckPair(Card card)
        {
            if (cardsOnFlop.Any(c => c.Name[0] == card.Name[0]))
            {
                var cardList = new List<ICard>();
                cardList.Add(cardsOnFlop.First(c => c.Name[0] == card.Name[0]));
                cardList.Add(cardsOnHand.First(c => c.Name[0] == card.Name[0]));

                return new PokerFigure("Pair", cardList, 100);
            }

            return null;
        }
    }
}
