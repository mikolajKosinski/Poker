using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace WpfClient.Interfaces
{
    public interface IMainWindoViewModel
    {
        void TakeScreenShoot(System.Windows.Point pointToWindow, System.Windows.Point pointToScreen);
        ICommand HandSelectCommand { get; set; }
    }
}
