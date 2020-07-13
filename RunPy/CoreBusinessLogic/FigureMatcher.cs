using System;
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
            hand.ForEach(card =>
            {
                if (flop.Any(c => c.Name[0] == card.Name[0]))
                {
                    var cardList = new List<ICard>
                    {
                        flop.First(c => c.Name[0] == card.Name[0]),
                        hand.First(c => c.Name[0] == card.Name[0])
                    };
                    var name = matchedFigures.Keys.Contains("Pair") ? "SecondPair" : "Pair";
                    matchedFigures.Add(name, new PokerFigure("Pair", cardList, 100));
                }
            });
        }

        private void CheckThreeOfKind()
        {
            var tempHand = hand.Concat(flop).ToList();
            var threeOf = tempHand.GroupBy(x => x.Name[0])
                        .Where(group => group.Count() > 2)
                        .Select(group => group)
                        .ToList()[0];
            var cardList = new List<ICard>();
            foreach (var card in threeOf)
            {
                cardList.Add(tempHand.First(c => c.ID == card.ID));
            }
            matchedFigures.Add("ThreeOfKind", new PokerFigure("ThreeOfKind", cardList, 100));
        }
    }
}
