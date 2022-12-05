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
        private IList<int> _allFigures = new List<int>() { 2,3,4,5,6,7,8,9,10,11,12,13,14 };
        private List<ICard> _availableCards = new List<ICard>();

        public IList<ICard> GetCards() => _availableCards;

        public IList<ICard> GetOuts()
        {
            if (!tempHand.Any()) return new List<ICard>();
            var cardsLeft = 7 - tempHand.Count();
            var possibleSets = new List<List<int>>();
            var outs = new List<ICard>();
            var allCards = GetDeck();

            for (int q = 0; q < 9; q++)
            {
                var set = new List<int>();
                for(int w = 0; w < 5; w++)
                {
                    set.Add(_allFigures[q+w]);
                }
                possibleSets.Add(set);
            }

            foreach (var set in possibleSets)
            {
                var temp = tempHand.Select(p => Convert.ToInt32(p.Figure)).ToList();
                var neededFigures = set.Except(temp);
                if (neededFigures.Count() > cardsLeft)
                    continue;

                var cardForHand = tempHand.Where(p => set.Contains(Convert.ToInt32(p.Figure))).Select(p => p).ToList();
                _availableCards.AddRange(cardForHand.Where(p => !_availableCards.Contains(p)));

                foreach (var figure in neededFigures)
                {
                    var possibleOuts = allCards.Select(p => p).Where(p => Convert.ToInt32(p.Figure) == figure);
                    
                    foreach(var card in possibleOuts)
                    {
                        if(!tempHand.Any(p =>p.Figure == card.Figure) && !outs.Contains(card))
                        {
                            outs.Add(card);
                        }
                    }
                }
            }


                //foreach(var card in tempHand)
                //{
                //    foreach(var set in possibleSets) 
                //    {
                //        var cardFigure = Convert.ToInt32(card.Figure.ToString());
                //        var neededFigures = set.Except(new List<int>() { car });
                //    }
                //}

                //OutsCount = GetNeededCardsCount();
                //if (OutsCount > cardsLeft) return new List<ICard>();

                //var highestCardHand = tempHand.OrderByDescending(x => x.Figure).First();
                //var highestPossibleFigure = highestCardHand.Figure + 2;

                //if ((int)highestPossibleFigure > 14) highestPossibleFigure = CardFigure._As;

                //var lowestPossible = highestPossibleFigure - 5;
                //var lowestByHand = highestCardHand.Figure - 5;
                //var restFromDeck = GetDeckExceptTempHand();
                //var outs = new List<ICard>();

                //var availableByHighest = tempHand.Where(p => p.Figure > lowestPossible && p.Figure < highestPossibleFigure)
                //    .GroupBy(p => p.Figure)
                //    .ToList();

                //var cardsNeededByHighest = 5 - availableByHighest.Count();

                //if (cardsNeededByHighest < cardsLeft)
                //{
                //    for (var figure = highestPossibleFigure; figure >= lowestPossible; figure--)
                //    {
                //        if (!tempHand.Any(x => x.Figure == figure))
                //        {
                //            outs.AddRange(restFromDeck.Where(p => p.Figure == figure).ToList());
                //        }
                //    }
                //}

                //for (var figure = highestCardHand.Figure; figure > lowestByHand; figure--)
                //{
                //    if (!tempHand.Any(x => x.Figure == figure))
                //    {
                //        outs.AddRange(restFromDeck.Where(p => p.Figure == figure && !outs.Any(o => o.Color == p.Color && o.Figure == p.Figure)).ToList());
                //    }
                //}

                return outs;
        }

        //private int GetNeededCardsCount()
        //{
        //    var highestCardHand = tempHand.OrderByDescending(x => x.Figure).First();
        //    var lowestCard = highestCardHand.Figure - 4;
        //    var elements = GetDeckExceptTempHand()
        //        .Where(p => p.Figure > lowestCard && p.Figure < highestCardHand.Figure)
        //        .GroupBy(p => p.Figure)
        //        .ToList();
        //    return elements.Count();
        //}

        public void Check()
        {
            if (NotInOrder(tempHand))
            {
                //Probability = (int)GetOddsPercentage(GetOuts().Count());
                //var highestCard = tempHand.OrderByDescending(x => x.Figure).First();
                //var lowestCard = tempHand.OrderByDescending(x => x.Figure).Last();
                //var elements = tempHand.Where(p => p.Figure >= lowestCard.Figure).OrderByDescending(x => x.Figure).Take(5).ToList();
                //CardList = GetWithNoRept(elements);
                decimal outs = GetOuts().Count();
                decimal cardsLeft = 52 - tempHand.Count();
                Probability = decimal.Round((outs / cardsLeft)*100, 2);
                //Probability = (int)GetOddsPercentage(GetOuts().Count());
                //Probability = GetOuts().Count() * 4;
                OutsList = GetOuts().ToList();
                OutsCount = GetOuts().Count();
                return;
            }

            CardList = tempHand.OrderByDescending(x => x.Figure).Take(5).ToList();
            //CardList = GetWithNoRept(res);
            Probability = 100;
        }

        //private List<ICard> GetWithNoRept(List<ICard> list)
        //{
        //    var result = new List<ICard>();

        //    foreach(var item in list)
        //    {
        //        if(!result.Any(p => p.Figure == item.Figure))
        //        {
        //            result.Add(item);
        //        }
        //    }

        //    return result;
        //}
    }
}
