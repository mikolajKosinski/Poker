using CoreBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBusinessLogic.Hands
{
    public class FourOfKind : BaseHandManager, IFigureManager
    {
        public FourOfKind(IList<ICard> hand, IList<ICard> desk) : base(hand, desk)
        {

        }

        public void Check()
        {
            var tempHand = hand.Concat(desk).ToList();

            if (!CheckGroupCount(tempHand, 4)) return;

            var fourOf = GetGroup(tempHand, 4);
            foreach (var card in fourOf)
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
