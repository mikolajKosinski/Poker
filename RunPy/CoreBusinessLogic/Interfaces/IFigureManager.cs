using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBusinessLogic.Interfaces
{
    public interface IFigureManager
    {
        public IList<ICard> GetOuts();                
        public decimal Probability { get; set; }
        public List<ICard> CardList { get;set; }
        public List<ICard> OutsList { get; set; }
        public void Check();
        public string Name { get; }
        public IList<ICard> GetCards();
    }
}
