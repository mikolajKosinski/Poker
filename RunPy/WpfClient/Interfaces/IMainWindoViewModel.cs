using System;
using System.Collections.Generic;
using System.Text;

namespace WpfClient.Interfaces
{
    public interface IMainWindoViewModel
    {
        void TakeScreenShoot(System.Windows.Point pointToWindow, System.Windows.Point pointToScreen);
    }
}
