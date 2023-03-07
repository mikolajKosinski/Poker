
using CoreBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBusinessLogic.Hands
{
    public class RoyalFlush : BaseHandManager, IFigureManager
    {
        Straight straight { get; set; }

        public RoyalFlush(IList<ICard> hand, IList<ICard> desk, Straight straight, ISettings container) : base(hand, desk, container)
        {
            this.straight = straight;
        }

        //public IList<ICard> GetCards() => _availableCards;
        private IList<int> _allFigures = new List<int>() { 10, 11, 12, 13, 14 };
        public void UpdateHand(ICard card) => hand.Add(card);
        public void UpdateDesk(ICard card) => desk.Add(card);

        public string Name { get; } = "RoyalFlush";
        public void Check()
        {
            OutsList = GetOuts().ToList();
            Probability = GetProbability();

            if (Probability == 0 || tempHand.Count() == 7) return;

         
            //OutsCount = GetOuts().Count();
        }

        public IList<ICard> GetOuts()
        {
            var color = GetDominatingColor();
            var local = tempHand.Where(x => x.Color == color).ToList();

            if (!local.Any()) return new List<ICard>();
            var cardsLeft = 7 - tempHand.Count();
            var possibleSets = new List<List<int>>();
            var outs = new List<ICard>();
            var allCards = GetDeck();
            var figures = _allFigures.ToList();
            possibleSets.Add(figures);

            foreach (var set in possibleSets)
            {
                var temp = local.Select(p => Convert.ToInt32(p.Figure)).ToList();
                var neededFigures = set.Except(temp);
                if (neededFigures.Count() > cardsLeft)
                    continue;

                var cardForHand = local.Where(p => set.Contains(Convert.ToInt32(p.Figure))).Select(p => p).ToList();
                _availableCards.AddRange(cardForHand.Where(p => !_availableCards.Contains(p)));

                foreach (var figure in neededFigures)
                {
                    var possibleOuts = allCards.Select(p => p).Where(p => Convert.ToInt32(p.Figure) == figure);

                    foreach (var card in possibleOuts)
                    {
                        if (!local.Any(p => p.Figure == card.Figure) && !outs.Contains(card) && card.Color == color)
                        {
                            outs.Add(card);
                        }
                    }
                }
            }
            return outs;
        }

        //private int GetNeededCardsCount()
        //{
        //    var highestCardHand = tempHand.Any(c => c.Figure == CardFigure._As) ? 
        //        tempHand.First(c => c.Figure == CardFigure._As) :
        //        new Card(CardFigure._As, CardColor.club);
        //    var lowestCard = highestCardHand.Figure - 4;
        //    var elements = GetWithNoRept(tempHand).Where(p => p.Figure >= lowestCard).OrderByDescending(x => x.Figure).ToList();

        //    return 5 - elements.Count();
        //}

        //private List<ICard> GetWithNoRept(List<ICard> list)
        //{
        //    var result = new List<ICard>();

        //    foreach (var item in list)
        //    {
        //        if (!result.Any(p => p.Figure == item.Figure))
        //        {
        //            result.Add(item);
        //        }
        //    }

        //    return result;
        //}

        //private List<ICard> GetRFByColor(CardColor color)
        //{
        //    return new List<ICard>
        //    {
        //        new Card(CardFigure._As, color),
        //        new Card(CardFigure._King, color),
        //        new Card(CardFigure._Queen, color),
        //        new Card(CardFigure._Jack, color),
        //        new Card(CardFigure._10, color)
        //    };
        //}
    }
}
