using Autofac;
using CoreBusinessLogic;
using CoreBusinessLogic.Hands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using WpfClient.Interfaces;
using WpfClient.ViewModels;

namespace WpfClient.IoC
{
    public class Factory
    {
        private ContainerBuilder _builder;
        public IContainer Container;

        public Factory()
        {
            _builder = new ContainerBuilder();
            RegisterServices();
            Container = _builder.Build();
        }

        private void RegisterServices()
        {
            _builder.RegisterType<LoggerWrapper>().As<ILoggerWrapper>();
            _builder.RegisterType<FigureMatcher>().As<IFigureMatcher>();
            _builder.RegisterType<CardManager>().As<ICardManager>();
            _builder.RegisterType<CardRecognition>().As<ICardRecognition>();
            _builder.RegisterType<Settings>().As<ISettings>().SingleInstance();
            _builder.RegisterType<SettingsViewModel>().As<ISettingsViewModel>().SingleInstance();
            _builder.RegisterType<AreasWindowViewModel>().As<IAreasWindowViewModel>().SingleInstance();
        }
    }
}
