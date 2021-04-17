using CoreBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBusinessLogic.Hands
{
    public class Flush : BaseHandManager, IFigureManager
    {
        public Flush(IList<ICard> hand, IList<ICard> desk) : base(hand, desk)
        {

        }

        public void Check()
        {
            var tempHand = hand.Concat(desk).ToList();
            CardColor color;

            if (_gotFlush())
            {               
                Probability = 100;
                color = tempHand.GroupBy(x => x.Color)
                          .Where(group => group.Count() >= 5)
                          .Select(group => group.Key)
                          .First();
            }
            else
            {
                Probability = (int)GetOddsPercentage(GetOuts().Count());
                color = GetDominatingColor();
            }

            CardList = tempHand.Where(x => x.Color == color).ToList();
        }

        private bool _gotFlush()
        {
            return tempHand.GroupBy(x => x.Color)
                        .Where(group => group.Count() >= 5)
                        .ToList()
                        .Any();
        }

        public IList<ICard> GetOuts()
        {
            CardsNeeded = GetNeededCardsCount();
            return GetMatchingCardsFromDeck();
        }

        private int GetNeededCardsCount()
        {
            var color = GetDominatingColor();
            var hand = tempHand.Where(p => p.Color == color).ToList();
            var count = hand.Count();
            return count >= 5 ? 0 : 5 - count;
        }
               
        private IList<ICard> GetMatchingCardsFromDeck()
        {
            var colorGroup = GetDominatingColorGroup();
            var figures = colorGroup.Select(p => p.Figure).ToList();
            var color = colorGroup[0].Color;
            var restOfCards = GetDeckExceptTempHand();
            return restOfCards.Where(p => p.Color == color && !figures.Contains(p.Figure)).ToList();
        }
    }
}
