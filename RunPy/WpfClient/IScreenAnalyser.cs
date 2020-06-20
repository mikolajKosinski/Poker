using System;
using System.Collections.Generic;
using System.Text;
using static CoreBusinessLogic.Enums;

namespace WpfClient
{
    public interface IScreenAnalyser
    {
        void AddCard(CardType type, Tuple<int, int> position);
    }
}
