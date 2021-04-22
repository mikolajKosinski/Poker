using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace WpfClient.Interfaces
{
    public interface IAreasWindowViewModel
    {
        //bool IsVisible { get; set; }
        ICommand DeskSelectCommand { get; set; }
        ICommand HandSelectCommand { get; set; }
        ICommand SingleCardCommand { get; set; }
        //CardArea DeskArea { get; set; }
        //CardArea HandArea { get; set; }
        //CardArea SingleCardArea { get; set; }
    }
}
