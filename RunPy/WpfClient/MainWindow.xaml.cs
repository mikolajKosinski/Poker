using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfClient.Interfaces;
using WpfClient.ViewModels;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IMainWindoViewModel mainWindowViewModel;

        public MainWindow(IMainWindoViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            DataContext = mainWindowViewModel;
            InitializeComponent();
            MouseDown += MainWindow_MouseDown;
        }
        
        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!((App)Application.Current).IsScreenCaptureMode()) return;

            var pointToWindow = Mouse.GetPosition(this);
            var pointToScreen = PointToScreen(pointToWindow);
            System.Console.WriteLine();
            //((App)Application.Current).Test2();
            //AllowsTransparency = false;
            //WindowStyle = WindowStyle.SingleBorderWindow;

            //((App)Application.Current).Test2();
            //mainWindowViewModel.TakeScreenShoot(pointToWindow, pointToScreen);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
