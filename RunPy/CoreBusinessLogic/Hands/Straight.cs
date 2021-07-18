using CoreBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBusinessLogic.Hands
{
    public class Straight : BaseHandManager, IFigureManager
    {
        public Straight(IList<ICard> hand, IList<ICard> desk) : base(hand, desk)
        {
        }

        public string Name { get; } = "Straight";

        public IList<ICard> GetOuts()
        {
            if (!tempHand.Any()) return new List<ICard>();
            var cardsLeft = 7 - tempHand.Count();

            OutsCount = GetNeededCardsCount();
            if (OutsCount > cardsLeft) return new List<ICard>();

            var highestCardHand = tempHand.OrderByDescending(x => x.Figure).First();
            var highestPossibleFigure = highestCardHand.Figure + 2;

            if ((int)highestPossibleFigure > 14) highestPossibleFigure = CardFigure._As;

            var lowestPossible = highestPossibleFigure - 5;
            var lowestByHand = highestCardHand.Figure - 5;
            var restFromDeck = GetDeckExceptTempHand();
            var outs = new List<ICard>();

            var availableByHighest = tempHand.Where(p => p.Figure > lowestPossible && p.Figure < highestPossibleFigure)
                .GroupBy(p => p.Figure)
                .ToList();
                        
            var cardsNeededByHighest = 5 - availableByHighest.Count();

            if (cardsNeededByHighest < cardsLeft)
            {
                for (var figure = highestPossibleFigure; figure >= lowestPossible; figure--)
                {
                    if (!tempHand.Any(x => x.Figure == figure))
                    {
                        outs.AddRange(restFromDeck.Where(p => p.Figure == figure).ToList());
                    }
                }
            }

            for (var figure = highestCardHand.Figure; figure > lowestByHand; figure--)
            {
                if (!tempHand.Any(x => x.Figure == figure))
                {
                    outs.AddRange(restFromDeck.Where(p => p.Figure == figure && !outs.Any(o => o.Color == p.Color && o.Figure == p.Figure)).ToList());
                }
            }

            return outs;
        }

        private int GetNeededCardsCount()
        {
            var highestCardHand = tempHand.OrderByDescending(x => x.Figure).First();
            var lowestCard = highestCardHand.Figure - 4;
            var elements = GetDeckExceptTempHand()
                .Where(p => p.Figure > lowestCard && p.Figure < highestCardHand.Figure)
                .GroupBy(p => p.Figure)
                .ToList();
            return elements.Count();
        }

        public void Check()
        {
            if (NotInOrder(tempHand))
            {
                Probability = (int)GetOddsPercentage(GetOuts().Count());
                var highestCardHand = tempHand.OrderByDescending(x => x.Figure).First();
                var lowestCard = highestCardHand.Figure - 5;
                var elements = tempHand.Where(p => p.Figure >= lowestCard).OrderByDescending(x => x.Figure).Take(5).ToList();
                CardList = GetWithNoRept(elements);
                Probability = (int)GetOddsPercentage(GetOuts().Count());
                OutsList = GetOuts().ToList();
                OutsCount = GetOuts().Count();
                return;
            }

            var res = tempHand.OrderByDescending(x => x.Figure).Take(5).ToList();
            CardList = GetWithNoRept(res);
            Probability = 100;
        }

        private List<ICard> GetWithNoRept(List<ICard> list)
        {
            var result = new List<ICard>();

            foreach(var item in list)
            {
                if(!result.Any(p => p.Figure == item.Figure))
                {
                    result.Add(item);
                }
            }

            return result;
        }
    }
}
