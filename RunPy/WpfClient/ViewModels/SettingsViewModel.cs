using Autofac;
using CoreBusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows;
using WpfClient.Interfaces;
using System.Windows.Controls;
using System.Windows.Input;
using System.Reflection.Metadata;

namespace WpfClient.ViewModels
{
    public class SettingsViewModel : ISettingsViewModel
    {
        private IContainer _container;
        private ISettings settings;
        private string threshold;

        public SettingsViewModel(ISettings settings)
        {
            this.settings = settings;
            settings.CountingSystemsList = new List<string>() { "algebraic", "2-4" };
            SelectedFormula = settings.SelectedFormula;
            ThresholdValue = settings.SelectedThreshold;
            CountInfoCommand = new CustomCommand(ThresholdInfoButtonCommand, CanSelect);
            ThresholdInfoCommand = new CustomCommand(CountInfoButtonCommand, CanSelect);
        }

        private void ThresholdInfoButtonCommand(object sender)
        {
            MessageBox.Show("Threshold");
        }

        private void CountInfoButtonCommand(object sender)
        {
            MessageBox.Show("Count");
        }

        public bool CanSelect(object parameter)
        {
            return true;
        }

        public ICommand CountInfoCommand { get; set; }
        public ICommand ThresholdInfoCommand { get; set; }

        public string SelectedFormula
        {
            get
            {
                return settings.SelectedFormula;
            }
            set
            {
                settings.SelectedFormula = value;
                OnFormulaChanged();
            }
        }

        public IList<string> CountingSystemsList
        {
            get
            {
                return settings.CountingSystemsList;
            }
        }

        public string ThresholdValue
        {
            get
            {
                return threshold;
            }
            set
            {
                threshold = value;
                OnThresholdChanged();
            }
        }

        public delegate void FormulaChangedHandler();
        public event FormulaChangedHandler FormulaChanged;
        private void OnFormulaChanged()
        {
            if (FormulaChanged != null)
                FormulaChanged();
        }

        public delegate void ThresholdHandler();
        public event ThresholdHandler ThresholdChanged;
        private void OnThresholdChanged()
        {
            if (ThresholdChanged != null)
                ThresholdChanged();
        }

        public bool GeneralSuggestions { get; set; }
        public bool HandSuggestions { get; set; }
    }
}
