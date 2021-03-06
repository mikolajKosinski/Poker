using Autofac;
using CoreBusinessLogic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WpfClient.Interfaces;

namespace WpfClient.ViewModels
{
    public class MainWindowViewModel : IMainWindoViewModel
    {
        ICardRecognition _cardRecognition;
        IFigureMatcher _figureMatcher;
        ICardManager _cardManager;
        List<CardArea> _areas;
        public ICommand HandSelectCommand { get; set; }
        public ICommand AnalyzeCommand { get; set; }

        public MainWindowViewModel(ICardRecognition cardRecognition, IFigureMatcher figureMatcher, ICardManager cardManager)
        {
            _cardManager = cardManager;
            _cardRecognition = cardRecognition;
            _figureMatcher = figureMatcher;
            HandSelectCommand = new CustomCommand(SelectArea, CanSelect);
            AnalyzeCommand = new CustomCommand(Analyze, CanSelect);
        }

        public bool CanSelect(object parameter)
        {
            return true;
        }

        private void SelectArea(object parameter)
        {
            var pageAnalyze = new ScreenAnalyzePage();
            pageAnalyze.Closed += PageAnalyze_Closed;
            ((App)Application.Current).HideWindow();
            pageAnalyze.Show();
        }

        private void PageAnalyze_Closed(object sender, EventArgs e)
        {
            _areas = (sender as ScreenAnalyzePage).AreasList;
            ((App)Application.Current).ShowWindow();
        }

        private void Analyze(object sender)
        {
            foreach(var item in _areas)
            {
                var cardTuple = GetCardImageName(item);
                var cr = new CardRecognition();
                var res = cr.GetCard();
                Console.WriteLine();
            }
        }

        private Tuple<string,string> GetCardImageName(CardArea item)
        {
            double screenLeft = SystemParameters.VirtualScreenLeft;
            double screenTop = SystemParameters.VirtualScreenTop;
            double screenWidth = SystemParameters.VirtualScreenWidth;
            double screenHeight = SystemParameters.VirtualScreenHeight;

            using (Bitmap bmp = new Bitmap((int)screenWidth,
                (int)screenHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    var guid = new Guid();
                    var figureName = @"C:\Users\Mikolaj\PycharmProjects\pythonProject1\figure.png";
                    var colorName = @"C:\Users\Mikolaj\PycharmProjects\pythonProject1\color.png";
                    var figureWidth = (int)(item.xEnd - item.xStart) / 2;
                    var figureHeight = (int)(item.yEnd - item.yStart) / 2; 
                    g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);
                    Bitmap figure = bmp.Clone(new Rectangle((int)item.xStart, (int)item.yStart-2, 20, 25), bmp.PixelFormat);
                    Bitmap color = bmp.Clone(new Rectangle((int)item.xStart, (int)item.yStart +22, 25, 25), bmp.PixelFormat);
                    //Bitmap croppedFigure = card.Clone(new Rectangle((int)item.xStart, (int)item.yStart - 2, 20, 35), bmp.PixelFormat);
                    Bitmap resizedFigure = new Bitmap(figure, new System.Drawing.Size(30,30));
                    Bitmap resizedColor = new Bitmap(color, new System.Drawing.Size(30, 30));
                    //card.Save(figureName);
                    resizedFigure.Save(figureName);
                    resizedFigure.Save("figure.png");
                    resizedColor.Save(colorName);
                    return new Tuple<string, string>(figureName, colorName);
                }
            }
        }

        public void TakeScreenShoot(System.Windows.Point pointToWindow, System.Windows.Point pointToScreen)
        {
            //var (xStart, yStart, xWidth, yWidth) = GetCaptureArea(pointToScreen, pointToWindow);
            //Rectangle rect = new Rectangle(xStart, yStart, xWidth, yWidth);
            //Bitmap bmp = new Bitmap(150, 110);
            //Graphics g = Graphics.FromImage(bmp);
            //g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
            //bmp.Save("image.jpg", ImageFormat.Jpeg);

            _figureMatcher.AddCardToFlop(CardFigure._King, CardColor.spade);
            _figureMatcher.AddCardToFlop(CardFigure._4, CardColor.club);
            _figureMatcher.AddCardToFlop(CardFigure._10, CardColor.club);
            _figureMatcher.AddCardToFlop(CardFigure._5, CardColor.diamond);
            _figureMatcher.AddCardToFlop(CardFigure._Queen, CardColor.club);

            _figureMatcher.AddCardToHand(CardFigure._King, CardColor.club);
            _figureMatcher.AddCardToHand(CardFigure._King, CardColor.diamond);
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image.jpg");
           // var card = manager.GetCardByImage(path);// C:\\Users\\mkosi\\PycharmProjects\\tensorEnv\\dataset\\2C\\test.jpg");

            _figureMatcher.CheckHand();

            var cr = new CardRecognition();
            var res = cr.GetCard();

            Console.WriteLine();
        }

        private (int xStart, int yStart, int xWidth, int yWidth) GetCaptureArea(System.Windows.Point pointToScreen, System.Windows.Point pointToWindow)
        {
            return ((int)pointToScreen.X, (int)pointToScreen.Y, 100, 100);
        }
    }
}
