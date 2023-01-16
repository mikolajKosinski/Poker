using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static CoreBusinessLogic.CardRecognition;

namespace CoreBusinessLogic
{
    public interface ICardRecognition
    {
        //string RecogniseByPath(string path);
        //Tuple<int, int, int, int> GetSingleCardArea();
        //Tuple<int, int, int, int> GetArea();
        //Tuple<int, int, int, int> GetPosition(string path);
        //List<Tuple<decimal, decimal, decimal, decimal>> GetDesk();
        //string GetAllCards(string fileName);
        //string GetColorFigure(int cardsCount, string cardName);
        string GetCardsCountOnDesk();
        //Task<string> GetHandAsync();
        ICard GetCard(string figure, string color, int number, string area);
        //string CenterFigure(string path);
        //CardFigure RecognizeFigure(string imagePath, int number);
        //CardColor RecognizeColor(string imagePath, int number);
        event CardHandler CardRecognised;
        Dictionary<string, CardFigure> FigureDict { get; set; }
        Dictionary<string, CardColor> ColorDict { get; set; }
    }
}
