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

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IMainWindoViewModel, MainWindowViewModel>();
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
