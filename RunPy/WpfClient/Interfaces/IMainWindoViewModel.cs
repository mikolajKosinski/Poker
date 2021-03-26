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
        ICommand DeskSelectCommand { get; set; }
        void Analyze(object sender);
        public CardArea DeskArea { get; set; }
        public CardArea HandArea { get; set; }
        public CardArea SingleCardArea { get; set; }
        List<ICard> RecognizedCardsList { get; set; }
        ObservableCollection<ICard> HandCards { get; set; }
        ObservableCollection<ICard> DeskCards { get; set; }
        event EventHandler<ICard> CardRecognized;
        bool ElementAdded { get; set; }
    }
}
