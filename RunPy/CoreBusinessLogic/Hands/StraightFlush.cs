﻿using CoreBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
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

            if (IsInOrder(flush.CardList)) Probability = 100;
        }

        public IList<ICard> GetOuts()
        {
            throw new NotImplementedException();
        }
    }
}
