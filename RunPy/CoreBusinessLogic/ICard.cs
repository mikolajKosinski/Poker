using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBusinessLogic
{
    public interface ICard
    {
        string Name { get; set; }
        Guid ID { get; }
    }
}
