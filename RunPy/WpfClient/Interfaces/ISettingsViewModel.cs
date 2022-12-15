using System;
using System.Collections.Generic;
using System.Text;
using static WpfClient.ViewModels.SettingsViewModel;

namespace WpfClient.Interfaces
{
    public interface ISettingsViewModel
    {
        public string ThresholdValue { get; set; }
        public string SelectedFormula { get; set; }
        public event FormulaChangedHandler FormulaChanged;
        public event ThresholdHandler ThresholdChanged;
    }
}
