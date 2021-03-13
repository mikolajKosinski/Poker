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
        private IMainWindoViewModel _mainWindowViewModel;
        private CardArea _currentCardArea;
        private ICard _currentCard;
        public List<CardArea> AreasList { get; set; }
        public List<CardArea> ApprovedList { get; set; }

        public ScreenAnalyzePage(IMainWindoViewModel mainWindowViewModel)
        {
            InitializeComponent();
            _mainWindowViewModel = mainWindowViewModel;
            DataContext = _mainWindowViewModel;
            AreasList = new List<CardArea>();
            ApprovedList = new List<CardArea>();
            MouseDown += ScreenAnalyzePage_MouseDown;
            MouseMove += ScreenAnalyzePage_MouseMove;
            _mainWindowViewModel.CardRecognized += _mainWindowViewModel_CardRecognized;
        }

        private void _mainWindowViewModel_CardRecognized(object sender, ICard card)
        {
            var approveWindow = new CardApproveWindow(card);
            _currentCard = card;
            approveWindow.Closed += ApproveWindow_Closed;
            approveWindow.Show();
        }

        private void ApproveWindow_Closed(object sender, EventArgs e)
        {
            var cardApproved = (sender as CardApproveWindow).Approved;
            if (cardApproved)
            {
                ApprovedList.Add(_currentCardArea);
                _mainWindowViewModel.RecoList.Add(_currentCard);
            }
        }

        private void ScreenAnalyzePage_MouseMove(object sender, MouseEventArgs e)
        {

            Point canvPosToWindow = canv.TransformToAncestor(this).Transform(new Point(0, 0));

            Rectangle r = rect;
            var upperlimit = canvPosToWindow.Y + (r.Height / 2);
            var lowerlimit = canvPosToWindow.Y + canv.ActualHeight - (r.Height / 2);

            var leftlimit = canvPosToWindow.X + (r.Width / 2);
            var rightlimit = canvPosToWindow.X + canv.ActualWidth - (r.Width / 2);


            var absmouseXpos = e.GetPosition(this).X;
            var absmouseYpos = e.GetPosition(this).Y;

            if ((absmouseXpos > leftlimit && absmouseXpos < rightlimit)
                && (absmouseYpos > upperlimit && absmouseYpos < lowerlimit))
            {
                Canvas.SetLeft(r, e.GetPosition(canv).X - (r.Width / 2));
                Canvas.SetTop(r, e.GetPosition(canv).Y - (r.Height / 2));
            }
            //System.Windows.Point position = e.GetPosition(this);
            //double pX = position.X;
            //double pY = position.Y;



            //// Sets the Height/Width of the circle to the mouse coordinates.
            //rect.Width = 50;
            //rect.Height = 50;
        }

        private void ScreenAnalyzePage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Rectangle selectedRect = new Rectangle();
            //selectedRect.Fill = Brushes.Red;
            //selectedRect.Width = 20;
            //selectedRect.Height = 40;
            //selectedRect.StrokeThickness = 2;
            
            //canv.Children.Add(rect);
            //AreasList.Add(GetAreaByPoint());
            //AreasList.Clear();
            _mainWindowViewModel.Areas.Add(GetAreaByPoint());
            //Canvas.SetLeft(rect, e.GetPosition(rect).X);
            //Canvas.SetTop(rect, e.GetPosition(rect).Y);
            _mainWindowViewModel.Analyze(null);
            _mainWindowViewModel.Areas.Clear();
            ApprovedList.Add(GetAreaByPoint());
        }

        private CardArea GetAreaByPoint()
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _mainWindowViewModel.RecognizedCardsList.Add(new Card(CardFigure._10, CardColor.club));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
