using Autofac;
using CoreBusinessLogic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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
        private int _singleCardHeight;
        private int _deskOffset;
        private int _deskWidth;
        private int _handWidth;
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
        public ICommand AddCommand { get; set; }
        public ICommand RemoveCommand { get; set; }

        private ObservableCollection<ICard> _handCards;
        public ObservableCollection<ICard> HandCards
        {
            get
            {
                if (_handCards == null)
                {
                    _handCards = new ObservableCollection<ICard>();
                }
                return _handCards;
            }
            set
            {
                _handCards = value;
                NotifyPropertyChanged(nameof(HandCards));
            }
        }

        private ObservableCollection<ICard> _deskCards;
        public ObservableCollection<ICard> DeskCards
        {
            get
            {
                if (_deskCards == null)
                {
                    _deskCards = new ObservableCollection<ICard>();
                }
                return _deskCards;
            }
            set
            {
                _deskCards = value;
                NotifyPropertyChanged(nameof(DeskCards));
            }
        }

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
            _cardManager = cardManager;
            _cardRecognition = cardRecognition;
            _figureMatcher = figureMatcher;
            DeskSelectCommand = new CustomCommand(SelectDesk, CanSelect);
            HandSelectCommand = new CustomCommand(SelectHand, CanSelect);
            SingleCardCommand = new CustomCommand(SelectSingleCard, CanSelect);
            AnalyzeCommand = new CustomCommand(Analyze, CanSelect);
            AddCommand = new CustomCommand(Add, CanSelect);
            RemoveCommand = new CustomCommand(Remove, CanSelect);
        }

        public void Add(object parameter)
        {
            DeskCards.Add(new Card(CardFigure._2, CardColor.club));
        }

        public void Remove(object parameter)
        {
            ClearList();
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
            var at = (sender as ScreenAnalyzePage).AT;
            ((App)Application.Current).ShowWindow();
            CardArea area = SingleCardArea;

            if (at == AnalyzeType.Desk) area = DeskArea;
            if (at == AnalyzeType.Hand) area = HandArea;            

            //GetAreas(area, at);
        }

        private void ClearList()
        {
            var count = DeskCards.Count();
            foreach(var item in DeskCards.ToList())
            {
                DeskCards.Remove(DeskCards.First());
            }
        }

        public void Analyze(object sender)
        {
            ClearList();
            var desk = GetAreaBitmap(DeskArea, AnalyzeType.Desk);

            if(desk == null)
            {
                MessageBox.Show("Try again");
                return;
            }

            //var hand = GetAreaBitmap(HandArea, AnalyzeType.Hand);
            var single = GetAreaBitmap(SingleCardArea, AnalyzeType.SingleCard);

            AnalyzeDesk(desk);            
        }

        private void AnalyzeDesk(Bitmap desk)
        {
            double exact = (double)_deskWidth / (double)_singleCardWidth;
            int count = _deskWidth / _singleCardWidth;

            if (exact < 3) count++;
            if (3 < exact && exact < 4) count++;
            var topName = @$"C:\Users\Mikolaj\PycharmProjects\pythonProject1\top.png";
            var top = desk.Clone(new Rectangle(0, 0, _deskWidth, 40), desk.PixelFormat);
            var _offsetCount = 4;
            //if (count > 3) _offsetCount++;
            //if (count > 4) _offsetCount++;

            if(count == 5) _deskOffset = (_deskWidth - (_singleCardWidth * count)) / _offsetCount;
            var width = _singleCardWidth + _deskOffset;
            

            for(int idx = 0; idx < count; idx++)
            {
                if ((width * idx) > _deskWidth) return;

                var card = top.Clone(new Rectangle(width * idx, 5, 30, 30), top.PixelFormat);
                card.Save(@$"C:\Users\Mikolaj\PycharmProjects\pythonProject1\allCards.png");                
                var points = _cardRecognition.GetSingleCardArea();
                var xStart = (width * idx) + points.Item1;
                var yStart = 5;
                var figureWidth = points.Item2 - 5;
                var figureHeight = points.Item4 - points.Item3;
                var figure = top.Clone(new Rectangle(xStart, yStart, figureWidth, figureHeight), top.PixelFormat);

                var bottom = desk.Clone(new Rectangle(xStart, figureHeight, figureWidth, figureHeight), desk.PixelFormat);
                bottom.Save(@$"C:\Users\Mikolaj\PycharmProjects\pythonProject1\color.png");
                //points = _cardRecognition.GetSingleCardArea();
                //var color = bottom.Clone(new Rectangle(points.Item1, points.Item3, points.Item2 - points.Item1, points.Item4 - points.Item3), desk.PixelFormat);
                //color.Save(@$"C:\Users\Mikolaj\PycharmProjects\pythonProject1\color.png");
                bottom.Save("color.png");
                //figure = GetRepaintedFromBlack(figure, Color.White);
                figure = new Bitmap(figure, new System.Drawing.Size(30, 30));
                figure.Save("figure.png");
                //color.Save("color.png");
                var cardd = _cardRecognition.GetCard();
                DeskCards.Add(cardd);
            }

            Console.WriteLine();
        }

        //private void GetSingleCardArea()
        //{
        //    double screenLeft = SystemParameters.VirtualScreenLeft;
        //    double screenTop = SystemParameters.VirtualScreenTop;
        //    double screenWidth = SystemParameters.VirtualScreenWidth;
        //    double screenHeight = SystemParameters.VirtualScreenHeight;

        //    using (Bitmap bmp = new Bitmap((int)screenWidth,
        //        (int)screenHeight))
        //    {
        //        using (Graphics g = Graphics.FromImage(bmp))
        //        {
        //            var cardsAreaName = @$"C:\Users\Mikolaj\PycharmProjects\pythonProject1\allCards.png";
        //            var cutOff = @$"C:\Users\Mikolaj\PycharmProjects\pythonProject1\allCardsCutOff.png";
        //        }
        //    }
        //}
        
        private Bitmap GetAreaBitmap(CardArea item, AnalyzeType at)
        {
            double screenLeft = SystemParameters.VirtualScreenLeft;
            double screenTop = SystemParameters.VirtualScreenTop;
            double screenWidth = SystemParameters.VirtualScreenWidth;
            double screenHeight = SystemParameters.VirtualScreenHeight;
            Tuple<int, int, int, int> points = new Tuple<int, int, int, int>(0,0,0,0);
            int width = GetWidth(at);
            int cutOffWidth = 0;
            int cutOffHeight = 0;
            var name = GetCutOffName(at);

            using (Bitmap bmp = new Bitmap((int)screenWidth,
                (int)screenHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    var cardsAreaName = @$"C:\Users\Mikolaj\PycharmProjects\pythonProject1\allCards.png";
                    var cutOffName = @$"C:\Users\Mikolaj\PycharmProjects\pythonProject1\{name}.png";
                    g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);
                    Bitmap basicPicture = bmp.Clone(new Rectangle((int)item.xStart, (int)item.yStart, width, 100), bmp.PixelFormat);
                    basicPicture = GetRepaintedFromGreen(basicPicture, Color.Black);
                    basicPicture.Save(cardsAreaName);
                    points = GetPoints(at);
                    cutOffWidth = points.Item2 - points.Item1;
                    cutOffHeight = points.Item4 - points.Item3;

                    if (at == AnalyzeType.SingleCard)
                    {
                        _singleCardWidth = cutOffWidth;
                        _singleCardHeight = cutOffHeight;
                    }
                    if (at == AnalyzeType.Desk) _deskWidth = cutOffWidth;
                    if (at == AnalyzeType.Hand) _handWidth = cutOffWidth;

                    var height = points.Item4 - points.Item3;
                    var xStart = points.Item1;
                    var yStart = points.Item3;

                    try
                    {
                        Bitmap cutOffImg = basicPicture.Clone(
                            new Rectangle(
                            xStart,
                            yStart,
                            cutOffWidth,
                            height), basicPicture.PixelFormat);
                        cutOffImg.Save(cutOffName);
                        return cutOffImg;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
        }

        private Tuple<int, int, int, int> GetPoints(AnalyzeType at)
        {
            if (at == AnalyzeType.SingleCard) return _cardRecognition.GetSingleCardArea();
            return _cardRecognition.GetArea();
        }

        private string GetCutOffName(AnalyzeType at)
        {
            if (at == AnalyzeType.SingleCard) return "singleCutOff";
            if (at == AnalyzeType.Hand) return "HandCutOff";
            return "DeskCutOff";
        }

        private int GetWidth(AnalyzeType at)
        {
            if (at == AnalyzeType.SingleCard) return 120;
            if (at == AnalyzeType.Hand) return 250;
            return 450;
        }

        private Bitmap GetRepaintedFromGreen(Bitmap basicPicture, Color color)
        {
            for (int x = 0; x < basicPicture.Width; x++)
            {
                for (int y = 0; y < basicPicture.Height; y++)
                {
                    var c = basicPicture.GetPixel(x, y);
                    if (c.G > 70 && c.B < 90)
                    {
                        basicPicture.SetPixel(x, y, color);
                    }
                }
            }

            return basicPicture;
        }

        private Bitmap GetRepaintedFromBlack(Bitmap basicPicture, Color color)
        {
            for (int x = 0; x < basicPicture.Width; x++)
            {
                for (int y = 0; y < basicPicture.Height; y++)
                {
                    var c = basicPicture.GetPixel(x, y);
                    if (c.G == 255 && c.B  == 255)
                    {
                        basicPicture.SetPixel(x, y, color);
                    }
                }
            }

            return basicPicture;
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
