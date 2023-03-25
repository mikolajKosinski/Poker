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
            RoyalFlush,
            none
        }

        public enum ProbabilityCountingFormula
        {
            first,
            second,
            third
        }

        public enum AnalyzeArea
        {
            Hand,
            Desk,
            All
        }

        public enum Stage
        {
            None,
            All,
            Flop,
            Turn,
            River
        }
    }
}
