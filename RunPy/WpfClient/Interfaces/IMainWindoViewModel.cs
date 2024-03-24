using CoreBusinessLogic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static CoreBusinessLogic.Enums;

namespace WpfClient.Interfaces
{
    public interface IMainWindoViewModel
    {
        Task Analyze(AnalyzeArea aera, Stage stage);
        //void TakeScreenShoot();
        List<ICard> RecognizedCardsList { get; set; }
        ObservableCollection<ICard> HandCards { get; set; }
        ObservableCollection<ICard> DeskCards { get; set; }

        int SelectedIndex { get; set; }

        event EventHandler<ICard> CardRecognized;
        bool ElementAdded { get; set; }
        void ShowWindow();
        void HideWindow();
        
        CardArea SingleCardArea { get; set; }
        void AddToCheckList(string check);
        void SelectTab(string name);
    }
}
