using System;
using System.Collections.Generic;
using System.Text;
using WpfClient.Interfaces;

namespace WpfClient.ViewModels
{
    public class SettingsViewModel : ISettingsViewModel
    {
        private IMainWindoViewModel _mainVM;

        public SettingsViewModel(IMainWindoViewModel mainVM)
        {
            _mainVM = mainVM;
        }
    }
}
