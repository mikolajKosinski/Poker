using CoreBusinessLogic;
using CoreDomain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Input;
using WpfClient.Interfaces;

namespace WpfClient.ViewModels
{
    public class MainWindowViewModel : IMainWindoViewModel
    {
        public MainWindowViewModel()
        {
        }

        public void TakeScreenShoot(System.Windows.Point pointToWindow, System.Windows.Point pointToScreen)
        {
            var (xStart, yStart, xWidth, yWidth) = GetCaptureArea(pointToScreen, pointToWindow);
            Rectangle rect = new Rectangle(xStart, yStart, xWidth, yWidth);
            Bitmap bmp = new Bitmap(150, 110);
            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
            bmp.Save("image.jpg", ImageFormat.Jpeg);

            ICardRecognition cardReco = new CardRecognition();
            ICardManager manager = new CardManager(cardReco);
            IFigureMatcher matcher = new FigureMatcher();

            matcher.AddCardToFlop(CardFigure._4, CardColor.diamond);
            matcher.AddCardToFlop(CardFigure._4, CardColor.club);
            matcher.AddCardToFlop(CardFigure._10, CardColor.club);
            matcher.AddCardToFlop(CardFigure._10, CardColor.diamond);
            matcher.AddCardToFlop(CardFigure._Queen, CardColor.club);

            matcher.AddCardToHand(CardFigure._King, CardColor.club);
            matcher.AddCardToHand(CardFigure._King, CardColor.diamond);
            //var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image.jpg");
            //var card = manager.GetCardByImage(path);// C:\\Users\\mkosi\\PycharmProjects\\tensorEnv\\dataset\\2C\\test.jpg");

            var res = matcher.CheckHand();
            Console.WriteLine();
        }

        private (int xStart, int yStart, int xWidth, int yWidth) GetCaptureArea(System.Windows.Point pointToScreen, System.Windows.Point pointToWindow)
        {
            return ((int)pointToScreen.X, (int)pointToScreen.Y, 100, 100);
        }
    }
}
