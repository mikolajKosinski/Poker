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
        ICard GetCard(string figure, string color);
        string CenterFigure(string path);
    }
}
