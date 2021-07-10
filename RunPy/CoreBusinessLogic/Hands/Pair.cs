using CoreBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBusinessLogic.Hands
{
    public class Pair : BaseHandManager, IFigureManager
    {
        public Pair(IList<ICard> hand, IList<ICard> desk) : base (hand, desk)
        {
            Probability = 0;
        }

        public string Name { get; } = "Pair";

        public IList<ICard> GetOuts()
        {
            var deck = GetDeckExceptTempHand();
            var matches = new List<ICard>();

            foreach(var item in tempHand)
            {
                var cards = deck.Where(p => p.Figure == item.Figure).ToList();
                matches.AddRange(cards);
            }

            return matches;
        }

        public void Check()
        {
            var tempHand = hand.Concat(desk).ToList();
            if (!CheckGroupCount(tempHand, 2)) return;
            var pair = GetGroup(tempHand, 2);
            if (pair.Any())
            {
                Probability = 100;
                CardList.Add(pair.First());
            }
        }
    }
}
