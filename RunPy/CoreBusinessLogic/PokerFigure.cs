using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBusinessLogic
{
    public class PokerFigure : IPokerFigure
    {
        public string Name { get; set; }
        public List<ICard> Cards { get; set; }
        public decimal Possibility { get; set; }

        public PokerFigure(string name, List<ICard> cards, decimal possibility)
        {
            Name = name;
            Cards = cards;
            Possibility = possibility;
        }
    }
}
