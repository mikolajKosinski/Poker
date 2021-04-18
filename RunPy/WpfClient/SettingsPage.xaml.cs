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

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : UserControl
    {
        ISettingsWindowViewModel _vm;

        public SettingsPage()
        {

        }

        public SettingsPage(ISettingsWindowViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
        }
    }
}
