using CoreBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBusinessLogic.Hands
{
    public class ThreeOfKind : BaseHandManager, IFigureManager
    {
        public ThreeOfKind(IList<ICard> hand, IList<ICard> desk) : base(hand, desk) { }

        public IList<ICard> GetOuts()
        {
            return new List<ICard>();
        }

        public void Check()
        {
            var tempHand = hand.Concat(desk).ToList();
            if (CheckGroupCount(tempHand, 3))
            {
                var threeOf = GetGroup(tempHand, 3);
                foreach (var card in threeOf)
                {
                    CardList.Add(tempHand.First(c => c.ID == card.ID));
                }
                Probability = 100;
            }
        }
    }
}
