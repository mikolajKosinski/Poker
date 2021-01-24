using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBusinessLogic.Interfaces
{
    public interface IFigureManager
    {
        public IList<ICard> GetOuts();
                
        public int Probability { get; set; }
        public IList<ICard> CardList { get;set; }
        public void Check();

    }
}
