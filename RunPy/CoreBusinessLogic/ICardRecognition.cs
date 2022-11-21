using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBusinessLogic
{
    public interface ICardRecognition
    {
        string RecogniseByPath(string path);
        Tuple<int, int, int, int> GetSingleCardArea();
        Tuple<int, int, int, int> GetArea();
        Tuple<int, int, int, int> GetPosition(string path);
        List<Tuple<decimal, decimal, decimal, decimal>> GetDesk();
        string GetAllCards();
        string GetColorFigure(int cardsCount);
        ICard GetCard(string figure, string color);
        string CenterFigure(string path);
    }
}
