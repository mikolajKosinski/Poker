using System;
using System.Collections.Generic;
using System.Text;

namespace WpfClient
{
    public class CardArea
    {
        int xStart { get; set; }
        int yStart { get; set; }
        int xEnd { get; set; }
        int yEnd { get; set; }

        public CardArea(int xs, int ys, int xe, int ye)
        {
            xStart = xs;
            yStart = ys;
            xEnd = xe;
            yEnd = ye;
        }
    }
}
