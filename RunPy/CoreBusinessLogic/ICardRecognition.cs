using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static CoreBusinessLogic.CardRecognition;
using static CoreBusinessLogic.Enums;

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
        Task<ICard> GetCard(string figure, string color, int number, AnalyzeArea area, Stage stage);
        //string CenterFigure(string path);
        //CardFigure RecognizeFigure(string imagePath, int number);
        //CardColor RecognizeColor(string imagePath, int number);
        event CardHandler CardRecognised;
        Dictionary<string, CardFigure> FigureDict { get; set; }
        Dictionary<string, CardColor> ColorDict { get; set; }
        string GetDetect(string path, string area, int number);
        Task PredictCard(string fileName, string imagePath, AnalyzeArea area, recoType fc, string number, Stage stage);
        Task<int> DetectCard(string imagePath, AnalyzeArea area, Stage stage);

        //Task UploadImageToBlob(string imagePath);
    }
}
