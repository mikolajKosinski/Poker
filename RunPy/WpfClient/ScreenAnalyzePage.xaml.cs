using CoreBusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfClient.Interfaces;
using WpfClient.ViewModels;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for ScreenAnalyzePage.xaml
    /// </summary>
    public partial class ScreenAnalyzePage : Window
    {
        private Point _startPoint;
        private Point _endPoint;
        private IMainWindoViewModel _mainWindowViewModel;
        private CardArea _currentCardArea;
        private ICard _currentCard;
        public Visibility DeskAreaVisibiity;
        public Visibility SingleCardAreaVisibility;
        public Visibility HandAreaVisibility;
        private bool _isDragging;
        private Point _anchorPoint;
        public AnalyzeType AT { get; set; }
        public List<CardArea> AreasList { get; set; }
        public List<CardArea> ApprovedList { get; set; }

        public ScreenAnalyzePage(AnalyzeType at)
        {
            InitializeComponent();
            AT = at;
            SetAreasVisibility();
            DataContext = _mainWindowViewModel;
            AreasList = new List<CardArea>();
            ApprovedList = new List<CardArea>();
            MouseDown += ScreenAnalyzePage_MouseDown;
            MouseUp += ScreenAnalyzePage_MouseUp;
            MouseMove += ScreenAnalyzePage_MouseMove;
            //_mainWindowViewModel.CardRecognized += _mainWindowViewModel_CardRecognized;
        }

        private void ScreenAnalyzePage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var pointToWindow = Mouse.GetPosition(this);
            _endPoint = PointToScreen(pointToWindow);
            var area = new CardArea(_startPoint.X, _startPoint.Y, _endPoint.X, _endPoint.Y);

            //if (AT == AnalyzeType.SingleCard) _mainWindowViewModel.SingleCardArea = area;
            //if (AT == AnalyzeType.Desk) _mainWindowViewModel.DeskArea = area;
            //if (AT == AnalyzeType.Hand) _mainWindowViewModel.HandArea = area;

            if (AT == AnalyzeType.Desk) OnDeskAreaSelected(area);
            if (AT == AnalyzeType.Hand) OnHandAreaSelected(area);

            this.Close();
        }

        private void SetAreasVisibility()
        {
            switch(AT)
            {
                case AnalyzeType.Desk:
                    DeskAreaVisibiity = Visibility.Visible;
                    return;
                case AnalyzeType.SingleCard:
                    SingleCardAreaVisibility = Visibility.Visible;
                    return;
                case AnalyzeType.Hand:
                    DeskAreaVisibiity = Visibility.Visible;
                    return;
            }    
        }

        public event EventHandler<CardArea> DeskAreaSelected;

        private void OnDeskAreaSelected(CardArea area)
        {
            if (DeskAreaSelected != null)
                DeskAreaSelected(null, area);
        }

        public event EventHandler<CardArea> HandAreaSelected;

        private void OnHandAreaSelected(CardArea area)
        {
            if (HandAreaSelected != null)
                HandAreaSelected(null, area);
        }

        //private void _mainWindowViewModel_CardRecognized(object sender, ICard card)
        //{
        //    var approveWindow = new CardApproveWindow(card);
        //    _currentCard = card;
        //    approveWindow.Closed += ApproveWindow_Closed;
        //    approveWindow.Show();
        //}

        //private void ApproveWindow_Closed(object sender, EventArgs e)
        //{
        //    var cardApproved = (sender as CardApproveWindow).Approved;
        //    if (cardApproved)
        //    {
        //        ApprovedList.Add(_currentCardArea);
        //        _mainWindowViewModel.DeskCards.Add(_currentCard);
        //    }
        //}

        private Rectangle GetRectangle()
        {
            if (AT == AnalyzeType.Desk) return DeskAreaRect;
            //if (AT == AnalyzeType.SingleCard) return SingleCardAreaRect;
            return HandAreaRect;
        }

        private Canvas GetCanv()
        {
            if (AT == AnalyzeType.Desk) return DeskCanv;
            //if (AT == AnalyzeType.SingleCard) return SingleCardCanv;
            return HandCanv;
        }

        private void ScreenAnalyzePage_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging) return;

            var rect = GetRectangle();
            var x = e.GetPosition(GetCanv()).X;
            var y = e.GetPosition(GetCanv()).Y;
            rect.SetValue(Canvas.LeftProperty, _anchorPoint.X);
            rect.SetValue(Canvas.TopProperty, _anchorPoint.Y);
            rect.Width = Math.Abs(x - _anchorPoint.X);
            rect.Height = Math.Abs(y - _anchorPoint.Y);

        }

        private void ScreenAnalyzePage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _anchorPoint.X = e.GetPosition(GetCanv()).X;
            _anchorPoint.Y = e.GetPosition(GetCanv()).Y;
            _isDragging = true;
            var pointToWindow = Mouse.GetPosition(this);
            _startPoint = PointToScreen(pointToWindow);
        }

        private CardArea GetDeskArea()
        {
            var pointToWindow = Mouse.GetPosition(this);
            var pointToScreen = PointToScreen(pointToWindow);
            var xStart = pointToScreen.X - 225;
            var xEnd = pointToScreen.X + 175;
            var yStart = pointToScreen.Y - 75;
            var yEnd = pointToScreen.Y + 75;
            _currentCardArea = new CardArea(
                xStart, yStart, xEnd, yEnd);
            return _currentCardArea;
        }

        private CardArea GetHandArea()
        {
            var pointToWindow = Mouse.GetPosition(this);
            var pointToScreen = PointToScreen(pointToWindow);
            var xStart = pointToScreen.X - 70;
            var xEnd = pointToScreen.X + 70;
            var yStart = pointToScreen.Y - 75;
            var yEnd = pointToScreen.Y + 75;
            _currentCardArea = new CardArea(
                xStart, yStart, xEnd, yEnd);
            return _currentCardArea;
        }

        private CardArea GetSingleCardArea()
        {
            var pointToWindow = Mouse.GetPosition(this);
            var pointToScreen = PointToScreen(pointToWindow);
            var xStart = pointToScreen.X - 50;
            var xEnd = pointToScreen.X + 50;
            var yStart = pointToScreen.Y - 75;
            var yEnd = pointToScreen.Y + 75;
            _currentCardArea = new CardArea(
                xStart, yStart, xEnd, yEnd);
            return _currentCardArea;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    _mainWindowViewModel.RecognizedCardsList.Add(new Card(CardFigure._10, CardColor.club));
        //}

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
