using CoreBusinessLogic;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using WpfClient.Interfaces;
using static CoreBusinessLogic.Enums;

namespace WpfClient.ViewModels
{    
    public class MainWindowViewModel : IMainWindoViewModel, INotifyPropertyChanged
    {
        private enum CardTypeEnum
        {
            Desk = 0,
            Hand = 1
        }

        public IList<string> CountingSystemsList { get; set; }

        IFigureMatcher _matcher;
        private ILoggerWrapper logger;
        int currentTotal;
        int alreadyCounted;
        #region properties

        private string _progressInfo;
        public string ProgressInfo
        {
            get
            {
                return _progressInfo;
            }
            set
            {
                _progressInfo = value;
                NotifyPropertyChanged(nameof(ProgressInfo));
            }
        }

        private Visibility _progressBarVisibility;
        public Visibility ProgressBarVisibility
        {
            get
            {
                return _progressBarVisibility;
            }
            set
            {
                _progressBarVisibility = value;
                NotifyPropertyChanged(nameof(ProgressBarVisibility));
            }
        }

        private decimal _progressBarValue;
        public decimal ProgressBarValue
        {
            get
            {
                return _progressBarValue;
            }
            set
            {
                _progressBarValue = value;
                NotifyPropertyChanged(nameof(ProgressBarValue));
            }
        }

        private bool _isFlushEnable;
        public bool IsFlushEnable
        {
            get
            {
                return _isFlushEnable;
            }
            set
            {
                _isFlushEnable = value;
                NotifyPropertyChanged(nameof(IsFlushEnable));
            }
        }

        private bool _isFourEnable;
        public bool IsFourEnable
        {
            get
            {
                return _isFourEnable;
            }
            set
            {
                _isFourEnable = value;
                NotifyPropertyChanged(nameof(IsFourEnable));
            }
        }

        private bool _isFullEnable;
        public bool IsFullEnable
        {
            get
            {
                return _isFullEnable;
            }
            set
            {
                _isFullEnable = value;
                NotifyPropertyChanged(nameof(IsFullEnable));
            }
        }

        private bool _isPairEnable;
        public bool IsPairEnable
        {
            get
            {
                return _isPairEnable;
            }
            set
            {
                _isPairEnable = value;
                NotifyPropertyChanged(nameof(IsPairEnable));
            }
        }

        private bool _isRoyalFlushEnable;
        public bool IsRoyalFlushEnable
        {
            get
            {
                return _isRoyalFlushEnable;
            }
            set
            {
                _isRoyalFlushEnable = value;
                NotifyPropertyChanged(nameof(IsRoyalFlushEnable));
            }
        }

        private bool _isStraightEnable;
        public bool IsStraightEnable
        {
            get
            {
                return _isStraightEnable;
            }
            set
            {
                _isStraightEnable = value;
                NotifyPropertyChanged(nameof(IsStraightEnable));
            }
        }

        private bool _isStraightFlushEnable;
        public bool IsStraightFlushEnable
        {
            get
            {
                return _isStraightFlushEnable;
            }
            set
            {
                _isStraightFlushEnable = value;
                NotifyPropertyChanged(nameof(IsStraightFlushEnable));
            }
        }

        private bool _isThreeEnable;
        public bool IsThreeEnable
        {
            get
            {
                return _isThreeEnable;
            }
            set
            {
                _isThreeEnable = value;
                NotifyPropertyChanged(nameof(IsThreeEnable));
            }
        }

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

        private bool _isHandCheckEnable;
        public bool IsHandCheckEnable
        {
            get
            {
                return _isHandCheckEnable;
            }
            set
            {
                _isHandCheckEnable = value;
                NotifyPropertyChanged(nameof(IsHandCheckEnable));
            }
        }

        private bool _isDeskCheckEnable;
        public bool IsDeskCheckEnable
        {
            get
            {
                return _isDeskCheckEnable;
            }
            set
            {
                _isDeskCheckEnable = value;
                NotifyPropertyChanged(nameof(IsDeskCheckEnable));
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

        private ObservableCollection<string> _messageBoard;
        public ObservableCollection<string> MessageBoard
        {
            get
            {
                return _messageBoard;
            }
            set
            {
                _messageBoard = value;
                NotifyPropertyChanged(nameof(MessageBoard));
            }
        }

        private ObservableCollection<string> _checkList;
        public ObservableCollection<string> CheckList
        {
            get
            {
                return _checkList;
            }
            set
            {
                _checkList = value;
                NotifyPropertyChanged(nameof(CheckList));
            }
        }

        private ObservableCollection<string> _cardsOnHand;
        public ObservableCollection<string> CardsOnHand
        {
            get
            {
                return _cardsOnHand;
            }
            set
            {
                _cardsOnHand = value;
                NotifyPropertyChanged(nameof(CardsOnHand));
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
                //_deskCards = value;
                Application.Current.Dispatcher.Invoke(() => _deskCards = value);
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
        #endregion

        public IAreasWindowViewModel AreasViewModel { get; set; }
        public ISettingsViewModel SettingsViewModel { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<ICard> CardRecognized;
       
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
        //public CardArea HandArea { get; set; }
        //public CardArea DeskArea { get; set; }
        public CardArea SingleCardArea { get; set; }

        #region commands
        public ICommand CleanCommand { get; set; }
        public ICommand AnalyzeCommand { get; set; }
        public ICommand AnalyzeHandCommand { get; set; }
        public ICommand AnalyzeDeskCommand { get; set; }
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

        #endregion

        

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
            logger.Info($"Selecting tab");
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

            logger.Info($"{idx} tab selected");
        }
               

        private void GetOutsList(Enums.PokerHands hand)
        {
            NeededCardsList.Clear();
            logger.Info($"Outs list cleared");

            try
            {
                NeededCardsList = new ObservableCollection<ICard>(_matcher.PokerHandsDict[hand].OutsList);
                logger.Info($"Outs list filled");
            }
            catch(Exception x)
            {
                logger.Error(x);            
            }
        }

        private Dictionary<CardFigure, string> _figureDict;
        private Dictionary<CardColor, string> _colorDict;

        public MainWindowViewModel(ICardRecognition cardRecognition, IFigureMatcher figureMatcher, ICardManager cardManager, ISettingsViewModel settingsViewModel, ILoggerWrapper logger)
        {
            IsSettingsVisible = true;
            this.logger = logger;
            ProgressBarVisibility = Visibility.Collapsed;
            this.SettingsViewModel = settingsViewModel;
            _matcher = figureMatcher;
            _matcher.SetPokerHandsDict();
            IsHandCheckEnable = true;
            IsDeskCheckEnable = false;
            CardsOnHand = new ObservableCollection<string>();
            NeededCardsList = new ObservableCollection<ICard>();
            AreasViewModel = new AreasWindowViewModel(this, figureMatcher);
            CheckList = new ObservableCollection<string> { "Formula", "Threshold", "Desk Area", "Hand Area" };
            MessageBoard = new ObservableCollection<string>();
            this.SettingsViewModel.ThresholdChanged += SettingsViewModel_ThresholdChanged;
            this.SettingsViewModel.FormulaChanged += SettingsViewModel_FormulaChanged;
            if (!string.IsNullOrWhiteSpace(settingsViewModel.SelectedFormula)) AddToCheckList("Formula");
            if (!string.IsNullOrWhiteSpace(settingsViewModel.ThresholdValue)) AddToCheckList("Threshold");
            RecognizedCardsList = new List<ICard>();
            DeskCards = new ObservableCollection<ICard>();
            HandCards = new ObservableCollection<ICard>();
            _cardRecognition = cardRecognition;
            _cardRecognition.CardRecognised += _cardRecognition_CardRecognised;
            CleanCommand = new CustomCommand(CleanButtonCommand, CanSelect);
            AnalyzeCommand = new CustomCommand(AnalyzeButtonCommand, CanSelect);
            AnalyzeHandCommand = new CustomCommand(AnalyzeHandButtonCommand, CanSelect);
            AnalyzeDeskCommand = new CustomCommand(AnalyzeDeskButtonCommand, CanSelect);
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
            IsAreasVisible = false;

            settingsViewModel.MainWindowSelected += SettingsViewModel_MainWindowSelected;    

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
            IsFlushEnable = false;

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

        private void SettingsViewModel_MainWindowSelected()
        {
            IsSettingsVisible = false;
            IsAreasVisible = false;
            IsMainVisible = true;
            logger.Info("Show Main window");
        }

        List<ICard> cardDeskList = new List<ICard>();
        List<ICard> cardHandList = new List<ICard>();

        private void _cardRecognition_CardRecognised(string cardReco, AnalyzeArea area)
        {
            try
            {
                var split = cardReco.Split('|');
                var fcType = split[2];
                var number = Convert.ToInt32(split[3]);
                CardColor color;
                CardFigure figure;

                if (area.ToString() == AnalyzeArea.Desk.ToString())
                {
                    if (fcType == "color")
                    {
                        color = _cardRecognition.ColorDict[split[0]];
                        cardDeskList[number].Color = color;
                    }
                    else
                    {
                        figure = _cardRecognition.FigureDict[split[0]];
                        cardDeskList[number].Figure = figure;
                    }
                }
                else
                {
                    if (fcType == "color")
                    {
                        color = _cardRecognition.ColorDict[split[0]];
                        cardHandList[number].Color = color;
                    }
                    else
                    {
                        figure = _cardRecognition.FigureDict[split[0]];
                        cardHandList[number].Figure = figure;
                    }
                }
            }
            catch (Exception x)
            {
                
            }

            alreadyCounted++;
            Debug.WriteLine($"{alreadyCounted} / {currentTotal}");

            if ((area == AnalyzeArea.Hand && alreadyCounted == 4) || (area == AnalyzeArea.Desk && alreadyCounted == currentTotal))
            {
                Debug.WriteLine("########## counting");
                foreach (var card in cardDeskList)
                {
                    _matcher.PokerHandsDict[PokerHands.Full].UpdateDesk(card);

                    Application.Current.Dispatcher.BeginInvoke(new Action(() => DeskCards.Add(card)));
                }

                foreach (var card in cardHandList)
                {
                    _matcher.PokerHandsDict[PokerHands.Full].UpdateHand(card);

                    Application.Current.Dispatcher.BeginInvoke(new Action(() => HandCards.Add(card)));
                }

                Calculate();
                IsDeskCheckEnable = true;
            }
        }

        private void SettingsViewModel_FormulaChanged()
        {
            AddToCheckList("Formula");
        }

        private void SettingsViewModel_ThresholdChanged()
        {
            AddToCheckList("Threshold");
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

      

        private void PageAnalyze_Closed(object sender, EventArgs e)
        {
            var at = (sender as ScreenAnalyzePage).AT;
            ((App)Application.Current).ShowWindow();
            CardArea area = SingleCardArea;

            if (at == AnalyzeType.Desk) area = SettingsViewModel.DeskArea;
            if (at == AnalyzeType.Hand) area = SettingsViewModel.HandArea;    
        }

        private Dispatcher _getDispatcher()
        {
            return Application.Current.Dispatcher;
        }

        private void ShowSettings(object sender)
        {
            IsAreasVisible = false;
            IsMainVisible = false;
            IsSettingsVisible = true;
            IsGeneralTabVisible = false;
            IsRoyalTabVisible = false;
            IsStraightFlushTabVisible = false;
            IsFourOfKindTabVisible = false;
            IsFullTabVisible = false;
            IsFlushTabVisible = false;
            IsStraightTabVisible = false;
            IsThreeOfKindTabVisible = false;
            IsPairTabVisible = false;
            logger.Info("Show Settings");
        }

        public void ShowMainWindow(object sender)
        {
            IsSettingsVisible = false;
            IsAreasVisible = false;
            IsMainVisible = true;
            logger.Info("Show Main window");
        }

        public void ShowAreas(object sender)
        {
            IsSettingsVisible = false;
            IsAreasVisible = true;
            IsMainVisible = false;
            IsGeneralTabVisible = false;
            IsRoyalTabVisible = false;
            IsStraightFlushTabVisible = false;
            IsFourOfKindTabVisible = false;
            IsFullTabVisible = false;
            IsFlushTabVisible = false;
            IsStraightTabVisible = false;
            IsThreeOfKindTabVisible = false;
            IsPairTabVisible = false;
            logger.Info("Show Areas");
        }

        private void AnalyzeButtonCommand(object sender)
        {
            Task.Run(async () => await Analyze(AnalyzeArea.All));
            //Analyze();
        }

        private void AnalyzeHandButtonCommand(object sender)
        {            
            Task.Run(async () => await Analyze(AnalyzeArea.Hand));
            IsHandCheckEnable = false;
            IsDeskCheckEnable = true;
            //Analyze();
        }

        private void AnalyzeDeskButtonCommand(object sender)
        {
            if (cardDeskList.Any())
            {                
                DeskCards.Clear();
                ProgressBarValue= 0;
            }
         
            Task.Run(async () => await Analyze(AnalyzeArea.Desk));
            IsDeskCheckEnable = false;
            //Analyze();
        }

        private void CleanButtonCommand(object sender)
        {
            Clean();
            IsHandCheckEnable = true;
            IsDeskCheckEnable = false;
        }

        void Clean()
        {
            _matcher.Clean();
            ProgressBarValue = 0;
            Hands.Clear();
            DeskCards.Clear();
            HandCards.Clear();
            logger.Info("All info Cleaned");
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
            logger.Info("ShowGeneralTab");
        }

        private void ShowRoyalTab(object sender)
        {
            logger.Info("ShowRoyalTab");
            IsGeneralTabVisible = false;
            IsRoyalTabVisible = true;
            IsStraightFlushTabVisible = false;
            IsFourOfKindTabVisible = false;
            IsFullTabVisible = false;
            IsFlushTabVisible = false;
            IsStraightTabVisible = false;
            IsThreeOfKindTabVisible = false;
            IsPairTabVisible = false;
            GetOutsList(Enums.PokerHands.RoyalFlush);
            CardsOnHand.Clear();
            logger.Info("CardsOnHand cleared");
            var availableCards = _matcher.PokerHandsDict[Enums.PokerHands.RoyalFlush].GetCards();
            foreach (var card in availableCards)
            {
                CardsOnHand.Add($"{card.Figure} {card.Color}");
                logger.Info("Card added to hand");
            }            
        }

        private void ShowStraightFlushTab(object sender)
        {
            logger.Info("ShowStraightFlushTab");
            IsGeneralTabVisible = false;
            IsRoyalTabVisible = false;
            IsStraightFlushTabVisible = true;
            IsFourOfKindTabVisible = false;
            IsFullTabVisible = false;
            IsFlushTabVisible = false;
            IsStraightTabVisible = false;
            IsThreeOfKindTabVisible = false;
            IsPairTabVisible = false;
            GetOutsList(Enums.PokerHands.StraightFlush);
            CardsOnHand.Clear();
            var availableCards = _matcher.PokerHandsDict[Enums.PokerHands.StraightFlush].GetCards();
            foreach (var card in availableCards)
            {
                CardsOnHand.Add($"{card.Figure} {card.Color}");
                logger.Info("Card added to hand");
            }
        }

        private void ShowFourOfKindTab(object sender)
        {
            logger.Info("ShowFourOfKindTab");
            IsGeneralTabVisible = false;
            IsRoyalTabVisible = false;
            IsStraightFlushTabVisible = false;
            IsFourOfKindTabVisible = true;
            IsFullTabVisible = false;
            IsFlushTabVisible = false;
            IsStraightTabVisible = false;
            IsThreeOfKindTabVisible = false;
            IsPairTabVisible = false;
            GetOutsList(Enums.PokerHands.FourOfKind);
            CardsOnHand.Clear();
            var availableCards = _matcher.PokerHandsDict[Enums.PokerHands.FourOfKind].GetCards();
            foreach (var card in availableCards)
            {
                CardsOnHand.Add($"{card.Figure} {card.Color}");
                logger.Info("Card added to hand");
            }
        }

        private void ShowFullTab(object sender)
        {
            logger.Info("ShowFullTab");
            IsGeneralTabVisible = false;
            IsRoyalTabVisible = false;
            IsStraightFlushTabVisible = false;
            IsFourOfKindTabVisible = false;
            IsFullTabVisible = true;
            IsFlushTabVisible = false;
            IsStraightTabVisible = false;
            IsThreeOfKindTabVisible = false;
            IsPairTabVisible = false;
            GetOutsList(Enums.PokerHands.Full);
            CardsOnHand.Clear();
            var availableCards = _matcher.PokerHandsDict[Enums.PokerHands.Full].GetCards();
            foreach (var card in availableCards)
            {
                CardsOnHand.Add($"{card.Figure} {card.Color}");
                logger.Info("Card added to hand");
            }
        }

        private void ShowFlushTab(object sender)
        {
            logger.Info("ShowFlushTab");
            IsGeneralTabVisible = false;
            IsRoyalTabVisible = false;
            IsStraightFlushTabVisible = false;
            IsFourOfKindTabVisible = false;
            IsFullTabVisible = false;
            IsFlushTabVisible = true;
            IsStraightTabVisible = false;
            IsThreeOfKindTabVisible = false;
            IsPairTabVisible = false;
            GetOutsList(Enums.PokerHands.Flush);
            CardsOnHand.Clear();
            var availableCards = _matcher.PokerHandsDict[Enums.PokerHands.Flush].GetCards();
            foreach (var card in availableCards)
            {
                CardsOnHand.Add($"{card.Figure} {card.Color}");
                logger.Info("Card added to hand");
            }
        }

        private void ShowStraightTab(object sender)
        {
            logger.Info("ShowStraightTab");
            IsGeneralTabVisible = false;
            IsRoyalTabVisible = false;
            IsStraightFlushTabVisible = false;
            IsFourOfKindTabVisible = false;
            IsFullTabVisible = false;
            IsFlushTabVisible = false;
            IsStraightTabVisible = true;
            IsThreeOfKindTabVisible = false;
            IsPairTabVisible = false;
            GetOutsList(Enums.PokerHands.Straight);
            CardsOnHand.Clear();
            var availableCards = _matcher.PokerHandsDict[Enums.PokerHands.Straight].GetCards();
            foreach (var card in availableCards)
            {
                CardsOnHand.Add($"{card.Figure} {card.Color}");
                logger.Info("Card added to hand");
            }
        }

        private void ShowThreeOfKindTab(object sender)
        {
            logger.Info("ShowThreeOfKindTab");
            IsGeneralTabVisible = false;
            IsRoyalTabVisible = false;
            IsStraightFlushTabVisible = false;
            IsFourOfKindTabVisible = false;
            IsFullTabVisible = false;
            IsFlushTabVisible = false;
            IsStraightTabVisible = false;
            IsThreeOfKindTabVisible = true;
            IsPairTabVisible = false;
            GetOutsList(Enums.PokerHands.ThreeOfKind);
            CardsOnHand.Clear();
            var cardsOnHand = _matcher.PokerHandsDict[Enums.PokerHands.ThreeOfKind].GetCards();
            foreach (var card in cardsOnHand)
            {
                CardsOnHand.Add($"{card.Figure} {card.Color}");
                logger.Info("Card added to hand");
            }
        }

        private void ShowPairTab(object sender)
        {
            logger.Info("ShowPairTab");
            IsGeneralTabVisible = false;
            IsRoyalTabVisible = false;
            IsStraightFlushTabVisible = false;
            IsFourOfKindTabVisible = false;
            IsFullTabVisible = false;
            IsFlushTabVisible = false;
            IsStraightTabVisible = false;
            IsThreeOfKindTabVisible = false;
            IsPairTabVisible = true;
            GetOutsList(Enums.PokerHands.Pair);
            CardsOnHand.Clear();
            var cardsOnHand = _matcher.PokerHandsDict[Enums.PokerHands.Pair].GetCards();
            foreach (var card in cardsOnHand)
            {
                CardsOnHand.Add($"{card.Figure} {card.Color}");
                logger.Info("Card added to hand");
            }
        }
                

        public async Task Analyze(AnalyzeArea at)
        {
            alreadyCounted = 0;
            currentTotal = 2;
            cardDeskList = new List<ICard>();
            cardHandList = new List<ICard>();
            if (SettingsViewModel.HandArea == null || SettingsViewModel.DeskArea == null)
            {
                MessageBox.Show("Please to select Card and Desk areas to analyze.");
                return;
            }

            logger.Info("Analyze started");
            //Application.Current.Dispatcher.BeginInvoke(new Action(() => { ProgressBarVisibility = Visibility.Visible; }));
            int flopCount;

            if (at == AnalyzeArea.Desk || at == AnalyzeArea.All)
            {
                GetCards(SettingsViewModel.DeskArea, "allCards");
                ProgressBarValue += 10;
                flopCount = Convert.ToInt32(_cardRecognition.GetCardsCountOnDesk());
                _cardRecognition.DetectCard("C:\\Users\\mkosi\\Documents\\GitHub\\Poker\\RunPy\\WpfClient\\obj\\Debug\\net5.0-windows\\allCards.PNG");
                currentTotal = flopCount * 2;
                //Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                //{
                //    MessageBoard.Add("Scanning Flop cards");
                //}));
                var tasks = new List<Task<ICard>>();
                for (int i = 0; i < flopCount; i++)
                {
                    var colorPath = @$"C:\Users\mkosi\Documents\GitHub\Poker\RunPy\WpfClient\obj\Debug\net5.0-windows\C{i}.PNG";
                    var figurePath = @$"C:\Users\mkosi\Documents\GitHub\Poker\RunPy\WpfClient\obj\Debug\net5.0-windows\F{i}.PNG";
                    cardDeskList.Add(new Card(new CardFigure(), new CardColor()));
                    //_cardRecognition.PredictCard("sfds", )
                    _cardRecognition.GetCard(figurePath, colorPath, i, AnalyzeArea.Desk);                    
                    //Application.Current.Dispatcher.BeginInvoke(new Action(() => { ProgressBarValue += 10; }));
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => { MessageBoard.Add($"Working {i} of {flopCount} flop cards "); }));
                    File.Delete(colorPath);
                    File.Delete(figurePath);
                }
            }

            if (at == AnalyzeArea.Hand || at == AnalyzeArea.All)
            {
                GetCards(SettingsViewModel.HandArea, "allCards");
                ProgressInfo = "Scanning Hand area";
                _cardRecognition.DetectCard("C:\\Users\\mkosi\\Documents\\GitHub\\Poker\\RunPy\\WpfClient\\obj\\Debug\\net5.0-windows\\allCards.PNG");
                flopCount = Convert.ToInt32(_cardRecognition.GetCardsCountOnDesk());
                currentTotal = 4;

                for (int i = 0; i < flopCount; i++)
                {
                    var colorPath = @$"C:\Users\mkosi\Documents\GitHub\Poker\RunPy\WpfClient\obj\Debug\\net5.0-windows\C{i}.PNG";
                    var figurePath = @$"C:\Users\mkosi\Documents\GitHub\Poker\RunPy\WpfClient\obj\Debug\\net5.0-windows\F{i}.PNG";
                    cardHandList.Add(new Card(new CardFigure(), new CardColor()));
                    _cardRecognition.GetCard(figurePath, colorPath, i, AnalyzeArea.Hand);                    
                    //Application.Current.Dispatcher.BeginInvoke(new Action(() => { ProgressBarValue += 10; }));
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => { MessageBoard.Add($"Working {i} of {flopCount} hand cards "); }));
                    File.Delete(colorPath);
                    File.Delete(figurePath);
                }
            }
        }

        private void Calculate()
        {
            var ordered = _matcher.PokerHandsDict.ToDictionary(x => x.Key, x => x.Value);
            ordered = _matcher.PokerHandsDict.OrderByDescending(p => p.Value.Probability).ToDictionary(x => x.Key, x => x.Value);

            _matcher.CheckHand();
            ProgressInfo = "Counting cards";
            RoyalFlushTabName = $"Royal flush [{ordered[Enums.PokerHands.RoyalFlush].Probability}%]";
            StraightFlushTabName = $"Straight flush [{ordered[Enums.PokerHands.StraightFlush].Probability}%]";
            FlushTabName = $"Flush [{ordered[Enums.PokerHands.Flush].Probability}%]";
            FourOfKindTabName = $"Four of kind [{ordered[Enums.PokerHands.FourOfKind].Probability}%]";
            FullTabName = $"Full [{ordered[Enums.PokerHands.Full].Probability}%]";
            StraightTabName = $"Straight [{ordered[Enums.PokerHands.Straight].Probability}%]";
            ThreeOfKindTabName = $"Three of kind [{ordered[Enums.PokerHands.ThreeOfKind].Probability}%]";
            PairTabName = $"Pair [{ordered[Enums.PokerHands.Pair].Probability}%]";

            IsFlushEnable = ordered[Enums.PokerHands.Flush].Probability >= Convert.ToInt32(SettingsViewModel.ThresholdValue);
            IsFourEnable = ordered[Enums.PokerHands.FourOfKind].Probability > Convert.ToInt32(SettingsViewModel.ThresholdValue);
            IsFullEnable = ordered[Enums.PokerHands.Full].Probability > Convert.ToInt32(SettingsViewModel.ThresholdValue);
            IsPairEnable = ordered[Enums.PokerHands.Pair].Probability > Convert.ToInt32(SettingsViewModel.ThresholdValue);
            IsRoyalFlushEnable = ordered[Enums.PokerHands.RoyalFlush].Probability > Convert.ToInt32(SettingsViewModel.ThresholdValue);
            IsStraightEnable = ordered[Enums.PokerHands.Straight].Probability > Convert.ToInt32(SettingsViewModel.ThresholdValue);
            IsStraightFlushEnable = ordered[Enums.PokerHands.StraightFlush].Probability > Convert.ToInt32(SettingsViewModel.ThresholdValue);
            IsThreeEnable = ordered[Enums.PokerHands.ThreeOfKind].Probability > Convert.ToInt32(SettingsViewModel.ThresholdValue);
            ProgressBarVisibility = Visibility.Collapsed;            
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Hands.Clear();
                Hands.Add($"Royal flush [{ordered[Enums.PokerHands.RoyalFlush].Probability}%]");
                Hands.Add($"Straight flush [{ordered[Enums.PokerHands.StraightFlush].Probability}%]");
                Hands.Add($"Flush [{ordered[Enums.PokerHands.Flush].Probability}%]");
                Hands.Add($"Four of kind [{ordered[Enums.PokerHands.FourOfKind].Probability}%]");
                Hands.Add($"Full [{ordered[Enums.PokerHands.Full].Probability}%]");
                Hands.Add($"Straight [{ordered[Enums.PokerHands.Straight].Probability}%]");
                Hands.Add($"Three of kind [{ordered[Enums.PokerHands.ThreeOfKind].Probability}%]");
                Hands.Add($"Pair [{ordered[Enums.PokerHands.Pair].Probability}%]");
            }));
            ShowGeneralTab(null);
        }
               
        public void AddToCheckList(string check)
        {
            if (!CheckList.Contains(check))
                return;

            var newValue = CheckList.First(p => p == check).Replace(check, $"V {check}");
            CheckList[CheckList.IndexOf(check)] = newValue;
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private void GetCards(CardArea item, string path)
        {            
            logger.Info($"Getting cards area : {item}");
            // TODO : Height / width !!!
            double screenLeft = SystemParameters.VirtualScreenLeft;
            double screenTop = SystemParameters.VirtualScreenTop;
            double screenWidth = SystemParameters.VirtualScreenHeight;
            double screenHeight = SystemParameters.VirtualScreenWidth;
            int basicWidth = Convert.ToInt32(item.xEnd) - Convert.ToInt32(item.xStart);
            var basicHeight = Convert.ToInt32(item.yEnd) - Convert.ToInt32(item.yStart) + 10;
            Bitmap basicDesk;            

            using (Bitmap bmp = new Bitmap((int)screenWidth,
               (int)screenHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    try
                    {
                        var cardsAreaName = @$"C:\Users\mkosi\Documents\GitHub\Poker\RunPy\WpfClient\obj\Debug\net5.0-windows\{path}.png";
                        g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);
                        basicDesk = bmp.Clone(new Rectangle((int)item.xStart, (int)item.yStart, basicWidth, basicHeight), bmp.PixelFormat);

                        float val1 = (float)5;
                        float val2 = (float)10;

                        ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Gif);

                        // Create an Encoder object based on the GUID  
                        // for the Quality parameter category.  
                        System.Drawing.Imaging.Encoder myEncoder =
                            System.Drawing.Imaging.Encoder.Quality;

                        EncoderParameters myEncoderParameters = new EncoderParameters(1);
                        var myEncoderParameter = new EncoderParameter(myEncoder, 0L);
                        myEncoderParameters.Param[0] = myEncoderParameter;

                        basicDesk.SetResolution(val1, val2);
                        basicDesk.Save(@$"C:\Users\mkosi\Documents\GitHub\Poker\RunPy\WpfClient\obj\Debug\net5.0-windows\{path}.gif", jpgEncoder, myEncoderParameters);
                        basicDesk.Save(cardsAreaName);
                    }
                    catch(Exception x)
                    {
                        logger.Error(x);
                    }
                }
            }
        }
    }
}
