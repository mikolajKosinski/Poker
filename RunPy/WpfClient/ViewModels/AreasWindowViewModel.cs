using CoreBusinessLogic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using WpfClient.Interfaces;
using static CoreBusinessLogic.Enums;

namespace WpfClient.ViewModels
{
    public class AreasWindowViewModel : IAreasWindowViewModel
    {
        IFigureMatcher figureMatcher;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ObservableCollection<ICard> HandCards
        {
            get
            {
                return _mainWindowVM.HandCards;
            }
        }

        public ObservableCollection<ICard> DeskCards
        {
            get
            {
                return _mainWindowVM.DeskCards;
            }
        }

        //private bool _isVisible;
        //public bool IsVisible
        //{
        //    get
        //    {
        //        return _isVisible;
        //    }
        //    set
        //    {
        //        _isVisible = value;
        //        NotifyPropertyChanged(nameof(IsVisible));
        //    }
        //}


        IMainWindoViewModel _mainWindowVM;
        //public CardArea HandArea { get; set; }
        //public CardArea DeskArea { get; set; }
        //public CardArea SingleCardArea { get; set; }
        public ICommand HandSelectCommand { get; set; }
        public ICommand DeskSelectCommand { get; set; }
        public ICommand SingleCardCommand { get; set; }
        public ICommand AnalyzeCommand { get; set; }
        public ICommand AnalyzeHandCommand { get; set; }
        public ICommand AnalyzeDeskCommand { get; set; }
        public ICommand NewGameCommand { get; set; }

        public AreasWindowViewModel(IMainWindoViewModel vm, IFigureMatcher figureMatcher)
        {
            _mainWindowVM = vm;
            DeskSelectCommand = new CustomCommand(SelectDesk, CanSelect);
            HandSelectCommand = new CustomCommand(SelectHand, CanSelect);
            SingleCardCommand = new CustomCommand(SelectSingleCard, CanSelect);
            AnalyzeCommand = new CustomCommand(CallAnalyzeCommand, CanSelect);
            this.figureMatcher = figureMatcher;
            //IsVisible = false;
        }

        private void CallAnalyzeCommand(object sender)
        {
            _mainWindowVM.Analyze(AnalyzeArea.All);            
        }

        private void CallAnalyzeHandCommand(object sender)
        {
            _mainWindowVM.Analyze(AnalyzeArea.All);
        }

        private void CallAnalyzeDeskCommand(object sender)
        {
            _mainWindowVM.Analyze(AnalyzeArea.Desk);
        }

        public bool CanSelect(object parameter)
        {
            return true;
        }

        public bool CanClean(object parameter)
        {
            return true;
        }

        private void SelectHand(object parameter)
        {
            SelectArea(AnalyzeType.Hand);
        }

        private void SelectDesk(object parameter)
        {
            SelectArea(AnalyzeType.Desk);
        }

        private void SelectSingleCard(object parameter)
        {
            SelectArea(AnalyzeType.SingleCard);
        }

        private void SelectArea(AnalyzeType at)
        {
            var pageAnalyze = new ScreenAnalyzePage(_mainWindowVM, this, at);
            pageAnalyze.Closed += PageAnalyze_Closed;
            _mainWindowVM.HideWindow();
            pageAnalyze.Show();
        }

        private void PageAnalyze_Closed(object sender, EventArgs e)
        {
            _mainWindowVM.ShowWindow();
        }
    }
}
