using CoreBusinessLogic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfClient.Interfaces
{
    public interface IMainWindoViewModel
    {
        Task Analyze();
        //void TakeScreenShoot();
        List<ICard> RecognizedCardsList { get; set; }
        ObservableCollection<ICard> HandCards { get; set; }
        ObservableCollection<ICard> DeskCards { get; set; }

        event EventHandler<ICard> CardRecognized;
        bool ElementAdded { get; set; }
        void ShowWindow();
        void HideWindow();
        CardArea DeskArea { get; set; }
        CardArea HandArea { get; set; }
        CardArea SingleCardArea { get; set; }

        void SelectTab(string name);
    }
}
