using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBusinessLogic
{
    public interface IFigureMatcher
    {
        void CheckHand();
        void AddCardToFlop(CardFigure figure, CardColor color);
        void AddCardToHand(CardFigure figure, CardColor color);
        void AddCardToFlop(ICard card);
        void AddCardToHand(ICard card);
    }
}
