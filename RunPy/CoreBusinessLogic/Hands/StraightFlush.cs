using CoreBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace CoreBusinessLogic.Hands
{
    public class StraightFlush : BaseHandManager, IFigureManager
    {
        Straight straight;

        public StraightFlush(IList<ICard> hand, IList<ICard> desk, Straight straight) : base(hand, desk)
        {
            this.straight = straight;
        }

        public IList<ICard> GetCards() => _availableCards;

        public string Name { get; } = "StraightFlush";
        public void Check()
        {
            decimal outs = GetOuts().Count();
            decimal cardsLeft = 52 - tempHand.Count();
            Probability = decimal.Round((outs / cardsLeft) * 100, 2);
            //Probability = (int)GetOddsPercentage(GetOuts().Count());
            //Probability = GetOuts().Count() * 4;
            OutsList = GetOuts().ToList();
            OutsCount = GetOuts().Count();
            //OutsCount = GetOuts().Count();
            //var color = GetDominatingColor();
            //var straight = new Straight(hand, desk);
            //var gotSF = straight.Probability == 100 && straight.CardList.All(p => p.Color == color);

            //if (gotSF)
            //{
            //    Probability = 100;
            //    CardList = tempHand.OrderBy(p => p.Figure).Take(5).ToList();
            //}
            //else
            //{
            //    Probability = (int)GetOddsPercentage(GetOuts().Count());
            //    var cardsByColor = straight.CardList.Where(p => p.Color == color).ToList();
            //    var highestFigure = cardsByColor.Max(p => p.Figure);
            //    var lowestFigure = highestFigure - 5;
            //    CardList = cardsByColor.Where(p => p.Figure >= lowestFigure).ToList();
            //    OutsList = GetOuts().ToList();
            //    OutsCount = OutsList.Count();
            //}
        }
        private IList<int> _allFigures = new List<int>() { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
        private List<ICard> _availableCards = new List<ICard>();
        public IList<ICard> GetOuts()
        {
            var color = GetDominatingColor();
            var local = tempHand.Where(x => x.Color== color).ToList();

            if (!local.Any()) return new List<ICard>();
            var cardsLeft = 7 - tempHand.Count();
            var possibleSets = new List<List<int>>();
            var outs = new List<ICard>();
            var allCards = GetDeck();

            for (int q = 0; q < 9; q++)
            {
                var set = new List<int>();
                for (int w = 0; w < 5; w++)
                {
                    set.Add(_allFigures[q + w]);
                }
                possibleSets.Add(set);
            }

            foreach (var set in possibleSets)
            {
                var temp = local.Select(p => Convert.ToInt32(p.Figure)).ToList();
                var neededFigures = set.Except(temp);
                if (neededFigures.Count() > cardsLeft)
                    continue;

                var cardForHand = local.Where(p => set.Contains(Convert.ToInt32(p.Figure))).Select(p => p).ToList();
                _availableCards.AddRange(cardForHand.Where(p => !_availableCards.Contains(p)));

                foreach (var figure in neededFigures)
                {
                    var possibleOuts = allCards.Select(p => p).Where(p => Convert.ToInt32(p.Figure) == figure);

                    foreach (var card in possibleOuts)
                    {
                        if (!local.Any(p => p.Figure == card.Figure) && !outs.Contains(card) && card.Color == color)
                        {
                            outs.Add(card);
                        }
                    }
                }
            }
            return outs;
        }

        private int GetNeededCardsCount()
        {
            var highestCardHand = tempHand.OrderByDescending(x => x.Figure).First();
            var lowestCard = highestCardHand.Figure - 5;
            var elements = tempHand.Where(p => p.Figure >= lowestCard).OrderByDescending(x => x.Figure).ToList();

            return 5 - elements.Count();
        }

        private List<ICard> GetOutsList()
        {
            var highestCardHand = tempHand.OrderByDescending(x => x.Figure).First();
            var lowestCard = highestCardHand.Figure - 5;
            var deck = GetDeckExceptTempHand();
            var elements = deck.Where(p => p.Figure > lowestCard).OrderByDescending(x => x.Figure).ToList();
            var result = elements.Except(tempHand).ToList();
            return result;
        }
    }
}
