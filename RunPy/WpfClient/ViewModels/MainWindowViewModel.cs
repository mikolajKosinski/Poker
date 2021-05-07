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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WpfClient.Interfaces;

namespace WpfClient.ViewModels
{    
    public class MainWindowViewModel : IMainWindoViewModel, INotifyPropertyChanged
    {
        private enum CardTypeEnum
        {
            Desk = 0,
            Hand = 1
        }

        private bool _areasVisible;
        public bool IsAreasVisible
        {
            get
            {
                return _areasVisible;
            }
            set
            {
                _areasVisible = value;
                NotifyPropertyChanged(nameof(IsAreasVisible));
            }
        }

        private bool _isSettingsVisible;
        public bool IsSettingsVisible
        {
            get
            {
                return _isSettingsVisible;
            }
            set
            {
                _isSettingsVisible = value;
                NotifyPropertyChanged(nameof(IsSettingsVisible));
            }
        }

        private bool _isMainVisible;
        public bool IsMainVisible
        {
            get
            {
                return _isMainVisible;
            }
            set
            {
                _isMainVisible = value;
                NotifyPropertyChanged(nameof(IsMainVisible));
            }
        }

        public IAreasWindowViewModel AreasViewModel { get; set; }
        public ISettingsViewModel SettingsViewModel { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<ICard> CardRecognized;
        private int _singleCardWidth;
        //private int _singleCardHeight;
        private int _deskOffset;
        private int _handOffset;
        private int _deskWidth;
        private int _handWidth;
        private bool _handRecognized;
        private bool _deskRecognized;
        private int _deskCardsCount;
        private List<Tuple<int, int, int, int>> _deskPointsList;
        private List<Tuple<int, int, int, int>> _handPointsList;
        private CardArea _deskCardsArea;
        //private event EventHandler<CardRecognitionEventArgs> _cardRecognized;
        public bool ElementAdded { get; set; }
        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //private void OnCardRecognized(CardRecognitionEventArgs e)
        //{
        //    _cardRecognized?.Invoke(this, e);
        //}

        ICardRecognition _cardRecognition;
        IFigureMatcher _figureMatcher;
        ICardManager _cardManager;
        public CardArea HandArea { get; set; }
        public CardArea DeskArea { get; set; }
        public CardArea SingleCardArea { get; set; }
       
        public ICommand AnalyzeCommand { get; set; }

        public ICommand AreasWindowCommand { get; set; }
        public ICommand MainWindowCommand { get; set; }

        public ICommand SettingsWindowCommand { get; set; }
        //public ICommand AddCommand { get; set; }
        //public ICommand RemoveCommand { get; set; }

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

        private ObservableCollection<IHandDescription> _hands;
        public ObservableCollection<IHandDescription> Hands
        {
            get
            {
                if (_hands == null)
                {
                    _hands = new ObservableCollection<IHandDescription>();
                }
                return _hands;
            }
            set
            {
                _hands = value;
                NotifyPropertyChanged(nameof(Hands));
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
            AreasViewModel = new AreasWindowViewModel(this);
            SettingsViewModel = new SettingsViewModel(this);
            RecognizedCardsList = new List<ICard>();
            DeskCards = new ObservableCollection<ICard>();
            HandCards = new ObservableCollection<ICard>();
            _cardManager = cardManager;
            _cardRecognition = cardRecognition;
            _figureMatcher = figureMatcher;
            _deskPointsList = new List<Tuple<int, int, int, int>>();
            _handPointsList = new List<Tuple<int, int, int, int>>();
            //_cardRecognized += MainWindowViewModel__cardRecognized;
            AnalyzeCommand = new CustomCommand(Analyze, CanSelect);
            AreasWindowCommand = new CustomCommand(ShowAreas, CanSelect);
            MainWindowCommand = new CustomCommand(ShowMainWindow, CanSelect);
            SettingsWindowCommand = new CustomCommand(ShowSettings, CanSelect);
            //AddCommand = new CustomCommand(Add, CanSelect);
            //RemoveCommand = new CustomCommand(Remove, CanSelect);
            _deskCards.CollectionChanged += _deskCards_CollectionChanged;
            _handCards.CollectionChanged += _handCards_CollectionChanged;
            IsAreasVisible = false;
        }

        private void _handCards_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _deskRecognized = DeskCards.Count == _deskCardsCount;
            _handRecognized = HandCards.Count == 2;
            _analyze();
        }

        private void _deskCards_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _deskRecognized = DeskCards.Count == _deskCardsCount;
            _handRecognized = HandCards.Count == 2;
            _analyze();
        }

        private void _analyze()
        {
            if(_handRecognized && _deskRecognized)
            {
                var matcher = new FigureMatcher();
                
                foreach(var card in DeskCards)
                {
                    matcher.AddCardToFlop(card);
                }

                foreach(var card in HandCards)
                {
                    matcher.AddCardToHand(card);
                }

                matcher.CheckHand();
                foreach (var key in matcher.PokerHandsDict.Keys)
                {
                    var hand = matcher.PokerHandsDict[key];
                    Hands.Add(new HandDescription(key.ToString(), hand.Probability));
                }
                Console.WriteLine();
            }
        }

        //private void MainWindowViewModel__cardRecognized(object sender, CardRecognitionEventArgs e)
        //{
        //    DeskCards.Add(e.Card);
        //}

        //public void Add(object parameter)
        //{
        //    DeskCards.Add(new Card(CardFigure._2, CardColor.club));
        //}

        //public void Remove(object parameter)
        //{
        //    ClearList();
        //}

        public bool CanSelect(object parameter)
        {
            return true;
        }

        public void HideWindow()
        {
            ((App)Application.Current).HideWindow();
        }

        public void ShowWindow()
        {
            ((App)Application.Current).ShowWindow();
        }

        //private void SelectArea(AnalyzeType at)
        //{
        //    var pageAnalyze = new ScreenAnalyzePage(this, SettingsViewModel, at);
        //    pageAnalyze.Closed += PageAnalyze_Closed;
        //    ((App)Application.Current).HideWindow();
        //    pageAnalyze.Show();
        //}

        private void PageAnalyze_Closed(object sender, EventArgs e)
        {
            var at = (sender as ScreenAnalyzePage).AT;
            ((App)Application.Current).ShowWindow();
            CardArea area = SingleCardArea;

            if (at == AnalyzeType.Desk) area = DeskArea;
            if (at == AnalyzeType.Hand) area = HandArea;            

            //GetAreas(area, at);
        }

        private Dispatcher _getDispatcher()
        {
            return Application.Current.Dispatcher;
        }

        async Task DispatchToUiThread(Action action) => await _getDispatcher().InvokeAsync(action);

        private void ClearList()
        {
            foreach(var item in DeskCards.ToList())
            {
                DeskCards.Remove(DeskCards.First());
            }

            foreach (var item in HandCards.ToList())
            {
                HandCards.Remove(HandCards.First());
            }

            foreach (var item in Hands.ToList())
            {
                Hands.Remove(Hands.First());
            }
        }

        private void ShowSettings(object sender)
        {
            IsAreasVisible = false;
            IsMainVisible = false;
            IsSettingsVisible = true;
        }

        public void ShowMainWindow(object sender)
        {
            IsSettingsVisible = false;
            IsAreasVisible = false;
            IsMainVisible = true;
        }

        public void ShowAreas(object sender)
        {
            IsSettingsVisible = false;
            IsAreasVisible = true;
            IsMainVisible = false;
        }

        public void Analyze(object sender)
        {
            //TakeScreenShoot();
            ClearList();
            //var single = GetAreaBitmap(SingleCardArea, AnalyzeType.SingleCard);
            var desk = GetAreaBitmap(DeskArea, AnalyzeType.Desk);

            if (desk == null)
            {
                MessageBox.Show("Try again");
                return;
            }

            //var hand = GetAreaBitmap(HandArea, AnalyzeType.Hand);
            

            AnalyzeDeskV2(desk);
            //AnalyzeHand(hand);

            var matcher = new FigureMatcher();

            foreach (var card in DeskCards)
            {
                matcher.AddCardToFlop(card);
            }

            foreach (var card in HandCards)
            {
                matcher.AddCardToHand(card);
            }

            //matcher.CheckHand();
            Console.WriteLine();
        }

        private void AnalyzeDeskV2(Bitmap desk)
        {
            int offset = 0;
            var figureHeight = Convert.ToInt32(_singleCardWidth * 0.4);
            int cardsCount = _deskWidth / _singleCardWidth;            
            if (cardsCount == 5) offset = GetOffset();
            var cardWithOffset = _singleCardWidth + offset;
            
            for (int idx =0; idx<cardsCount; idx++)
            {
                var startPoint = (cardWithOffset * idx) + idx;
                
                var figure = desk.Clone(new Rectangle(startPoint, 7, _singleCardWidth/3, figureHeight), desk.PixelFormat);
                var color = desk.Clone(new Rectangle(startPoint, figureHeight + 5, _singleCardWidth / 3, figureHeight), desk.PixelFormat);
                
                figure.Save($"deskFigure{idx}.jpg");
                color.Save($"deskColor{idx}.jpg");                
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var figurePath = Path.Combine(path, $"deskFigure{idx}.jpg");
                var colorPath = Path.Combine(path, $"deskColor{idx}.jpg");
                figurePath = figurePath.Replace("\\", "/");
                colorPath = colorPath.Replace("\\", "/");
                Task.Run(() => {PredictCard(idx, figurePath, colorPath, CardTypeEnum.Desk);});
            }
        }

        private int GetOffset()
        {
            return (_deskWidth - (_singleCardWidth * 5)) / 4;
        }

        private void AnalyzeDesk(Bitmap desk)
        {
            AnalyzeDeskV2(desk);

            double exact = (double)_deskWidth / (double)_singleCardWidth;
            int count = _deskWidth / _singleCardWidth;

            if (exact < 3) count++;
            if (3.5 < exact && exact < 4) count = 4;
            if (exact > 4.5) count = 5;
            //var topName = @$"C:\Users\Mikolaj\PycharmProjects\pythonProject1\top.png";
            var top = desk.Clone(new Rectangle(0, 0, _deskWidth, 40), desk.PixelFormat);
            var _offsetCount = 4;

            if(count == 5) _deskOffset = (_deskWidth - (_singleCardWidth * count)) / _offsetCount;
            var width = _singleCardWidth + _deskOffset;
            _deskCardsCount = count;

            for (int idx = 0; idx < count; idx++)
            {
                //_analyzeCards(width, _deskWidth, idx, top, desk, CardTypeEnum.Desk);
                if ((width * idx) > _deskWidth) return;

                var card = top.Clone(new Rectangle(width * idx, 5, 30, 30), top.PixelFormat);
                card.Save(@$"C:\Users\Mikolaj\PycharmProjects\pythonProject1\allCards.png");
                var points = GetDeskPoints(idx);
                var xStart = (width * idx) + points.Item1;
                var yStart = 5;
                var figureWidth = points.Item2 - 5;
                var figureHeight = points.Item4 - points.Item3;
                var figure = top.Clone(new Rectangle(xStart, yStart, figureWidth, figureHeight), top.PixelFormat);
                var color = desk.Clone(new Rectangle(xStart, figureHeight, figureWidth, figureHeight), desk.PixelFormat);
                color.Save($"deskColor{idx}.png");
                figure.Save($"deskFigure{idx}.png");
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var figurePath = Path.Combine(path, $"deskFigure{idx}.png");
                var colorPath = Path.Combine(path, $"deskColor{idx}.png");
                figurePath = figurePath.Replace("\\", "/");
                colorPath = colorPath.Replace("\\", "/");
                _ = PredictCard(idx, figurePath, colorPath, CardTypeEnum.Desk);
            }
        }

        private void AnalyzeHand(Bitmap hand)
        {            
            
            var topName = @$"C:\Users\Mikolaj\PycharmProjects\pythonProject1\top.png";
            var top = hand.Clone(new Rectangle(0, 0, _handWidth, 40), hand.PixelFormat);
            top.Save("toppHand.png");
            var _offsetCount = 1;
            _handOffset = (_handWidth - (_singleCardWidth * 2)) / _offsetCount;
            var width = _singleCardWidth + _handOffset;

            for (int idx = 0; idx < 2; idx++)
            {
                //_analyzeCards(width, _handWidth, idx, top, hand, CardTypeEnum.Hand);
                if ((width * idx) > _handWidth) return;

                var card = top.Clone(new Rectangle(width * idx, 5, 30, 30), top.PixelFormat);
                card.Save(@$"C:\Users\Mikolaj\PycharmProjects\pythonProject1\allCards.png");
                var points = GetHandPoints(idx);
                var xStart = (width * idx) + points.Item1;
                var yStart = 5;
                var figureWidth = points.Item2 - 5;
                var figureHeight = points.Item4 - points.Item3;
                var figure = top.Clone(new Rectangle(xStart, yStart, figureWidth, figureHeight), top.PixelFormat);
                var color = hand.Clone(new Rectangle(xStart, figureHeight, figureWidth, figureHeight), hand.PixelFormat);
                color.Save($"handColor{idx}.png");
                figure.Save($"handFigure{idx}.png");
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var figurePath = Path.Combine(path, $"handFigure{idx}.png");
                var colorPath = Path.Combine(path, $"handColor{idx}.png");
                figurePath = figurePath.Replace("\\", "/");
                colorPath = colorPath.Replace("\\", "/");
                Task.Run(() => { PredictCard(idx, figurePath, colorPath, CardTypeEnum.Hand); });
            }
        }

        private void _analyzeCards(int width, int areaWidth, int idx, Bitmap top, Bitmap source, CardTypeEnum cardType)
        {
            if ((width * idx) > areaWidth) return;

            var cd = cardType == CardTypeEnum.Desk ? "desk" : "hand";
            var card = top.Clone(new Rectangle(width * idx, 5, 30, 30), top.PixelFormat);
            card.Save(@$"C:\Users\Mikolaj\PycharmProjects\pythonProject1\allCards.png");
            var points = GetDeskPoints(idx);
            var xStart = (width * idx) + points.Item1;
            var yStart = 5;
            var figureWidth = points.Item2 - 5;
            var figureHeight = points.Item4 - points.Item3;
            var figure = top.Clone(new Rectangle(xStart, yStart, figureWidth, figureHeight), top.PixelFormat);
            var color = source.Clone(new Rectangle(xStart, figureHeight, figureWidth, figureHeight), source.PixelFormat);
            color.Save($"{cd}Color{idx}.png");
            figure.Save($"{cd}Figure{idx}.png");
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var figurePath = Path.Combine(path, $"{cd}Figure{idx}.png");
            var colorPath = Path.Combine(path, $"{cd}Color{idx}.png");
            figurePath = figurePath.Replace("\\", "/");
            colorPath = colorPath.Replace("\\", "/");
            Task.Run(() => { PredictCard(idx, figurePath, colorPath, cardType); });
        }

        private Tuple<int,int,int,int> GetDeskPoints (int idx)
        {
            //if (_deskPointsList.Count() > idx) return _deskPointsList[idx];
            //_deskPointsList.Add(_cardRecognition.GetSingleCardArea());
            //return _deskPointsList[idx];
            return GetPoints(CardTypeEnum.Desk, idx);
        }

        private Tuple<int, int, int, int> GetHandPoints(int idx)
        {
            //if (_handPointsList.Count() > idx) return _handPointsList[idx];
            //_handPointsList.Add(_cardRecognition.GetSingleCardArea());
            return GetPoints(CardTypeEnum.Hand, idx);
        }

        private Tuple<int, int, int, int> GetPoints(CardTypeEnum ct, int idx)
        {
            var points = ct == CardTypeEnum.Desk ? _deskPointsList : _handPointsList;
            if (points.Count() > idx) return points[idx];
            points.Add(_cardRecognition.GetSingleCardArea());
            return points[idx];
        }

        private async Task PredictCard(int idx, string figurePath, string colorPath, CardTypeEnum ct)
        {
            var card = await GetCard(figurePath, colorPath);
            if (ct == CardTypeEnum.Desk)
            {
                DispatchToUiThread(() => { DeskCards.Add(card); });
            }
            else
            {
                DispatchToUiThread(() => { HandCards.Add(card); });
            }
            File.Delete(figurePath);
            File.Delete(colorPath);
        }

       

        private async Task<ICard> GetCard(string figurePath, string colorPath)
        {
            return _cardRecognition.GetCard(figurePath, colorPath);
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
            int width = Convert.ToInt32(item.xEnd) - Convert.ToInt32(item.xStart);// GetWidth(at);
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
                    var cutOffLocalName = @$"{name}.png";
                    g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);
                    var cardHeight = Convert.ToInt32(item.yEnd) - Convert.ToInt32(item.yStart);
                    Bitmap basicPicture = bmp.Clone(new Rectangle((int)item.xStart, (int)item.yStart, width, cardHeight), bmp.PixelFormat);
                    basicPicture = GetRepaintedFromGreen(basicPicture, Color.Black);
                    if (at == AnalyzeType.Hand) basicPicture = GetRepaintedHand(basicPicture, Color.Black);
                    basicPicture.Save(cardsAreaName);
                    points = GetPoints(at);
                    cutOffWidth = points.Item2 - points.Item1;// - points.Item1;
                    cutOffHeight = points.Item4 - points.Item3 - points.Item3;

                    if (at == AnalyzeType.SingleCard) _singleCardWidth = cutOffWidth;
                    if (at == AnalyzeType.Desk) _deskWidth = cutOffWidth;
                    if (at == AnalyzeType.Hand) _handWidth = cutOffWidth;

                    var height = points.Item4 - points.Item3 - points.Item3;
                    var xStart = points.Item1 - 2;
                    var yStart = points.Item3;

                    //if (at == AnalyzeType.Hand) height = 60;

                    try
                    {
                        Bitmap cutOffImg = basicPicture.Clone(
                            new Rectangle(
                            xStart,
                            yStart,
                            cutOffWidth,
                            50), basicPicture.PixelFormat);
                        cutOffImg.Save(cutOffName);
                        cutOffImg.Save(cutOffLocalName);
                        return cutOffImg;
                    }
                    catch(Exception x)
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
                    if(y >= 0 && y < 5)
                    {
                        basicPicture.SetPixel(x, y, color);
                    }

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
                    if (c.GetBrightness() < 0.02)
                    {
                        basicPicture.SetPixel(x, y, color);
                    }
                }
            }

            return basicPicture;
        }

        private Bitmap GetRepaintedHand(Bitmap basicPicture, Color color)
        {
            for (int x = 0; x < basicPicture.Width; x++)
            {
                for (int y = 70; y < basicPicture.Height; y++)
                {
                    basicPicture.SetPixel(x, y, color);
                }
            }

            return basicPicture;
        }

        //private Bitmap GetRepaintedFromBlack(Bitmap basicPicture, Color color)
        //{
        //    for (int x = 0; x < basicPicture.Width; x++)
        //    {
        //        for (int y = 0; y < basicPicture.Height; y++)
        //        {
        //            var c = basicPicture.GetPixel(x, y);
        //            if (c.G == 255 && c.B  == 255)
        //            {
        //                basicPicture.SetPixel(x, y, color);
        //            }
        //        }
        //    }

        //    return basicPicture;
        //}

        //private Tuple<string,string> GetCardImageName(CardArea item)
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
        //            var guid = new Guid();
        //            var cardsAreaName = @"C:\Users\Mikolaj\PycharmProjects\pythonProject1\figure.png";
        //            var figureName = @"C:\Users\Mikolaj\PycharmProjects\pythonProject1\figure.png";
        //            var colorName = @"C:\Users\Mikolaj\PycharmProjects\pythonProject1\color.png";
        //            var figureWidth = (int)(item.xEnd - item.xStart) / 2;
        //            var figureHeight = (int)(item.yEnd - item.yStart) / 2; 
        //            g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);
        //            Bitmap figure = bmp.Clone(new Rectangle((int)item.xStart, (int)item.yStart-2, 20, 25), bmp.PixelFormat);
        //            Bitmap color = bmp.Clone(new Rectangle((int)item.xStart, (int)item.yStart + 22, 20, 20), bmp.PixelFormat);
        //            //Bitmap croppedFigure = card.Clone(new Rectangle((int)item.xStart, (int)item.yStart - 2, 20, 35), bmp.PixelFormat);
        //            Bitmap resizedFigure = new Bitmap(figure, new System.Drawing.Size(30,30));
        //            Bitmap resizedColor = new Bitmap(color, new System.Drawing.Size(15, 15));
        //            //card.Save(figureName);
        //            resizedFigure.Save(figureName);
        //            resizedFigure.Save("figure.png");
        //            resizedColor.Save(colorName);
        //            return new Tuple<string, string>(figureName, colorName);
        //        }
        //    }
        //}

        public void TakeScreenShoot()
        {
            //var (xStart, yStart, xWidth, yWidth) = GetCaptureArea(pointToScreen, pointToWindow);
            //Rectangle rect = new Rectangle(xStart, yStart, xWidth, yWidth);
            //Bitmap bmp = new Bitmap(150, 110);
            //Graphics g = Graphics.FromImage(bmp);
            //g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
            //bmp.Save("image.jpg", ImageFormat.Jpeg);

            _figureMatcher.AddCardToFlop(CardFigure._King, CardColor.club);
            _figureMatcher.AddCardToFlop(CardFigure._5, CardColor.club);
            _figureMatcher.AddCardToFlop(CardFigure._2, CardColor.heart);
            _figureMatcher.AddCardToFlop(CardFigure._2, CardColor.diamond);
            //_figureMatcher.AddCardToFlop(CardFigure._7, CardColor.spade);

            _figureMatcher.AddCardToHand(CardFigure._King, CardColor.heart);
            _figureMatcher.AddCardToHand(CardFigure._9, CardColor.club);
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image.jpg");
           // var card = manager.GetCardByImage(path);// C:\\Users\\mkosi\\PycharmProjects\\tensorEnv\\dataset\\2C\\test.jpg");

            _figureMatcher.CheckHand();

            var cr = new CardRecognition();
            //var res = cr.GetCard();


            foreach (var key in _figureMatcher.PokerHandsDict.Keys)
            {
                var hand = _figureMatcher.PokerHandsDict[key];
                Hands.Add(new HandDescription(key.ToString(), hand.Probability));
            }

            Console.WriteLine();
        }

        //private (int xStart, int yStart, int xWidth, int yWidth) GetCaptureArea(System.Windows.Point pointToScreen, System.Windows.Point pointToWindow)
        //{
        //    return ((int)pointToScreen.X, (int)pointToScreen.Y, 100, 100);
        //}
    }
}
