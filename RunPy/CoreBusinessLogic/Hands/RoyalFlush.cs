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
            var color = GetDominatingColorGroup()[0].Color;
            var rf = new List<ICard>
            {
                new Card(CardFigure._As, color),
                new Card(CardFigure._King, color),
                new Card(CardFigure._Queen, color),
                new Card(CardFigure._Jack, color),
                new Card(CardFigure._10, color)
            };

            foreach(var item in tempHand)
            {
                var rfItem = rf.FirstOrDefault(p => p.Color == item.Color && p.Figure == item.Figure);

                if (rfItem != null) rf.Remove(rfItem);
            }

            return rf;
        }
    }
}
