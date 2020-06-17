using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBusinessLogic
{
    public class PokerFigure : IPokerFigure
    {
        public string Name { get; set; }

        public PokerFigure(string name)
        {
            Name = name;
        }
    }
}
