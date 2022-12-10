﻿using Autofac;
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

        public SettingsViewModel(ISettings settings)
        {
            this.settings = settings;
            settings.CountingSystemsList = new List<string>() { "algebraic", "2-4" };
            SelectedFormula = CountingSystemsList[0];
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

        public string SliderValue
        {
            get
            {
                return sliderValue;
            }
            set
            {
                sliderValue = value;
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
