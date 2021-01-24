using CoreBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBusinessLogic.Hands
{
    public class Pair : BaseHandManager, IFigureManager
    {
        public Pair(IList<ICard> hand, IList<ICard> desk) : base (hand, desk)
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
            if (!CheckGroupCount(tempHand, 2)) return;
            var pair = GetGroup(tempHand, 2);
            if (pair.Any())
            {
                Probability = 100;
                CardList.Add(pair.First());
            }
        }
    }
}
