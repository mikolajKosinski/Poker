using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBusinessLogic
{
    public interface IFigureMatcher
    {
        Dictionary<PokerFigure, int> CheckHand();
        void AddCardToFlop(string name);
        void AddCardToHand(string name);
    }
}
