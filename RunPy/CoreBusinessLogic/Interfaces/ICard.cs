using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBusinessLogic
{
    public interface ICard
    {
        CardColor Color { get; }
        CardFigure Figure { get; }
        Guid ID { get; }
    }
}
