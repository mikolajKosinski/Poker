using Autofac;
using CoreBusinessLogic;
using CoreBusinessLogic.IoC;
using CoreDomain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindowVM = ServiceProvider.GetService<IMainWindoViewModel>();
            var mainWindow = new MainWindow(mainWindowVM);

            Current.MainWindow = mainWindow;
            Current.MainWindow.Show();
        }
    }
}
