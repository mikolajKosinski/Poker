using Autofac;
using CoreBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using static CoreBusinessLogic.Enums;

namespace CoreBusinessLogic
{
    public interface IFigureMatcher
    {
        IDictionary<PokerHands, IFigureManager> PokerHandsDict { get; set; }
        void CheckHand();
        void Clean();
        void AddCardToFlop(CardFigure figure, CardColor color);
        void AddCardToHand(CardFigure figure, CardColor color);
        void AddCardToFlop(ICard card);
        void AddCardToHand(ICard card);
        IList<ICard> GetHand(PokerHands hand);
        IContainer Container { get; set; }
        void SetPokerHandsDict();
    }
}
