using CoreBusinessLogic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace WpfClient.Interfaces
{
    public interface IMainWindoViewModel
    {
        void TakeScreenShoot(System.Windows.Point pointToWindow, System.Windows.Point pointToScreen);
        ICommand HandSelectCommand { get; set; }
        void Analyze(object sender);
        public List<CardArea> Areas { get; set; }
        List<ICard> RecognizedCardsList { get; set; }
        ObservableCollection<ICard> RecoList { get; set; }
        event EventHandler<ICard> CardRecognized;
        bool ElementAdded { get; set; }
    }
}
