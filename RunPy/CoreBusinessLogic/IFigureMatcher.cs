using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBusinessLogic
{
    public interface IFigureMatcher
    {
        Dictionary<string, PokerFigure> CheckHand();
        void AddCardToFlop(string name);
        void AddCardToHand(string name);
    }
}
