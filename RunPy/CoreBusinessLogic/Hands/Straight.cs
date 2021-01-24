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
            return new List<ICard>();
        }

        public void Check()
        {
            var tempHand = hand.Concat(desk).ToList();
            tempHand = tempHand.OrderBy(c => c.Figure).ToList();

            if (tempHand.Count < 5) return;
            if (!IsInOrder(tempHand)) return;

            //CardList.Add("Straight", new PokerFigure("Straight", CardList, 100));

            Probability = 100;
        }
    }
}
