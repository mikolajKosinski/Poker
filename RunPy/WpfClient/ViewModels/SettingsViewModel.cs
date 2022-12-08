using Autofac;
using CoreBusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows;
using WpfClient.Interfaces;
using System.Windows.Controls;

namespace WpfClient.ViewModels
{
    public class SettingsViewModel : ISettingsViewModel
    {
        private IContainer _container;
        private ISettings settings;
        private string sliderValue;

        public SettingsViewModel()
        {
            
        }

        public void Initiate(IContainer container)
        {
            this.settings = container.Resolve<ISettings>();
            settings.CountingSystemsList = new List<string>() { "algebraic", "table", "2-4" };
        }

        public string SelectedFormula
        {
            get
            {
                return settings.SelectedFormula;
            }
            set
            {
                settings.SelectedFormula = value;
            }
        }

        public IList<string> CountingSystemsList
        {
            get
            {
                return settings.CountingSystemsList;
            }
        }

        public string SliderValue
        {
            get
            {
                return sliderValue;
            }
            set
            {
                sliderValue = value;
            }
        }
    }
}
