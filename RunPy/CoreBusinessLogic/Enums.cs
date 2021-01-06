using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBusinessLogic
{
    public class Enums
    {
        public enum CardType
        {
            HandFirstCard,
            HandSecondCard,
            FlopFirstCard,
            FlopSecondCard,
            FlopThirdCard
        }

        public enum PokerHands
        {
            Pair,
            ThreeOfKind,
            Straight,
            Flush,
            Full,
            FourOfKind,
            StraightFlush,
            RoyalFlush
        }
    }
}
