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

namespace WpfClient
{    
    public partial class CardApproveWindow : Window
    {
        ICard _card;
        public bool Approved;
        public CardApproveWindow(ICard card)
        {
            InitializeComponent();
            _card = card;
            FigureLbl.Content = _card.Figure.ToString();
            ColorLbl.Content = _card.Color.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Approved = true;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Approved = false;
            this.Close();
        }
    }
}
