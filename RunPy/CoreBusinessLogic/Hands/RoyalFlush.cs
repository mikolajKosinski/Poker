using CoreBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBusinessLogic.Hands
{
    public class RoyalFlush : BaseHandManager, IFigureManager
    {
        public RoyalFlush(IList<ICard> hand, IList<ICard> desk) : base(hand, desk)
        {

        }

        public string Name { get; } = "RoyalFlush";
        public void Check()
        {
            var flush = new Flush(hand, desk);
            flush.Check();
            
            if (flush.Probability != 100 || NotInOrder(flush.CardList))
            {
                Probability = (int)GetOddsPercentage(GetOuts().Count());
                var color = GetDominatingColor();
                CardList = tempHand.Where(p => p.Color == color && p.Figure >= CardFigure._10).ToList();
                return;
            }

            if (flush.CardList.Any(x => x.Figure == CardFigure._As))
            {
                Probability = 100;
                var color = GetDominatingColor();
                CardList = GetRFByColor(color);
            }
            else
            {
                var sf = new StraightFlush(hand, desk);
                sf.Check();
                CardList = sf.CardList;
                Probability = (int)GetOddsPercentage(GetOuts().Count());
            }
        }

        public IList<ICard> GetOuts()
        {
            CardsNeeded = GetNeededCardsCount();
            var color = GetDominatingColor();
            var rf = GetRFByColor(color);

            foreach(var item in tempHand)
            {
                var rfItem = rf.FirstOrDefault(p => p.Color == item.Color && p.Figure == item.Figure);

                if (rfItem != null) rf.Remove(rfItem);
            }

            return rf;
        }

        private int GetNeededCardsCount()
        {
            var highestCardHand = tempHand.Any(c => c.Figure == CardFigure._As) ? 
                tempHand.First(c => c.Figure == CardFigure._As) :
                new Card(CardFigure._As, CardColor.club);
            var lowestCard = highestCardHand.Figure - 4;
            var elements = GetWithNoRept(tempHand).Where(p => p.Figure >= lowestCard).OrderByDescending(x => x.Figure).ToList();

            return 5 - elements.Count();
        }

        private List<ICard> GetWithNoRept(List<ICard> list)
        {
            var result = new List<ICard>();

            foreach (var item in list)
            {
                if (!result.Any(p => p.Figure == item.Figure))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        private List<ICard> GetRFByColor(CardColor color)
        {
            return new List<ICard>
            {
                new Card(CardFigure._As, color),
                new Card(CardFigure._King, color),
                new Card(CardFigure._Queen, color),
                new Card(CardFigure._Jack, color),
                new Card(CardFigure._10, color)
            };
        }
    }
}
