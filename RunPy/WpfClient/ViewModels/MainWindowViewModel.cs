using Autofac;
using CoreBusinessLogic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WpfClient.Interfaces;

namespace WpfClient.ViewModels
{
    public class MainWindowViewModel : IMainWindoViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<ICard> CardRecognized;
        private int _singleCardWidth;
        private CardArea _deskCardsArea;
        public bool ElementAdded { get; set; }
        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        ICardRecognition _cardRecognition;
        IFigureMatcher _figureMatcher;
        ICardManager _cardManager;
        public CardArea HandArea { get; set; }
        public CardArea DeskArea { get; set; }
        public CardArea SingleCardArea { get; set; }
        public ICommand HandSelectCommand { get; set; }
        public ICommand DeskSelectCommand { get; set; }
        public ICommand AnalyzeCommand { get; set; }
        public ICommand SingleCardCommand { get; set; }

        public ObservableCollection<ICard> RecoList { get; set; }

        private List<ICard> _recognizedCardsList;
        public List<ICard> RecognizedCardsList
        {
            get
            {
                return _recognizedCardsList;
            }
            set
            {
                _recognizedCardsList = value;
                NotifyPropertyChanged("RecognizedCardsList");
            }
        }


        public MainWindowViewModel(ICardRecognition cardRecognition, IFigureMatcher figureMatcher, ICardManager cardManager)
        {
            RecognizedCardsList = new List<ICard>();
            RecoList = new ObservableCollection<ICard>();
            _cardManager = cardManager;
            _cardRecognition = cardRecognition;
            _figureMatcher = figureMatcher;
            DeskSelectCommand = new CustomCommand(SelectDesk, CanSelect);
            HandSelectCommand = new CustomCommand(SelectHand, CanSelect);
            SingleCardCommand = new CustomCommand(SelectSingleCard, CanSelect);
            AnalyzeCommand = new CustomCommand(Analyze, CanSelect);
        }

        public bool CanSelect(object parameter)
        {
            return true;
        }

        private void SelectHand(object parameter)
        {
            SelectArea(AnalyzeType.Hand);
        }

        private void SelectDesk(object parameter)
        {
            SelectArea(AnalyzeType.Desk);
        }

        private void SelectSingleCard(object parameter)
        {
            SelectArea(AnalyzeType.SingleCard);
        }

        private void SelectArea(AnalyzeType at)
        {
            var pageAnalyze = new ScreenAnalyzePage(this, at);
            pageAnalyze.Closed += PageAnalyze_Closed;
            ((App)Application.Current).HideWindow();
            pageAnalyze.Show();
        }

        private void PageAnalyze_Closed(object sender, EventArgs e)
        {
            var analyze = (sender as ScreenAnalyzePage).AT;
            ((App)Application.Current).ShowWindow();

            if (analyze == AnalyzeType.Desk) SaveCardsArea(DeskArea, analyze);
            if (analyze == AnalyzeType.Hand) SaveCardsArea(HandArea, analyze);
            if (analyze == AnalyzeType.SingleCard) SaveCardsArea(SingleCardArea, analyze);
        }

        public void Analyze(object sender)
        {
            //SaveCardsArea(DeskArea, AnalyzeType.Desk);
            //SaveCardsArea(HandArea, AnalyzeType.Hand);

        }

        private void GetSingleCardArea()
        {
            double screenLeft = SystemParameters.VirtualScreenLeft;
            double screenTop = SystemParameters.VirtualScreenTop;
            double screenWidth = SystemParameters.VirtualScreenWidth;
            double screenHeight = SystemParameters.VirtualScreenHeight;

            using (Bitmap bmp = new Bitmap((int)screenWidth,
                (int)screenHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    var cardsAreaName = @$"C:\Users\Mikolaj\PycharmProjects\pythonProject1\allCards.png";
                    var cutOff = @$"C:\Users\Mikolaj\PycharmProjects\pythonProject1\allCardsCutOff.png";
                }
            }
        }

        private void SaveCardsArea(CardArea item, AnalyzeType at)
        {
            double screenLeft = SystemParameters.VirtualScreenLeft;
            double screenTop = SystemParameters.VirtualScreenTop;
            double screenWidth = SystemParameters.VirtualScreenWidth;
            double screenHeight = SystemParameters.VirtualScreenHeight;
            //var cardName = at == AnalyzeType.Desk ? "desk" : "hand";
            //int areaWidth = at == AnalyzeType.Desk ? 450 : 50;
            var areaWidth = 450;
            var cutOffName = "";
            if (at == AnalyzeType.Desk)
            {

            }
            if (at == AnalyzeType.Hand) cutOffName = "handCutOff";
            if (at == AnalyzeType.SingleCard)
            {
                cutOffName = "singleCutOff";
                areaWidth = 70;
            }

            using (Bitmap bmp = new Bitmap((int)screenWidth,
                (int)screenHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    var cardsAreaName = @$"C:\Users\Mikolaj\PycharmProjects\pythonProject1\allCards.png";
                    var cutOff = @$"C:\Users\Mikolaj\PycharmProjects\pythonProject1\{cutOffName}.png";
                    g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);
                    Bitmap allCards = bmp.Clone(new Rectangle((int)item.xStart, (int)item.yStart, areaWidth, 150), bmp.PixelFormat);
                    allCards.Save(cardsAreaName);

                    var points = _cardRecognition.GetCardsArea();
                    var width = points.Item2 - points.Item1;

                    if (at == AnalyzeType.SingleCard) width -= 2;
                    

                    var height = points.Item4 - points.Item3;
                    if (at == AnalyzeType.SingleCard) height += 2;

                    var xStart = (int)item.xStart + points.Item1;
                    var yStart = (int)item.yStart + points.Item3;
                    if (at == AnalyzeType.SingleCard)
                    {
                        xStart -= 3;
                        yStart -= 5;
                    }

                    Bitmap cutOffAll = bmp.Clone(
                        new Rectangle(
                        xStart, 
                        yStart, 
                        width, 
                        height), bmp.PixelFormat);
                    _singleCardWidth = width;
                    cutOffAll.Save(cutOff);
                    Console.WriteLine();
                }
            }
        }

        private Tuple<string,string> GetCardImageName(CardArea item)
        {
            double screenLeft = SystemParameters.VirtualScreenLeft;
            double screenTop = SystemParameters.VirtualScreenTop;
            double screenWidth = SystemParameters.VirtualScreenWidth;
            double screenHeight = SystemParameters.VirtualScreenHeight;

            using (Bitmap bmp = new Bitmap((int)screenWidth,
                (int)screenHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    var guid = new Guid();
                    var cardsAreaName = @"C:\Users\Mikolaj\PycharmProjects\pythonProject1\figure.png";
                    var figureName = @"C:\Users\Mikolaj\PycharmProjects\pythonProject1\figure.png";
                    var colorName = @"C:\Users\Mikolaj\PycharmProjects\pythonProject1\color.png";
                    var figureWidth = (int)(item.xEnd - item.xStart) / 2;
                    var figureHeight = (int)(item.yEnd - item.yStart) / 2; 
                    g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);
                    Bitmap figure = bmp.Clone(new Rectangle((int)item.xStart, (int)item.yStart-2, 20, 25), bmp.PixelFormat);
                    Bitmap color = bmp.Clone(new Rectangle((int)item.xStart, (int)item.yStart + 22, 20, 20), bmp.PixelFormat);
                    //Bitmap croppedFigure = card.Clone(new Rectangle((int)item.xStart, (int)item.yStart - 2, 20, 35), bmp.PixelFormat);
                    Bitmap resizedFigure = new Bitmap(figure, new System.Drawing.Size(30,30));
                    Bitmap resizedColor = new Bitmap(color, new System.Drawing.Size(15, 15));
                    //card.Save(figureName);
                    resizedFigure.Save(figureName);
                    resizedFigure.Save("figure.png");
                    resizedColor.Save(colorName);
                    return new Tuple<string, string>(figureName, colorName);
                }
            }
        }

        public void TakeScreenShoot(System.Windows.Point pointToWindow, System.Windows.Point pointToScreen)
        {
            //var (xStart, yStart, xWidth, yWidth) = GetCaptureArea(pointToScreen, pointToWindow);
            //Rectangle rect = new Rectangle(xStart, yStart, xWidth, yWidth);
            //Bitmap bmp = new Bitmap(150, 110);
            //Graphics g = Graphics.FromImage(bmp);
            //g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
            //bmp.Save("image.jpg", ImageFormat.Jpeg);

            _figureMatcher.AddCardToFlop(CardFigure._King, CardColor.spade);
            _figureMatcher.AddCardToFlop(CardFigure._4, CardColor.club);
            _figureMatcher.AddCardToFlop(CardFigure._10, CardColor.club);
            _figureMatcher.AddCardToFlop(CardFigure._5, CardColor.diamond);
            _figureMatcher.AddCardToFlop(CardFigure._Queen, CardColor.club);

            _figureMatcher.AddCardToHand(CardFigure._King, CardColor.club);
            _figureMatcher.AddCardToHand(CardFigure._King, CardColor.diamond);
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image.jpg");
           // var card = manager.GetCardByImage(path);// C:\\Users\\mkosi\\PycharmProjects\\tensorEnv\\dataset\\2C\\test.jpg");

            _figureMatcher.CheckHand();

            var cr = new CardRecognition();
            var res = cr.GetCard();

            Console.WriteLine();
        }

        private (int xStart, int yStart, int xWidth, int yWidth) GetCaptureArea(System.Windows.Point pointToScreen, System.Windows.Point pointToWindow)
        {
            return ((int)pointToScreen.X, (int)pointToScreen.Y, 100, 100);
        }
    }
}
