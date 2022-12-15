using Autofac;
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

        public StraightFlush(IList<ICard> hand, IList<ICard> desk, Straight straight, ISettings container) : base(hand, desk, container)
        {
            this.straight = straight;
        }

        public void UpdateHand(ICard card) => hand.Add(card);
        public void UpdateDesk(ICard card) => desk.Add(card);

        public string Name { get; } = "StraightFlush";
        public void Check()
        {
            OutsList = GetOuts().ToList();
            Probability = GetProbability();

            if (Probability == 0 || tempHand.Count() == 7) return;

            //OutsList = outs.ToList();
            //OutsCount = GetOuts().Count();
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
            ClearDesk();
            return outs;
        }

        //private int GetNeededCardsCount()
        //{
        //    var highestCardHand = tempHand.OrderByDescending(x => x.Figure).First();
        //    var lowestCard = highestCardHand.Figure - 5;
        //    var elements = tempHand.Where(p => p.Figure >= lowestCard).OrderByDescending(x => x.Figure).ToList();

        //    return 5 - elements.Count();
        //}

        //private List<ICard> GetOutsList()
        //{
        //    var highestCardHand = tempHand.OrderByDescending(x => x.Figure).First();
        //    var lowestCard = highestCardHand.Figure - 5;
        //    var deck = GetDeckExceptTempHand();
        //    var elements = deck.Where(p => p.Figure > lowestCard).OrderByDescending(x => x.Figure).ToList();
        //    var result = elements.Except(tempHand).ToList();
        //    return result;
        //}
    }
}
