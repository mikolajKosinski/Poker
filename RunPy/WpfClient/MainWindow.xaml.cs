using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Drawing;


namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CompositionTarget.Rendering += OnRendering;

        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            System.Windows.Point p = e.GetPosition(this);
        }

        private void OnRendering(object sender, EventArgs e)
        {
            lbl1.Content = Mouse.GetPosition(lbl1).ToString();
            lbl2.Content = Mouse.GetPosition(lbl2).ToString();
        }

        //private void Timer1_Tick(object sender, EventArgs e)
        //{
        //    Graphics g;
        //    Bitmap bmp;
        //    bmp = new Bitmap(250, 200);
        //    g = this.CreateGraphics();
        //    g = Graphics.FromImage(bmp);
        //    g.CopyFromScreen(MousePosition.X - 100, MousePosition.Y - 10,
        //        0, 0, new Size(300, 300));
        //    PictureBox1.Image = bmp;
        //}
    }
}
