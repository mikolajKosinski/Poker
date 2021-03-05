using Autofac;
using CoreBusinessLogic;
using CoreBusinessLogic.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WpfClient.Interfaces;
using WpfClient.ViewModels;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ServiceProvider ServiceProvider { get; set; }
        private MainWindow _mainWindow;
        private IContainer _container;

        public App()
        {
            var factory = new Factory();
            _container = factory.Builder;
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMainWindoViewModel>(new MainWindowViewModel(
                _container.Resolve<ICardRecognition>(),
                _container.Resolve<IFigureMatcher>(),
                _container.Resolve<ICardManager>()));
            services.AddScoped<IScreenAnalyser, ScreenAnalyser>();
        }

        public bool IsScreenCaptureMode() => _mainWindow.WindowStyle == WindowStyle.None;

        public void HideWindow()
        {
            Current.MainWindow.Hide();
            //_mainWindow.Background = new SolidColorBrush(Colors.Blue);
            //_mainWindow.Opacity = 0.1;
            //_mainWindow.WindowStyle = WindowStyle.None;
        }

        public void ShowWindow()
        {
            Current.MainWindow.Show();
            //Current.MainWindow.Hide();
            //Current.MainWindow = null;
            //Current.MainWindow = _mainWindow;
            //Current.MainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
            //Style style = new Style(typeof(Window));
            //style.Setters.Add(new Setter(Window.AllowsTransparencyProperty, false));
            //style.Setters.Add(new Setter(Window.StyleProperty, WindowStyle.SingleBorderWindow));
            //style.Setters.Add(new Setter(Window.OpacityProperty, 1));
            //_mainWindow.Style = style;
            //_mainWindow.Background = new SolidColorBrush(Colors.White);
            //_mainWindow.Opacity = 1;
            //_mainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindowVM = ServiceProvider.GetService<IMainWindoViewModel>();
            _mainWindow = new MainWindow(mainWindowVM);

            Current.MainWindow = _mainWindow;
            Current.MainWindow.Show();
        }
    }
}
