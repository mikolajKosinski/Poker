using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBusinessLogic
{
    public class Card : ICard
    {
        public Card(string name)
        {
            Name = name;
            ID = Guid.NewGuid();
        }

        public string Name { get; set; }
        public Guid ID { get; }
    }
}
