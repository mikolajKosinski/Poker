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

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for ScreenAnalyzePage.xaml
    /// </summary>
    public partial class ScreenAnalyzePage : Window
    {
        public List<CardArea> AreasList { get; set; }

        public ScreenAnalyzePage()
        {
            InitializeComponent();
            AreasList = new List<CardArea>();
            MouseDown += ScreenAnalyzePage_MouseDown;
            MouseMove += ScreenAnalyzePage_MouseMove;
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
            AreasList.Add(GetAreaByPoint());

            //Canvas.SetLeft(rect, e.GetPosition(rect).X);
            //Canvas.SetTop(rect, e.GetPosition(rect).Y);
            this.Close();
        }

        private CardArea GetAreaByPoint()
        {
            var pointToWindow = Mouse.GetPosition(this);
            var pointToScreen = PointToScreen(pointToWindow);
            var heightHalf = rect.Height / 2;
            var widthHalf = rect.Width / 2;
            return new CardArea(
                pointToScreen.X - widthHalf, 
                pointToScreen.Y - heightHalf, 
                pointToScreen.X + widthHalf, 
                pointToScreen.Y + heightHalf);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
