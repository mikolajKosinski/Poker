using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBusinessLogic
{
    public interface IHandDescription
    {
        string HandName { get; set; }
        int HandProbability { get; set; }
    }
}
