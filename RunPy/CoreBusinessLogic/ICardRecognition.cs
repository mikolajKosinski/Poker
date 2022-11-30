using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreBusinessLogic
{
    public interface ICardRecognition
    {
        string RecogniseByPath(string path);
        Tuple<int, int, int, int> GetSingleCardArea();
        Tuple<int, int, int, int> GetArea();
        Tuple<int, int, int, int> GetPosition(string path);
        List<Tuple<decimal, decimal, decimal, decimal>> GetDesk();
        string GetAllCards(string fileName);
        string GetColorFigure(int cardsCount, string cardName);
        string GetHand();
        Task<string> GetHandAsync();
        ICard GetCard(string figure, string color);
        string CenterFigure(string path);
    }
}
