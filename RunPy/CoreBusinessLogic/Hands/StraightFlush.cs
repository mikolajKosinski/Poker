using CoreBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBusinessLogic.Hands
{
    public class StraightFlush : BaseHandManager, IFigureManager
    {
        public StraightFlush(IList<ICard> hand, IList<ICard> desk) : base(hand, desk)
        {

        }

        public void Check()
        {
            var color = GetDominatingColor();
            var straight = new Straight(hand, desk);
            var gotSF = straight.Probability == 100 && straight.CardList.All(p => p.Color == color);

            if (gotSF)
            {
                Probability = 100;
                CardList = tempHand.OrderBy(p => p.Figure).Take(5).ToList();
            }
            else
            {
                Probability = (int)GetOddsPercentage(GetOuts().Count());
                var cardsByColor = straight.CardList.Where(p => p.Color == color).ToList();
                var highestFigure = cardsByColor.Max(p => p.Figure);
                var lowestFigure = highestFigure - 5;
                CardList = cardsByColor.Where(p => p.Figure >= lowestFigure).ToList();
            }
        }

        public IList<ICard> GetOuts()
        {
            CardsNeeded = GetNeededCardsCount();
            var straight = new Straight(hand, desk);
            var outs = straight.GetOuts();
            var color = GetDominatingColor();

            return outs.Where(p => p.Color == color).ToList();
        }

        private int GetNeededCardsCount()
        {
            var highestCardHand = tempHand.OrderByDescending(x => x.Figure).First();
            var lowestCard = highestCardHand.Figure - 5;
            var elements = tempHand.Where(p => p.Figure >= lowestCard).OrderByDescending(x => x.Figure).ToList();

            return 5 - elements.Count();
        }
    }
}
