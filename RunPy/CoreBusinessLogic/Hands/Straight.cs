using CoreBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBusinessLogic.Hands
{
    public class Straight : BaseHandManager, IFigureManager
    {
        public Straight(IList<ICard> hand, IList<ICard> desk) : base(hand, desk)
        {
            Probability = 0;
        }

        public IList<ICard> GetOuts()
        {
            var highestCardHand = tempHand.OrderByDescending(x => x.Figure).First();
            var highestPossibleFigure = highestCardHand.Figure + 2;

            if ((int)highestPossibleFigure > 14) highestPossibleFigure = CardFigure._As;

            var lowestCard = highestPossibleFigure - 5;
            var restFromDeck = GetDeckExceptTempHand();
            var outs = new List<ICard>();
            
            for(var figure = highestPossibleFigure; figure >= lowestCard; figure--)
            {
                if(!tempHand.Any(x => x.Figure == figure))
                {
                    outs.AddRange(restFromDeck.Where(p => p.Figure == figure).ToList());
                }
            }

            return outs;
        }

        public void Check()
        {
            var tempHand = hand.Concat(desk).ToList();
            tempHand = tempHand.OrderBy(c => c.Figure).ToList();

            if (tempHand.Count < 5) return;
            if (!IsInOrder(tempHand)) return;

            Probability = 100;
        }
    }
}
