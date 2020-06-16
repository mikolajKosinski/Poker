using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBusinessLogic
{
    public interface ICardManager
    {
        Card GetCardByImage(string path);
    }
}
