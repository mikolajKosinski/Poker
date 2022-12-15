
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBusinessLogic
{
    public class Card : ICard, IEquatable<ICard>
    {
        public Card(CardFigure figure, CardColor color)
        {
            Figure = figure;
            Color = color;
            ID = Guid.NewGuid();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ICard);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public bool Equals(ICard card)
        {
            if (card == null) return false;
            return card.Color == Color && card.Figure == Figure;
        }

        public CardFigure Figure { get; set; }
        public CardColor Color { get; set; }
        public Guid ID { get; private set; }
    }
}
