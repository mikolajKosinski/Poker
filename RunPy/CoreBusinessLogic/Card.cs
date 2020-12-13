using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBusinessLogic
{
    public class Card : ICard
    {
        public Card(CardFigure figure, CardColor color)
        {
            Figure = figure;
            Color = color;
            ID = Guid.NewGuid();
        }

        public CardFigure Figure { get; }
        public CardColor Color { get; }
        public Guid ID { get; }
    }
}
