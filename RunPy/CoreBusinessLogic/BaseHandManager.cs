using Autofac;
using CoreBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CoreBusinessLogic.Enums;

namespace CoreBusinessLogic
{
    public class BaseHandManager
    {
        ISettings settings;

        public BaseHandManager(IList<ICard> hand, IList<ICard> desk, ISettings settings)
        {
            this.hand = hand;
            this.desk = desk;
            CardList = tempHand;
            Probability = 0;
            this.settings= settings;
        }

        protected List<ICard> _availableCards = new List<ICard>();
        public IList<ICard> GetCards() => _availableCards;

        public decimal Probability { get; set; }
        public int OutsCount { get; set; }
        public List<ICard> OutsList { get; set; }
        public List<ICard> CardList { get; set; } 
        protected IList<ICard> hand;
        protected IList<ICard> desk;

        protected List<ICard> tempHand { get { return hand.Concat(desk).ToList(); } }

        protected decimal GetProbability()
        {
            var formula = settings.SelectedFormula.ToString();
            var cardsLeft = 7 - tempHand.Count;
            decimal chance = (decimal)OutsList.Count / (decimal)GetDeckExceptTempHand().Count();

            switch (formula) 
            {
                case "algebraic":
                    return decimal.Round(chance * 100, 2);

                case "2-4":
                    var multiplier = cardsLeft * 2;
                    return OutsList.Count * multiplier;
                default:
                    return 0;
            }
        }

        protected decimal GetOddsPercentage(int outs)
        {
            var cardsLeft = 7 - tempHand.Count();
            if (OutsCount > cardsLeft) return 0;

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
            if (tempHand.Count() < 5) return false;
            tempHand = tempHand.OrderBy(p => p.Figure).ToList();
            var index = 0;
            var elementsNotInOrder = new List<ICard>();

            while(index < tempHand.Count())
            {
                var item = tempHand[index];
                var itemPlus = tempHand.FirstOrDefault(p => p.Figure == item.Figure + 1);
                if (itemPlus == null && index != tempHand.Count -1) elementsNotInOrder.Add(item);
                index++;
            }

            if (tempHand.Count() == 7) return elementsNotInOrder.Count() < 3;
            if (tempHand.Count() == 6) return elementsNotInOrder.Count() < 2;
            return elementsNotInOrder.Count() < 1;
        }

        protected bool NotInOrder(IList<ICard> tempHand) => !IsInOrder(tempHand);

        public List<ICard> GetDeckExceptTempHand()
        {
            var deck = GetDeck();
            var tempHand = hand.Concat(desk).ToList();

            foreach (var item in tempHand)
            {
                var card = deck.FirstOrDefault(p => p.Color == item.Color && p.Figure == item.Figure);
                if (deck.Contains(card))
                {
                    deck.Remove(card);
                }
                else
                {
                    deck.Remove(deck[0]); //TODO fix !!!!!!
                }
            }

            return deck;
        }

        protected IList<ICard> GetDominatingColorGroup()
        {
            var groupedByColor = tempHand.GroupBy(p => p.Color).OrderBy(m => m.Count()).ToList();
            return groupedByColor.Last().ToList();
        }

        protected CardColor GetDominatingColor() => GetDominatingColorGroup()[0].Color;

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
