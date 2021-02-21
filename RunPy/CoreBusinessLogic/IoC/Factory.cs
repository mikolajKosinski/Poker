using Autofac;
using CoreBusinessLogic.Hands;
using CoreDomain;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBusinessLogic.IoC
{
    public class Factory
    {
        private ContainerBuilder _builder;
        public IContainer Builder;

        public Factory()
        {
            _builder = new ContainerBuilder();
            RegisterServices();
            Builder = _builder.Build();
        }

        private void RegisterServices()
        {
            _builder.RegisterType<FigureMatcher>().As<IFigureMatcher>();
            _builder.RegisterType<CardManager>().As<ICardManager>();
            _builder.RegisterType<CardRecognition>().As<ICardRecognition>();
        }
    }
}
