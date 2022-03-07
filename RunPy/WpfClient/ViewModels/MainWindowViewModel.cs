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

        IFigureMatcher _matcher;

        #region properties

        private bool _isGeneralTabVisible;
        public bool IsGeneralTabVisible
        {
            get
            {
                return _isGeneralTabVisible;
            }
            set
            {
                _isGeneralTabVisible = value;
                NotifyPropertyChanged(nameof(IsGeneralTabVisible));
            }
        }

        private bool _isRoyalTabVisible;
        public bool IsRoyalTabVisible
        {
            get
            {
                return _isRoyalTabVisible;
            }
            set
            {
                _isRoyalTabVisible = value;
                NotifyPropertyChanged(nameof(IsRoyalTabVisible));
            }
        }

        private bool _isStraightFlushTabVisible;
        public bool IsStraightFlushTabVisible
        {
            get
            {
                return _isStraightFlushTabVisible;
            }
            set
            {
                _isStraightFlushTabVisible = value;
                NotifyPropertyChanged(nameof(IsStraightFlushTabVisible));
            }
        }

        private bool _isFourOfKindTabVisible;
        public bool IsFourOfKindTabVisible
        {
            get
            {
                return _isFourOfKindTabVisible;
            }
            set
            {
                _isFourOfKindTabVisible = value;
                NotifyPropertyChanged(nameof(IsFourOfKindTabVisible));
            }
        }

        private bool _isFullTabVisible;
        public bool IsFullTabVisible
        {
            get
            {
                return _isFullTabVisible;
            }
            set
            {
                _isFullTabVisible = value;
                NotifyPropertyChanged(nameof(IsFullTabVisible));
            }
        }

        private bool _isFlushTabVisible;
        public bool IsFlushTabVisible
        {
            get
            {
                return _isFlushTabVisible;
            }
            set
            {
                _isFlushTabVisible = value;
                NotifyPropertyChanged(nameof(IsFlushTabVisible));
            }
        }

        private bool _isStraightTabVisible;
        public bool IsStraightTabVisible
        {
            get
            {
                return _isStraightTabVisible;
            }
            set
            {
                _isStraightTabVisible = value;
                NotifyPropertyChanged(nameof(IsStraightTabVisible));
            }
        }

        private bool _isThreeOfKindTabVisible;
        public bool IsThreeOfKindTabVisible
        {
            get
            {
                return _isThreeOfKindTabVisible;
            }
            set
            {
                _isThreeOfKindTabVisible = value;
                NotifyPropertyChanged(nameof(IsThreeOfKindTabVisible));
            }
        }

        private bool _isPairTabVisible;
        public bool IsPairTabVisible
        {
            get
            {
                return _isPairTabVisible;
            }
            set
            {
                _isPairTabVisible = value;
                NotifyPropertyChanged(nameof(IsPairTabVisible));
            }
        }

        private string _royalFlushTabName;
        public string RoyalFlushTabName
        {
            get
            {
                return _royalFlushTabName; 
            }
            set
            {
                _royalFlushTabName = value;
                NotifyPropertyChanged(nameof(RoyalFlushTabName));
            }
        }

        private string _straightFlushTabName;
        public string StraightFlushTabName
        {
            get
            {
                return _straightFlushTabName;
            }
            set
            {
                _straightFlushTabName = value;
                NotifyPropertyChanged(nameof(StraightFlushTabName));
            }
        }

        private string _fourOfKindTabName;
        public string FourOfKindTabName
        {
            get
            {
                return _fourOfKindTabName;
            }
            set
            {
                _fourOfKindTabName = value;
                NotifyPropertyChanged(nameof(FourOfKindTabName));
            }
        }

        private string _fullTabName;
        public string FullTabName
        {
            get
            {
                return _fullTabName;
            }
            set
            {
                _fullTabName = value;
                NotifyPropertyChanged(nameof(FullTabName));
            }
        }

        private string _flushTabName;
        public string FlushTabName
        {
            get
            {
                return _flushTabName;
            }
            set
            {
                _flushTabName = value;
                NotifyPropertyChanged(nameof(FlushTabName));
            }
        }

        private string _straightTabName;
        public string StraightTabName
        {
            get
            {
                return _straightTabName;
            }
            set
            {
                _straightTabName = value;
                NotifyPropertyChanged(nameof(StraightTabName));
            }
        }

        private string _threeOfKindTabName;
        public string ThreeOfKindTabName
        {
            get
            {
                return _threeOfKindTabName;
            }
            set
            {
                _threeOfKindTabName = value;
                NotifyPropertyChanged(nameof(ThreeOfKindTabName));
            }
        }

        private string _pairTabName;
        public string PairTabName
        {
            get
            {
                return _pairTabName;
            }
            set
            {
                _pairTabName = value;
                NotifyPropertyChanged(nameof(PairTabName));
            }
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

        #endregion

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
        public ICommand GeneralTabCommand { get; set; }
        public ICommand RoyalTabCommand { get; set; }
        public ICommand StraightFlushTabCommand { get; set; }
        public ICommand FourOfKindTabCommand { get; set; }
        public ICommand FullTabCommand { get; set; }
        public ICommand FlushTabCommand { get; set; }
        public ICommand StraightTabCommand { get; set; }
        public ICommand ThreeOfKindTabCommand { get; set; }
        public ICommand PairTabCommand { get; set; }

        public ICommand AreasWindowCommand { get; set; }
        public ICommand MainWindowCommand { get; set; }

        public ICommand SettingsWindowCommand { get; set; }
        //public ICommand AddCommand { get; set; }
        //public ICommand RemoveCommand { get; set; }

        private ObservableCollection<string> _cardList;
        public ObservableCollection<string> CardList
        {
            get
            {
                return _cardList;
            }
            set
            {
                _cardList = value;
                NotifyPropertyChanged(nameof(CardList));
            }
        }

        private ObservableCollection<ICard> _neededCradsList;
        public ObservableCollection<ICard> NeededCardsList
        {
            get
            {
                return _neededCradsList;
            }
            set
            {
                _neededCradsList = value;
                NotifyPropertyChanged(nameof(NeededCardsList));
            }
        }

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

        private int GetIndexByName(string name)
        {
            if (name.Contains("RoyalFlush")) return 0;
            if (name.Contains("StraightFlush")) return 1;
            if (name.Contains("FourOfKind")) return 2;
            if (name.Contains("Full")) return 3;
            if (name.Contains("Flush")) return 4;
            if (name.Contains("Straight")) return 5;
            if (name.Contains("ThreeOfKind")) return 6;
            if (name.Contains("Pair")) return 7;

            return 0;
        }

        public void SelectTab(string name)
        {
            var idx = GetIndexByName(name);

            switch(idx)
            {
                case 0:
                    ShowRoyalTab(null);
                    break;
                case 1:
                    ShowStraightFlushTab(null);
                    break;
                case 2:
                    ShowFourOfKindTab(null);
                    break;
                case 3:
                    ShowFullTab(null);
                    break;
                case 4:
                    ShowFlushTab(null);
                    break;
                case 5:
                    ShowStraightTab(null);
                    break;
                case 6:
                    ShowThreeOfKindTab(null);
                    break;
                case 7:
                    ShowPairTab(null);
                    break;
            }

            //if (idx == 0) ShowRoyalTab(null);
            //if (idx == 1) ShowStraightFlushTab(null);
            //if (idx == 2) ShowFourOfKindTab(null);
            //if (idx == 3) ShowFullTab(null);
            //if (idx == 4) ShowFlushTab(null);
            //if (idx == 5) ShowStraightTab(null);
            //if (idx == 6) ShowThreeOfKindTab(null);
            //if (idx == 7) ShowPairTab(null);
        }
               

        private void GetCardListByHand(Enums.PokerHands hand)
        {
            CardList.Clear();
            NeededCardsList.Clear();
            
            var list = _matcher.PokerHandsDict[hand].CardList;
            foreach (var item in list)
            {
                CardList.Add($"{_figureDict[item.Figure]} {_colorDict[item.Color]}");
            }

            try
            {
                NeededCardsList = new ObservableCollection<ICard>(_matcher.PokerHandsDict[hand].OutsList);
            }
            catch
            { }
        }

        private Dictionary<CardFigure, string> _figureDict;
        private Dictionary<CardColor, string> _colorDict;

        public MainWindowViewModel(ICardRecognition cardRecognition, IFigureMatcher figureMatcher, ICardManager cardManager)
        {
            _matcher = new FigureMatcher();
            CardList = new ObservableCollection<string>();
            NeededCardsList = new ObservableCollection<ICard>();
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
            GeneralTabCommand = new CustomCommand(ShowGeneralTab, CanSelect);
            RoyalTabCommand = new CustomCommand(ShowRoyalTab, CanSelect);
            StraightFlushTabCommand = new CustomCommand(ShowStraightFlushTab, CanSelect);
            FourOfKindTabCommand = new CustomCommand(ShowFourOfKindTab, CanSelect);
            FullTabCommand = new CustomCommand(ShowFullTab, CanSelect);
            FlushTabCommand = new CustomCommand(ShowFlushTab, CanSelect);
            StraightTabCommand = new CustomCommand(ShowStraightTab, CanSelect);
            ThreeOfKindTabCommand = new CustomCommand(ShowThreeOfKindTab, CanSelect);
            PairTabCommand = new CustomCommand(ShowPairTab, CanSelect);

            AreasWindowCommand = new CustomCommand(ShowAreas, CanSelect);
            MainWindowCommand = new CustomCommand(ShowMainWindow, CanSelect);
            SettingsWindowCommand = new CustomCommand(ShowSettings, CanSelect);
            //AddCommand = new CustomCommand(Add, CanSelect);
            //RemoveCommand = new CustomCommand(Remove, CanSelect);
            //_deskCards.CollectionChanged += _deskCards_CollectionChanged;
            //_handCards.CollectionChanged += _handCards_CollectionChanged;
            IsAreasVisible = false;

            RoyalFlushTabName = $"Royal flush [0%]";
            StraightFlushTabName = $"Straight flush [0%]";
            FlushTabName = $"Flush [0%]";
            FourOfKindTabName = $"Four of kind [0%]";
            FullTabName = $"Full [0%]";
            PairTabName = $"Pair [0%]";
            StraightTabName = $"Straight [0%]";
            ThreeOfKindTabName = $"Three of kind [0%]";
            PairTabName = $"Pair [0%]";

            IsGeneralTabVisible = false;
            IsRoyalTabVisible = false;
            IsStraightFlushTabVisible = false;
            IsFourOfKindTabVisible = false;
            IsFullTabVisible = false;
            IsFlushTabVisible = false;
            IsStraightTabVisible = false;
            IsThreeOfKindTabVisible = false;
            IsPairTabVisible = false;

            _figureDict = new Dictionary<CardFigure, string>()
            { {CardFigure._2, "2" },
            {CardFigure._3, "3" },
            {CardFigure._4, "4" },
            {CardFigure._5, "5" },
            {CardFigure._6, "6" },
            {CardFigure._7, "7" },
            {CardFigure._8, "8" },
            {CardFigure._9, "9" },
            {CardFigure._10, "10" },
            {CardFigure._Jack, "Jack" },
            {CardFigure._Queen, "Queen" },
            {CardFigure._King, "King" },
            {CardFigure._As, "AS" }};

            _colorDict = new Dictionary<CardColor, string>()
            {   { CardColor.club, "club" },
                { CardColor.diamond, "diamond" },
                { CardColor.heart, "heart" },
                { CardColor.spade, "spade" }};
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

        private void ClearDeck()
        {
            foreach (var item in DeskCards.ToList())
            {
                DeskCards.Remove(DeskCards.First());
            }
        }

        private void ClearHand()
        {
            foreach (var item in HandCards.ToList())
            {
                HandCards.Remove(HandCards.First());
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

        private void ShowGeneralTab(object sender)
        {
            IsGeneralTabVisible = true;
            IsRoyalTabVisible = false;
            IsStraightFlushTabVisible = false;
            IsFourOfKindTabVisible = false;
            IsFullTabVisible = false;
            IsFlushTabVisible = false;
            IsStraightTabVisible = false;
            IsThreeOfKindTabVisible = false;
            IsPairTabVisible = false;
        }

        private void ShowRoyalTab(object sender)
        {
            IsGeneralTabVisible = false;
            IsRoyalTabVisible = true;
            IsStraightFlushTabVisible = false;
            IsFourOfKindTabVisible = false;
            IsFullTabVisible = false;
            IsFlushTabVisible = false;
            IsStraightTabVisible = false;
            IsThreeOfKindTabVisible = false;
            IsPairTabVisible = false;
            GetCardListByHand(Enums.PokerHands.RoyalFlush);
        }

        private void ShowStraightFlushTab(object sender)
        {
            IsGeneralTabVisible = false;
            IsRoyalTabVisible = false;
            IsStraightFlushTabVisible = true;
            IsFourOfKindTabVisible = false;
            IsFullTabVisible = false;
            IsFlushTabVisible = false;
            IsStraightTabVisible = false;
            IsThreeOfKindTabVisible = false;
            IsPairTabVisible = false;
            GetCardListByHand(Enums.PokerHands.StraightFlush);
        }

        private void ShowFourOfKindTab(object sender)
        {
            IsGeneralTabVisible = false;
            IsRoyalTabVisible = false;
            IsStraightFlushTabVisible = false;
            IsFourOfKindTabVisible = true;
            IsFullTabVisible = false;
            IsFlushTabVisible = false;
            IsStraightTabVisible = false;
            IsThreeOfKindTabVisible = false;
            IsPairTabVisible = false;
            GetCardListByHand(Enums.PokerHands.FourOfKind);
        }

        private void ShowFullTab(object sender)
        {
            IsGeneralTabVisible = false;
            IsRoyalTabVisible = false;
            IsStraightFlushTabVisible = false;
            IsFourOfKindTabVisible = false;
            IsFullTabVisible = true;
            IsFlushTabVisible = false;
            IsStraightTabVisible = false;
            IsThreeOfKindTabVisible = false;
            IsPairTabVisible = false;
            GetCardListByHand(Enums.PokerHands.Full);
        }

        private void ShowFlushTab(object sender)
        {
            IsGeneralTabVisible = false;
            IsRoyalTabVisible = false;
            IsStraightFlushTabVisible = false;
            IsFourOfKindTabVisible = false;
            IsFullTabVisible = false;
            IsFlushTabVisible = true;
            IsStraightTabVisible = false;
            IsThreeOfKindTabVisible = false;
            IsPairTabVisible = false;
            GetCardListByHand(Enums.PokerHands.Flush);
        }

        private void ShowStraightTab(object sender)
        {
            IsGeneralTabVisible = false;
            IsRoyalTabVisible = false;
            IsStraightFlushTabVisible = false;
            IsFourOfKindTabVisible = false;
            IsFullTabVisible = false;
            IsFlushTabVisible = false;
            IsStraightTabVisible = true;
            IsThreeOfKindTabVisible = false;
            IsPairTabVisible = false;
            GetCardListByHand(Enums.PokerHands.Straight);
        }

        private void ShowThreeOfKindTab(object sender)
        {
            IsGeneralTabVisible = false;
            IsRoyalTabVisible = false;
            IsStraightFlushTabVisible = false;
            IsFourOfKindTabVisible = false;
            IsFullTabVisible = false;
            IsFlushTabVisible = false;
            IsStraightTabVisible = false;
            IsThreeOfKindTabVisible = true;
            IsPairTabVisible = false;
            GetCardListByHand(Enums.PokerHands.ThreeOfKind);
        }

        private void ShowPairTab(object sender)
        {
            IsGeneralTabVisible = false;
            IsRoyalTabVisible = false;
            IsStraightFlushTabVisible = false;
            IsFourOfKindTabVisible = false;
            IsFullTabVisible = false;
            IsFlushTabVisible = false;
            IsStraightTabVisible = false;
            IsThreeOfKindTabVisible = false;
            IsPairTabVisible = true;
            GetCardListByHand(Enums.PokerHands.Pair);
        }

        public async Task Analyze()
        {
            //TakeScreenShoot();
            GetDesk(DeskArea);
            //var single = GetAreaBitmap(SingleCardArea, AnalyzeType.SingleCard);
            var desk = GetAreaBitmap(DeskArea, AnalyzeType.Desk);
           
           // var hand = GetAreaBitmap(HandArea, AnalyzeType.Hand);

            if (desk == null)
            {
                MessageBox.Show("Try again");
                return;
            }

            //var hand = GetAreaBitmap(HandArea, AnalyzeType.Hand);

            _matcher.Clean();
            //await AnalyzeDeskV2(desk);
            //await AnalyzeHandV2(hand);

            foreach (var card in DeskCards)
            {
                _matcher.AddCardToFlop(card);
            }

            foreach (var card in HandCards)
            {
                _matcher.AddCardToHand(card);
            }

            _matcher.CheckHand();

            var ordered = _matcher.PokerHandsDict.ToDictionary(x => x.Key, x => x.Value);
            ordered = _matcher.PokerHandsDict.OrderByDescending(p => p.Value.Probability).ToDictionary(x => x.Key, x => x.Value);

            RoyalFlushTabName = $"Royal flush [{ordered[Enums.PokerHands.RoyalFlush].Probability}%]";
            StraightFlushTabName = $"Straight flush [{ordered[Enums.PokerHands.StraightFlush].Probability}%]";
            FlushTabName = $"Flush [{ordered[Enums.PokerHands.Flush].Probability}%]";
            FourOfKindTabName = $"Four of kind [{ordered[Enums.PokerHands.FourOfKind].Probability}%]";
            FullTabName = $"Full [{ordered[Enums.PokerHands.Full].Probability}%]";
            PairTabName = $"Pair [{ordered[Enums.PokerHands.Pair].Probability}%]";
            StraightTabName = $"Straight [{ordered[Enums.PokerHands.Straight].Probability}%]";
            ThreeOfKindTabName = $"Three of kind [{ordered[Enums.PokerHands.ThreeOfKind].Probability}%]";
            PairTabName = $"Pair [{ordered[Enums.PokerHands.Pair].Probability}%]";

            foreach (var item in ordered.Keys)
            {
                var cards = "";
                _matcher.PokerHandsDict[item].CardList.ForEach(p => cards += $" [{_figureDict[p.Figure]} {_colorDict[p.Color]}]");
                Hands.Add($"{_matcher.PokerHandsDict[item].Name} : {_matcher.PokerHandsDict[item].Probability}% : {cards}");
            }
            ShowGeneralTab(null);
        }

        private async Task AnalyzeDeskV2(Bitmap desk)
        {
            float offset = (float)_singleCardWidth / (float)9.5;
            var figureHeight = Convert.ToInt32(_singleCardWidth * 0.4);
            float cardsCount = (float)_deskWidth / (float)_singleCardWidth;
            var startPoint = 0;

            if (cardsCount > 3 && cardsCount < 3.5)
            {
                startPoint = 0;
                cardsCount = 3;
            }
            if (cardsCount > 4 && cardsCount < 4.5)
            {
                startPoint = 3;
                cardsCount = 4;
            }

            if (cardsCount > 5)
            {
                startPoint = 4;
                cardsCount = 5;
            }

            //if (((double)_deskWidth / (double)_singleCardWidth) > cardsCount)
            //{
            //    if(cardsCount < 5) cardsCount++;
            //}

            //if (cardsCount == 5) offset = GetOffset();
            float cardWithOffset = (float)_singleCardWidth + offset;
            var figureList = new List<string>();
            var colorList = new List<string>();
            float figureWidth = (float)_singleCardWidth / (float)3.26;
                    

            for (int idx = startPoint; idx<cardsCount; idx++)
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
                var color = hand.Clone(new RectangleF(xStartPoint, figureHeight, figureWidth * (float)0.65, figureHeight * (float)0.65), hand.PixelFormat);
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                //color = Resharp(color);
                figure.Save($"handFigure{idx}.jpg");
                color.Save($"handColor{idx}.jpg");
                var figurePath = Path.Combine(path, $"handFigure{idx}.jpg");
                var colorPath = Path.Combine(path, $"handColor{idx}.jpg");
                figurePath = figurePath.Replace("\\", "/");
                colorPath = colorPath.Replace("\\", "/");
                await PredictCard(idx, figurePath, colorPath, CardTypeEnum.Hand); 
            }
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
            foreach (var item in Hands.ToList())
            {
                try
                {
                    Hands.Remove(Hands.First());
                }
                catch(Exception x)
                {

                }
            }

            var card = await GetCard(figurePath, colorPath);
            if (ct == CardTypeEnum.Desk)
            {
                if (DeskCards.Count == 5)
                {
                    ClearDeck();
                }
                await DispatchToUiThread(() => { DeskCards.Add(card); });
            }
            else
            {
                if (HandCards.Count() != 2)
                {
                    await DispatchToUiThread(() => { HandCards.Add(card); });
                }
            }
            File.Delete(figurePath);
            File.Delete(colorPath);
        }

       

        private async Task<ICard> GetCard(string figurePath, string colorPath)
        {
            return _cardRecognition.GetCard(figurePath, colorPath);
        }


        private void GetDesk(CardArea item)
        {
            double screenLeft = SystemParameters.VirtualScreenLeft;
            double screenTop = SystemParameters.VirtualScreenTop;
            double screenWidth = SystemParameters.VirtualScreenWidth;
            double screenHeight = SystemParameters.VirtualScreenHeight;
            int basicWidth = Convert.ToInt32(item.xEnd) - Convert.ToInt32(item.xStart);
            var basicHeight = Convert.ToInt32(item.yEnd) - Convert.ToInt32(item.yStart) + 10;
            float cardWidth = 0;
            float halfCardWidth = 0;
            double quarterCard = 0;
            Bitmap basicDesk;            

            using (Bitmap bmp = new Bitmap((int)screenWidth,
               (int)screenHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    var cardsAreaName = @$"C:\Users\Mikolaj\PycharmProjects\pythonProject1\allCards.png";
                    g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);
                    basicDesk = bmp.Clone(new Rectangle((int)item.xStart, (int)item.yStart, basicWidth, basicHeight), bmp.PixelFormat);
                    basicDesk.Save(cardsAreaName);
                }
            }

            var points = _cardRecognition.GetDesk();
            points.Sort();
            //points.Reverse();
            float ratio = (float)basicDesk.Width / (float)416;

            if (points.Count == 3)
            {
                cardWidth = getCardWIdth(points, basicDesk, ratio);
                halfCardWidth = (float)cardWidth;
            }

            foreach (var point in points)
            {
                var middle = Convert.ToDouble(point.Item1) * basicDesk.Width;
                float middleX = (float)points[0].Item2;
                float height = (float)basicDesk.Height * ratio * middleX;
                float startX = (float)middle - 30;
                var card = basicDesk.Clone(new RectangleF(startX, height * (float)0.1, cardWidth, height), basicDesk.PixelFormat);
                //cutOff = GetRepaintedFromGreen(cutOff, Color.White);
                card = GetColorFigureArea(card);
                card.Save("ccutOff.jpg");
                var figure = card.Clone(new RectangleF(0, 0, card.Width, card.Height/2), basicDesk.PixelFormat);
                var color = card.Clone(new RectangleF(0, card.Height / 2, card.Width, card.Height / 2), basicDesk.PixelFormat);
                figure.Save("figure.jpg");
                color.Save("color.jpg");
                //float figureStartY = cardWidth * (float)0.2;
                //var card = basicDesk.Clone(new RectangleF(startX, figureStartY, cardWidth, height/2), basicDesk.PixelFormat);

                //card.Save("figure.jpg");
                //var cFigure = GetCenterPoint(card);
                //cFigure.Save("cFigure.jpg");
                Console.WriteLine();
            }
        }

        private Bitmap GetColorFigureArea(Bitmap card)
        {
            float height = card.Height * (float)0.80;
            float heightCutOff = card.Height * (float)0.20;
            float leftWidth = card.Width * (float)0.02;
            float width = card.Width * (float)0.28;
            return card.Clone(new RectangleF(leftWidth, heightCutOff, width, height), card.PixelFormat);
        }

        private Bitmap GetCenterPoint(Bitmap bmp)
        {
            float startX = bmp.Width * (float)0.1;
            float width = bmp.Width * (float)0.6;
            float height = bmp.Height * (float)0.9;

            return bmp.Clone(new RectangleF(startX, 1, width, height), bmp.PixelFormat);
        }

        private float getCardWIdth(List<Tuple<decimal, decimal, decimal, decimal>> points, Bitmap basicDesk, float ratio)
        {
            var c1 = (float)points[0].Item1;
            var c2 = (float)points[1].Item1;
            var c3 = (float)points[2].Item1;
            var middle1 = c1 * (basicDesk.Width * ratio);
            var middle2 = c2 * (basicDesk.Width * ratio);
            var middle3 = c3 * (basicDesk.Width * ratio);
            float twoCardDistance = (float)middle2 - (float)middle1;
            twoCardDistance = twoCardDistance * (float)0.9;
            return twoCardDistance;
        }

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
                    //basicPicture = GetRepaintedFromGreen(basicPicture, Color.Black);
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
                        globalXstart += xStart;
                        globalXlength += cutOffWidth;
                        globalYstart += yStart;
                        globalYlength += _singleCardHeight;

                        if (at == AnalyzeType.Hand)
                        {
                            var cOff = bmp.Clone(
                                new Rectangle(
                                globalXstart,
                                globalYstart,
                                globalXlength,
                                50), basicPicture.PixelFormat);
                            cOff.Save("original" + cutOffLocalName);
                            return cOff;
                        }

                       
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
                    var argb = c.ToArgb();
                    var hue = c.GetHue();

                    if(hue > 100 && hue < 120)
                    {
                        basicPicture.SetPixel(x, y, color);
                    }

                    //if(c.R < 100)
                    //{
                    //    basicPicture.SetPixel(x, y, color);
                    //}
                    //if (c.G > 70 && c.B < 90)
                    //{
                    //    basicPicture.SetPixel(x, y, color);
                    //}
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
    }
}
