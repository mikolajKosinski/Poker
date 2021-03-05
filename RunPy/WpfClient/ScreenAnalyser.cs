using System;
using System.Collections.Generic;
using System.Text;
using static CoreBusinessLogic.Enums;

namespace WpfClient
{
    public class ScreenAnalyser : IScreenAnalyser
    {
        private CardArea _desk;
        private CardArea _hand;
        private Dictionary<CardType, Tuple<int, int>> cardsPositionDict;

        public ScreenAnalyser()
        {
            cardsPositionDict = new Dictionary<CardType, Tuple<int, int>>();
        }

        public void AddCard(CardType type, Tuple<int, int> position)
        {
            cardsPositionDict.Add(type, position);
        }
    }
}
