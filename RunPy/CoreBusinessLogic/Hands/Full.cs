using CoreBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBusinessLogic.Hands
{
    public class Full : BaseHandManager, IFigureManager
    {
        public Full(IList<ICard> hand, IList<ICard> desk) : base(hand, desk)
        {

        }

        public void Check()
        {
            var tempHand = hand.Concat(desk).ToList();

            if (!CheckGroupCount(tempHand, 2)) return;
            if (!CheckGroupCount(tempHand, 3)) return;

            var pair = GetGroup(tempHand, 2);
            var threeOfKind = GetGroup(tempHand, 3);

            if (!pair.Any() || !threeOfKind.Any()) return;

            var full = pair.Concat(threeOfKind).ToList();
            foreach (var card in full)
            {
                CardList.Add(tempHand.First(c => c.ID == card.ID));
            }

            Probability = 100;
        }

        public IList<ICard> GetOuts()
        {
            throw new NotImplementedException();
        }
    }
}
