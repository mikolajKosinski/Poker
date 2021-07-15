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
        }

        private void HandsList_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listbox = (ListBox)sender;
            var name = listbox.SelectedItem.ToString();
            mainWindowViewModel.SelectTab(name);
        }
    }
}
