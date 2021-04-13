using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBusinessLogic.Interfaces
{
    public interface IFigureManager
    {
        public IList<ICard> GetOuts();                
        public int Probability { get; set; }
        public List<ICard> CardList { get;set; }
        public void Check();

    }
}
