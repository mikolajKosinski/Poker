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
            CardList = tempHand;
            Probability = 0;
            cardsDeck = GetDeck();
        }

        public int Probability { get; set; }
        public IList<ICard> CardList { get; set; } 
        protected IList<ICard> hand;
        protected IList<ICard> desk;
        protected IList<ICard> cardsDeck;

        protected List<ICard> tempHand { get { return hand.Concat(desk).ToList(); } }

        protected void SetHand(IList<ICard> hand) => this.hand = hand;
        protected void SetDesk(IList<ICard> desk) => this.desk = desk;

        protected decimal GetOddsPercentage(int outs)
        {
            decimal restOfCards = GetDeckExceptTempHand().Count();
            decimal loose = restOfCards - outs;
            decimal result = (outs / loose) * 100;
            return result;
        }

        protected List<ICard> GetGroup(List<ICard> cards, int groupCount)
        {
            var groups = cards
                .GroupBy(u => u.Figure)
                .Select(grp => grp.ToList())
                .ToList();
            if (groups.Any(x => x.Count == groupCount)) return groups.First(x => x.Count == groupCount);
            return new List<ICard>();
        }

        protected List<List<ICard>> GetAllGroupsByFigure(List<ICard> cards)
        {
            var groups = cards
                .GroupBy(u => u.Figure)
                .Select(grp => grp.ToList())
                .ToList();
            if (groups.Any(x => x.Count > 1)) return groups.Where(p => p.Count > 1).ToList();
            return new List<List<ICard>>();
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

        protected bool NotInOrder(IList<ICard> tempHand)
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

        public List<ICard> GetDeckExceptTempHand()
        {
            var deck = GetDeck();
            var tempHand = hand.Concat(desk).ToList();

            foreach(var item in tempHand)
            {
                var card = deck.First(p => p.Color == item.Color && p.Figure == item.Figure);
                deck.Remove(card);
            }

            return deck;
        }

        protected IList<ICard> GetDominatingColorGroup()
        {
            var tempHand = hand.Concat(desk).ToList();
            var groupedByColor = tempHand.GroupBy(p => p.Color).OrderBy(m => m.Count()).ToList();
            return groupedByColor.Last().ToList();
        }

        protected List<ICard> GetDeck()
        {
            return new List<ICard>
            {
                new Card(CardFigure._2, CardColor.club),
                new Card(CardFigure._3, CardColor.club),
                new Card(CardFigure._4, CardColor.club),
                new Card(CardFigure._5, CardColor.club),
                new Card(CardFigure._6, CardColor.club),
                new Card(CardFigure._7, CardColor.club),
                new Card(CardFigure._8, CardColor.club),
                new Card(CardFigure._9, CardColor.club),
                new Card(CardFigure._10, CardColor.club),
                new Card(CardFigure._Jack, CardColor.club),
                new Card(CardFigure._Queen, CardColor.club),
                new Card(CardFigure._King, CardColor.club),
                new Card(CardFigure._As, CardColor.club),

                new Card(CardFigure._2, CardColor.diamond),
                new Card(CardFigure._3, CardColor.diamond),
                new Card(CardFigure._4, CardColor.diamond),
                new Card(CardFigure._5, CardColor.diamond),
                new Card(CardFigure._6, CardColor.diamond),
                new Card(CardFigure._7, CardColor.diamond),
                new Card(CardFigure._8, CardColor.diamond),
                new Card(CardFigure._9, CardColor.diamond),
                new Card(CardFigure._10, CardColor.diamond),
                new Card(CardFigure._Jack, CardColor.diamond),
                new Card(CardFigure._Queen, CardColor.diamond),
                new Card(CardFigure._King, CardColor.diamond),
                new Card(CardFigure._As, CardColor.diamond),

                new Card(CardFigure._2, CardColor.heart),
                new Card(CardFigure._3, CardColor.heart),
                new Card(CardFigure._4, CardColor.heart),
                new Card(CardFigure._5, CardColor.heart),
                new Card(CardFigure._6, CardColor.heart),
                new Card(CardFigure._7, CardColor.heart),
                new Card(CardFigure._8, CardColor.heart),
                new Card(CardFigure._9, CardColor.heart),
                new Card(CardFigure._10, CardColor.heart),
                new Card(CardFigure._Jack, CardColor.heart),
                new Card(CardFigure._Queen, CardColor.heart),
                new Card(CardFigure._King, CardColor.heart),
                new Card(CardFigure._As, CardColor.heart),

                new Card(CardFigure._2, CardColor.spade),
                new Card(CardFigure._3, CardColor.spade),
                new Card(CardFigure._4, CardColor.spade),
                new Card(CardFigure._5, CardColor.spade),
                new Card(CardFigure._6, CardColor.spade),
                new Card(CardFigure._7, CardColor.spade),
                new Card(CardFigure._8, CardColor.spade),
                new Card(CardFigure._9, CardColor.spade),
                new Card(CardFigure._10, CardColor.spade),
                new Card(CardFigure._Jack, CardColor.spade),
                new Card(CardFigure._Queen, CardColor.spade),
                new Card(CardFigure._King, CardColor.spade),
                new Card(CardFigure._As, CardColor.spade)
            };
        }
    }
}
