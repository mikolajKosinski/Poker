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
            var flush = new Flush(hand, desk);
            flush.Check();

            if (IsInOrder(tempHand))
            { 
                Probability = 100; 
            }
            else
            {
                Probability = (int)GetOddsPercentage(GetOuts().Count());
            }
        }

        public IList<ICard> GetOuts()
        {
            var straight = new Straight(hand, desk);
            var outs = straight.GetOuts();
            var color = GetDominatingColorGroup()[0].Color;

            return outs.Where(p => p.Color == color).ToList();
        }
    }
}
