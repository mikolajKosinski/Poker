using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBusinessLogic
{
    public interface ICard
    {
        CardColor Color { get; set; }
        CardFigure Figure { get; set; }
        Guid ID { get; }
    }
}
