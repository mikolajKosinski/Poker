using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBusinessLogic
{
    public class HandDescription : IHandDescription
    {
        public HandDescription(string name, int prob)
        {
            HandName = name;
            HandProbability = prob;
        }

        public string HandName { get; set; }
        public int HandProbability { get; set; }
    }
}
