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
        public CardArea Area { get; set; }

        public ScreenAnalyzePage()
        {
            InitializeComponent();
            MouseDown += ScreenAnalyzePage_MouseDown;
        }

        private void ScreenAnalyzePage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rect = new Rectangle();
            rect.Fill = Brushes.Sienna;
            rect.Width = 100;
            rect.Height = 100;
            rect.StrokeThickness = 2;

            Area = new CardArea(1, 1, 1, 1);
            Cnv.Children.Add(rect);

            Canvas.SetLeft(rect, e.GetPosition(Cnv).X);
            Canvas.SetTop(rect, e.GetPosition(Cnv).Y);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
