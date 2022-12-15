using Autofac;
using CoreBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CoreBusinessLogic.Enums;

namespace CoreBusinessLogic.Hands
{
    public class Flush : BaseHandManager, IFigureManager
    {
        public Flush(IList<ICard> hand, IList<ICard> desk, ISettings settings) : base(hand, desk, settings)
        {

        }

        public void UpdateHand(ICard card) => hand.Add(card);
        public void UpdateDesk(ICard card) => desk.Add(card);

        //private List<ICard> _availableCards = new List<ICard>();
        //public IList<ICard> GetCards() => _availableCards;

        public string Name { get; } = "Flush";

        public void Check()
        {
            CardColor color;
            OutsList = GetOuts().ToList();

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
                Probability = GetProbability();
                color = GetDominatingColor();
            }

            var cardsLeft = 7 - tempHand.Count();
            if (GetNeededCardsCount() > cardsLeft)
            {
                Probability = 0;
                return;
            }
            
            if (Probability == 0 || tempHand.Count() == 7) return;

            _availableCards = tempHand.Where(x => x.Color == color).ToList();           
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
            OutsCount = GetNeededCardsCount();
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
