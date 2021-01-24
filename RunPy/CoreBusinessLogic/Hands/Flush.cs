using CoreBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBusinessLogic.Hands
{
    public class Flush : BaseHandManager, IFigureManager
    {
        public Flush(IList<ICard> hand, IList<ICard> desk) : base(hand, desk)
        {

        }

        public void Check()
        {
            var tempHand = hand.Concat(desk).ToList();

            if (!tempHand.GroupBy(x => x.Color)
                        .Where(group => group.Count() >= 5)
                        .ToList()
                        .Any()) return;
            var key = tempHand.GroupBy(x => x.Color)
                        .Where(group => group.Count() >= 5)
                        .Select(group => group.Key)
                        .First();
            CardList = tempHand.Where(x => x.Color == key).ToList();
            Probability = 100;
        }

        public IList<ICard> GetOuts()
        {
            throw new NotImplementedException();
        }
    }
}
