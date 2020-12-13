using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBusinessLogic
{
    public interface IFigureMatcher
    {
        Dictionary<string, PokerFigure> CheckHand();
        void AddCardToFlop(CardFigure figure, CardColor color);
        void AddCardToHand(CardFigure figure, CardColor color);
    }
}
