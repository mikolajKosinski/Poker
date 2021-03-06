using System;
using System.Collections.Generic;
using System.Text;

namespace WpfClient
{
    public class CardArea
    {
        public double xStart { get; set; }
        public double yStart { get; set; }
        public double xEnd { get; set; }
        public double yEnd { get; set; }

        public CardArea(double xs, double ys, double xe, double ye)
        {
            xStart = xs;
            yStart = ys;
            xEnd = xe;
            yEnd = ye;
        }
    }
}
