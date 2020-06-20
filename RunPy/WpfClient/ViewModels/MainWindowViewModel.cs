using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Input;
using WpfClient.Interfaces;

namespace WpfClient.ViewModels
{
    public class MainWindowViewModel : IMainWindoViewModel
    {
        private readonly IScreenAnalyser screenAnalyser;

        public MainWindowViewModel(IScreenAnalyser screenAnalyser)
        {
            this.screenAnalyser = screenAnalyser;            
        }

        public void TakeScreenShoot(System.Windows.Point pointToWindow, System.Windows.Point pointToScreen)
        {
            var (xStart, yStart, xWidth, yWidth) = GetCaptureArea(pointToScreen, pointToWindow);
            Rectangle rect = new System.Drawing.Rectangle(xStart, yStart, xWidth, yWidth);
            Bitmap bmp = new Bitmap(100, 100);
            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
            bmp.Save("image.jpg", ImageFormat.Jpeg);
        }

        private (int xStart, int yStart, int xWidth, int yWidth) GetCaptureArea(System.Windows.Point pointToScreen, System.Windows.Point pointToWindow)
        {
            return ((int)pointToScreen.X - 50, (int)pointToScreen.Y - 50, 100, 100);
        }
    }
}
