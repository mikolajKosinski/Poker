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
        public ICommand HandSelectCommand { get; set; }
        public ICommand DeskSelectCommand { get; set; }

        public CardArea HandArea { get; set; }
        public CardArea DeskArea { get; set; }

        public SettingsViewModel(ISettings settings)
        {
            this.settings = settings;
            settings.CountingSystemsList = new List<string>() { "algebraic", "2-4" };
            SelectedFormula = settings.SelectedFormula;
            ThresholdValue = settings.SelectedThreshold;
            CountInfoCommand = new CustomCommand(ThresholdInfoButtonCommand, CanSelect);
            ThresholdInfoCommand = new CustomCommand(CountInfoButtonCommand, CanSelect);
            DeskSelectCommand = new CustomCommand(SelectDesk, CanSelect);
            HandSelectCommand = new CustomCommand(SelectHand, CanSelect);
        }

        private void SelectDesk(object parameter)
        {
            SelectArea(AnalyzeType.Desk);
        }

        private void SelectHand(object parameter)
        {
            SelectArea(AnalyzeType.Hand);
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

        private void SelectArea(AnalyzeType at)
        {
            var pageAnalyze = new ScreenAnalyzePage(at);
            pageAnalyze.DeskAreaSelected += (s, e) => { DeskArea = e; };
            pageAnalyze.HandAreaSelected += (s, e) => { HandArea = e; };
            pageAnalyze.Closed += PageAnalyze_Closed;
            //_mainWindowVM.HideWindow();

            var check = at == AnalyzeType.Desk ? "Desk Area" : "Hand Area";
            //_mainWindowVM.AddToCheckList(check);
            pageAnalyze.Show();
        }

        private void PageAnalyze_Closed(object sender, EventArgs e)
        {
            //_mainWindowVM.ShowWindow();
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

        public delegate void AreaSelectedHandler();
        public event AreaSelectedHandler AreaSelected;

        private void OnAreaSelected()
        {
            if (AreaSelected != null)
                AreaSelected();
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
