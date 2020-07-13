using System.Windows;
using System.Windows.Input;
using WpfClient.Interfaces;

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
            var pointToWindow = Mouse.GetPosition(this);
            var pointToScreen = PointToScreen(pointToWindow);
            mainWindowViewModel.TakeScreenShoot(pointToWindow, pointToScreen);
        }
    }
}
