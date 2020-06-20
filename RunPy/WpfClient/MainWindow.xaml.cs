using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Extensions.DependencyInjection;
using CoreBusinessLogic;
using WpfClient.ViewModels;
using WpfClient.Interfaces;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IMainWindoViewModel mainWindowViewModel;
        private readonly ServiceProvider serviceProvider;

        public MainWindow()
        {
            serviceProvider = (Application.Current as App).ServiceProvider;
            mainWindowViewModel = new MainWindowViewModel(serviceProvider.GetService<IScreenAnalyser>());
            this.DataContext = mainWindowViewModel;
            InitializeComponent();
            MouseDown += MainWindow_MouseDown;
        }
        
        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point pointToWindow = Mouse.GetPosition(this);
            System.Windows.Point pointToScreen = PointToScreen(pointToWindow);
            mainWindowViewModel.TakeScreenShoot(pointToWindow, pointToScreen);
        }
    }
}
