using CoreBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBusinessLogic.Hands
{
    public class RoyalFlush : BaseHandManager, IFigureManager
    {
        public RoyalFlush(IList<ICard> hand, IList<ICard> desk) : base(hand, desk)
        {

        }

        public void Check()
        {
            var flush = new Flush(hand, desk);
            flush.Check();

            if (flush.Probability != 100) return;

            if (NotInOrder(flush.CardList)) return;

            if (flush.CardList.Any(x => x.Figure == CardFigure._As)) Probability = 100;
        }

        public IList<ICard> GetOuts()
        {
            throw new NotImplementedException();
        }
    }
}
