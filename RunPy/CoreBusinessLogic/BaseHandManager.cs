using CoreBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBusinessLogic
{
    public class BaseHandManager
    {
        public BaseHandManager(IList<ICard> hand, IList<ICard> desk)
        {
            this.hand = hand;
            this.desk = desk;
            CardList = new List<ICard>();
            Probability = 0;
        }

        public int Probability { get; set; }
        public IList<ICard> CardList { get; set; } 
        protected IList<ICard> hand;
        protected IList<ICard> desk;

        protected void SetHand(IList<ICard> hand) => this.hand = hand;
        protected void SetDesk(IList<ICard> desk) => this.desk = desk;
        

        protected List<ICard> GetGroup(List<ICard> cards, int groupCount)
        {
            var groups = cards
                .GroupBy(u => u.Figure)
                .Select(grp => grp.ToList())
                .ToList();
            if (groups.Any(x => x.Count == groupCount)) return groups.First(x => x.Count == groupCount);
            return new List<ICard>();
        }

        protected bool CheckGroupCount(List<ICard> cards, int elements)
        {
            return cards.GroupBy(x => x.Figure)
                        .Where(group => group.Count() == elements)
                        .Select(group => group)
                        .Any();
        }

        public bool IsInOrder(IList<ICard> tempHand)
        {
            var orderedList = tempHand.OrderBy(p => p.Figure).ToList();
            var elementsNotInOrder = orderedList
                .Where(p => p.Figure < CardFigure._As && !orderedList.Any(c => c.Figure == p.Figure + 1))
                .ToList();
            return elementsNotInOrder.Count() < 3;
        }

        protected bool NotInOrder(List<ICard> tempHand)
        {
            tempHand = tempHand.OrderBy(x => x.Figure).ToList();
            for (int q = 0; q < tempHand.Count - 1; q++)
            {
                var firstCard = tempHand[q].Figure;
                var nextCard = tempHand[q + 1].Figure;

                if (firstCard != nextCard - 1) return true;
            }

            return false;
        }
    }
}
