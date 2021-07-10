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
        private int _singleCardHeight;
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

        private ObservableCollection<string> _hands;
        public ObservableCollection<string> Hands
        {
            get
            {
                if (_hands == null)
                {
                    _hands = new ObservableCollection<string>();
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
            AnalyzeCommand = new CustomCommand(AnalyzeButtonCommand, CanSelect);
            AreasWindowCommand = new CustomCommand(ShowAreas, CanSelect);
            MainWindowCommand = new CustomCommand(ShowMainWindow, CanSelect);
            SettingsWindowCommand = new CustomCommand(ShowSettings, CanSelect);
            //AddCommand = new CustomCommand(Add, CanSelect);
            //RemoveCommand = new CustomCommand(Remove, CanSelect);
            //_deskCards.CollectionChanged += _deskCards_CollectionChanged;
            //_handCards.CollectionChanged += _handCards_CollectionChanged;
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
                    //Hands.Add(new HandDescription(key.ToString(), hand.Probability));
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

        private void AnalyzeButtonCommand(object sender)
        {
            Analyze();
        }

        public async Task Analyze()
        {
            //TakeScreenShoot();
            ClearList();
            var single = GetAreaBitmap(SingleCardArea, AnalyzeType.SingleCard);
            var desk = GetAreaBitmap(DeskArea, AnalyzeType.Desk);
            var hand = GetAreaBitmap(HandArea, AnalyzeType.Hand);

            if (desk == null)
            {
                MessageBox.Show("Try again");
                return;
            }

            //var hand = GetAreaBitmap(HandArea, AnalyzeType.Hand);
            

            await AnalyzeDeskV2(desk);
            await AnalyzeHandV2(hand);

            var matcher = new FigureMatcher();

            foreach (var card in DeskCards)
            {
                matcher.AddCardToFlop(card);
            }

            foreach (var card in HandCards)
            {
                matcher.AddCardToHand(card);
            }

            matcher.CheckHand();

            var ordered = matcher.PokerHandsDict.OrderByDescending(x => x.Value.Probability).ToDictionary(x => x.Key, x => x.Value);

            foreach (var item in ordered.Keys)
            {
                var cards = "";
                matcher.PokerHandsDict[item].CardList.ForEach(p => cards += $" [{p.Figure} {p.Color}]");
                Hands.Add($"{matcher.PokerHandsDict[item].Name} : {matcher.PokerHandsDict[item].Probability} : {cards}");
            }
            Console.WriteLine();
        }

        private async Task AnalyzeDeskV2(Bitmap desk)
        {
            float offset = (float)_singleCardWidth / (float)9.5;
            var figureHeight = Convert.ToInt32(_singleCardWidth * 0.4);
            float cardsCount = (float)_deskWidth / (float)_singleCardWidth;

            if (cardsCount > 3 && cardsCount < 3.5) cardsCount = 3;
            if (cardsCount > 4 && cardsCount < 4.5) cardsCount = 4;

            if (cardsCount > 5) cardsCount = 5;

            //if (((double)_deskWidth / (double)_singleCardWidth) > cardsCount)
            //{
            //    if(cardsCount < 5) cardsCount++;
            //}

            //if (cardsCount == 5) offset = GetOffset();
            float cardWithOffset = (float)_singleCardWidth + offset;
            var figureList = new List<string>();
            var colorList = new List<string>();
            float figureWidth = (float)_singleCardWidth / (float)3.26;

            for (int idx =0; idx<cardsCount; idx++)
            {
                var xStartPoint = (float)(_singleCardWidth * idx) + (idx * offset)/(float)2.2;

                var figure = desk.Clone(new RectangleF(xStartPoint, 0, figureWidth, figureHeight), desk.PixelFormat);
                var color = desk.Clone(new RectangleF(xStartPoint, figureHeight, figureWidth, figureHeight), desk.PixelFormat);
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                //color = CutOff(color);
                //figure = CutOff(figure);
                color = Resharp(color);
                figure.Save($"deskFigure{idx}.jpg");
                color.Save($"deskColor{idx}.jpg");                
                var figurePath = Path.Combine(path, $"deskFigure{idx}.jpg");
                var colorPath = Path.Combine(path, $"deskColor{idx}.jpg");
                figurePath = figurePath.Replace("\\", "/");
                colorPath = colorPath.Replace("\\", "/");
                figureList.Add(figurePath);
                colorList.Add(colorPath);
                await PredictCard(idx, figurePath, colorPath, CardTypeEnum.Desk);
            }
        }

        private int GetOffset()
        {
            return (_deskWidth - (_singleCardWidth * 5)) / 4;
        }

        private void AnalyzeDesk(Bitmap desk)
        {
            //AnalyzeDeskV2(desk);

            double exact = (double)_deskWidth / (double)_singleCardWidth;
            int count = _deskWidth / _singleCardWidth;

            if (exact < 3) count++;
            if (3.5 < exact && exact < 4) count = 4;
            if (exact > 4.5) count = 5;
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

        private async Task AnalyzeHandV2(Bitmap hand)
        {
            float offset = (float)_singleCardWidth * (float)0.075;
            var figureHeight = 25;// Convert.ToInt32(_singleCardWidth * 0.4);
            int cardsCount = 2;

            float cardWithOffset = (float)_singleCardWidth + offset;
            float figureWidth = (float)_singleCardWidth / (float)3.26;

            for (int idx = 0; idx < cardsCount; idx++)
            {
                var xStartPoint = (float)(_singleCardWidth * idx) + (idx * offset) / (float)2.2;

                var figure = hand.Clone(new RectangleF(xStartPoint, 0, figureWidth, figureHeight), hand.PixelFormat);
                var color = hand.Clone(new RectangleF(xStartPoint, figureHeight, figureWidth, figureHeight), hand.PixelFormat);
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                color = Resharp(color);
                figure.Save($"handFigure{idx}.jpg");
                color.Save($"handColor{idx}.jpg");
                var figurePath = Path.Combine(path, $"handFigure{idx}.jpg");
                var colorPath = Path.Combine(path, $"handColor{idx}.jpg");
                figurePath = figurePath.Replace("\\", "/");
                colorPath = colorPath.Replace("\\", "/");
                await PredictCard(idx, figurePath, colorPath, CardTypeEnum.Hand); 
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
            
            var figure = top.Clone(new RectangleF((float)xStart, (float)yStart, (float)figureWidth, (float)figureHeight), top.PixelFormat);
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
                await DispatchToUiThread(() => { DeskCards.Add(card); });
            }
            else
            {
                await DispatchToUiThread(() => { HandCards.Add(card); });
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
            int basicWidth = Convert.ToInt32(item.xEnd) - Convert.ToInt32(item.xStart);
            var basicHeight = Convert.ToInt32(item.yEnd) - Convert.ToInt32(item.yStart) + 10;            
            var name = GetCutOffName(at);
            var globalXstart = 0;
            var globalXlength = 0;
            var globalYstart = 0;
            var globalYlength = 0;

            using (Bitmap bmp = new Bitmap((int)screenWidth,
                (int)screenHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    var cardsAreaName = @$"C:\Users\Mikolaj\PycharmProjects\pythonProject1\allCards.png";
                    var cutOffLocalName = @$"{name}.png";
                    g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);                    
                    Bitmap basicPicture = bmp.Clone(new Rectangle((int)item.xStart, (int)item.yStart, basicWidth, basicHeight), bmp.PixelFormat);
                    globalXstart += (int)item.xStart;
                    //globalXlength += basicWidth;
                    globalYstart += (int)item.yStart;
                    //globalYlength += basicHeight;
                    basicPicture = GetRepaintedFromGreen(basicPicture, Color.Black);
                    if (at == AnalyzeType.Hand) basicPicture = GetRepaintedHand(basicPicture, Color.Black);
                    basicPicture.Save(cardsAreaName);
                    points = GetPoints(at);
                    var cutOffWidth = points.Item2 - points.Item1;
                    int xStart = 0;
                    int yStart = 0;

                    if (at == AnalyzeType.SingleCard)
                    { 
                        _singleCardWidth = cutOffWidth +2;
                        _singleCardHeight = points.Item4 - points.Item3;
                        xStart = points.Item1;
                        yStart = points.Item3;
                    }
                    if (at == AnalyzeType.Desk)
                    {
                        _deskWidth = cutOffWidth;
                        xStart = points.Item1 - 2;
                        yStart = points.Item3;
                    }
                    if (at == AnalyzeType.Hand)
                    {
                        _handWidth = cutOffWidth;
                        xStart = points.Item1;
                        yStart = points.Item3;
                    }

                        try
                    {
                        if(at == AnalyzeType.Hand)
                        {
                            var cOff = basicPicture.Clone(
                                new Rectangle(xStart,
                                yStart,
                                cutOffWidth,
                                50), basicPicture.PixelFormat);
                            cOff.Save("original" + cutOffLocalName);
                            return cOff;
                        }

                        globalXstart += xStart;
                        globalXlength += cutOffWidth;
                        globalYstart += yStart;
                        globalYlength += _singleCardHeight;
                        Bitmap cutOffImg = basicPicture.Clone(
                            new Rectangle(
                            xStart,
                            yStart,
                            cutOffWidth,
                            _singleCardHeight), basicPicture.PixelFormat);
                        Bitmap originalCutOff = bmp.Clone(
                            new Rectangle(
                                globalXstart,
                                globalYstart,
                                globalXlength,
                                globalYlength), bmp.PixelFormat);
                        originalCutOff.Save("original" + cutOffLocalName);
                        cutOffImg.Save(cutOffLocalName);
                        return originalCutOff;
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

        private Bitmap Clean(Bitmap bmp)
        {
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    var c = bmp.GetPixel(x, y);
                    if (c.R > 200 && c.G < 40 && c.B < 40)
                    {
                        bmp.SetPixel(x, y, Color.White);
                    }
                }
            }

            return bmp;
        }

        private Bitmap CutOff(Bitmap basicPicture)
        {
            var firstColumns = new List<Color>();
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < basicPicture.Height; y++)
                {
                    //var c = basicPicture.GetPixel(x, y);
                    //firstColumns.Add(c);
                }
            }
            
            for(int q = basicPicture.Width; q> basicPicture.Width - 5; q--)
            {
                var column = new List<Color>();
                for (int y = 0; y < basicPicture.Height; y++)
                {
                    //var c = basicPicture.GetPixel(q, y);
                    //basicPicture.SetPixel(q, y, Color.White);
                    //column.Add(c);
                }
            }
            var cut = basicPicture.Clone(new RectangleF(3, 0, basicPicture.Width - 3, basicPicture.Height), basicPicture.PixelFormat);
            return cut;
        }

        private Bitmap Resharp(Bitmap basicPicture)
        {
            for (int x = 0; x < basicPicture.Width; x++)
            {
                for (int y = 0; y < basicPicture.Height; y++)
                {
                    var c = basicPicture.GetPixel(x, y);
                    if (c.R < 200 && c.G < 200 && c.B < 200)
                    {
                        basicPicture.SetPixel(x, y, Color.Black);
                    }
                }
            }

            return basicPicture;
        }

        private Bitmap GetRepaintedLeftSide(Bitmap basicPicture, Color color)
        {
            for (int x = 0; x < 3; x++)
            {
                var column = new List<Color>();

                for (int y = 0; y < basicPicture.Height; y++)
                {
                    column.Add(basicPicture.GetPixel(x, y));
                    //basicPicture.SetPixel(x, y, color);
                }

                if(column.All(p => p.G < 200 && p.R < 200 && p.B < 200))
                {
                    for (int y = 0; y < basicPicture.Height; y++)
                    {
                        basicPicture.SetPixel(x, y, color);
                    }
                }
            }

            return basicPicture;
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
                //Hands.Add(new HandDescription(key.ToString(), hand.Probability));
            }

            Console.WriteLine();
        }

        //private (int xStart, int yStart, int xWidth, int yWidth) GetCaptureArea(System.Windows.Point pointToScreen, System.Windows.Point pointToWindow)
        //{
        //    return ((int)pointToScreen.X, (int)pointToScreen.Y, 100, 100);
        //}
    }
}
