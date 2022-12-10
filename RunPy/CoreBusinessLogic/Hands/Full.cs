using Autofac;
using CoreBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBusinessLogic.Hands
{
    public class Full : BaseHandManager, IFigureManager
    {
        ISettings container;

        public Full(IList<ICard> hand, IList<ICard> desk, ISettings container) : base(hand, desk, container)
        {
            this.container = container;
        }

        public IList<ICard> GetCards() => _availableCards;
        private IList<ICard> _availableCards = new List<ICard>();
        public string Name { get; } = "Full";
        public void Check()
        {
            if (GetGroup(tempHand, 2).Any() && GetGroup(tempHand, 3).Any())
            {
                Probability = 100;
            }

            OutsList = GetOuts().ToList();

            Probability = GetProbability();

            if (Probability == 0 || tempHand.Count() == 7) return;
            
            
            //CardList = group;
            //var pair = GetGroup(tempHand, 2);
            //var threeOfKind = GetGroup(tempHand, 3);

                //if(pair.Any())
                //{
                //    var secondPairHand = tempHand;
                //    secondPairHand.Remove(pair[0]);
                //    secondPairHand.Remove(pair[1]);
                //    var secondPair = GetGroup(secondPairHand, 2);
                //    CardList.AddRange(secondPair);
                //}

                //CardList.AddRange(pair);
                //CardList.AddRange(threeOfKind);

                //if (!pair.Any() || !threeOfKind.Any())
                //{
                //    Probability = (int)GetOddsPercentage(GetOuts().Count());
                //    OutsList = GetOuts().ToList();
                //    OutsCount = GetOuts().Count();
                //    return;
                //}

                //Probability = 100;
        }

        public IList<ICard> GetOuts()
        {
            //OutsCount = GetNeededCardsCount();
            var cards = GetDeckExceptTempHand();
            var outs = new List<ICard>();

            //if (GetGroup(tempHand, 2).Any() && !AreThreeOfKindAvailable())
            //{
            //    //decimal outs = GetOuts().Count();
            //    //decimal cardsLeft = 52 - tempHand.Count();
            //    //Probability = decimal.Round((outs / cardsLeft) * 100, 2);

            //    var pairs = GetAllPairs(tempHand);
            //    var first = pairs.Item1;
            //    var second = pairs.Item2;
            //    foreach (var card in first)
            //    {
            //        _cardsOnHand.Add(card);
            //    }
            //    foreach (var card in second)
            //    {
            //        _cardsOnHand.Add(card);
            //    }
            //}
            //else if (AreThreeOfKindAvailable())
            //{
            //    var group = GetGroup(tempHand, 3);
            //    foreach (var card in group)
            //    {
            //        _cardsOnHand.Add(card);
            //    }
            //}
            //else
            //{
            //    if (GetGroup(tempHand, 2).Any())
            //    {
            //        var group = GetGroup(tempHand, 2);
            //        foreach (var card in group)
            //        {
            //            _cardsOnHand.Add(card);
            //        }
            //    }
            //}

            if (AreTwoPairsAvailable())
            {
                var pairs = GetAllPairs(tempHand);                
                var firstFigure = pairs.Item1[0].Figure;
                var secondFigure = pairs.Item2[0].Figure;
                var firstPairOuts = cards.Where(p => p.Figure == firstFigure).ToList();
                var secondPairOuts = cards.Where(p => p.Figure == secondFigure).ToList();
                outs.AddRange(firstPairOuts);
                outs.AddRange(secondPairOuts);
                
                foreach(var card in pairs.Item1)
                    _availableCards.Add(card);
                foreach (var card in pairs.Item2)
                    _availableCards.Add(card);

                return outs;
            }

            if (AreThreeOfKindAvailable())
            {
                var threeOf = GetGroup(tempHand, 3);
                var restOfCards = tempHand.Except(threeOf).ToList();

                foreach (var card in threeOf)
                    _availableCards.Add(card);

                foreach (var item in restOfCards)
                {
                    var figure = item.Figure;
                    outs.AddRange(cards.Where(p => p.Figure == figure).ToList());
                }

                return outs;
            }

            //if(GetGroup(tempHand, 2).Any())
            //{
            //    var pair = GetGroup(tempHand, 2);
            //    var pairFigure = pair[0].Figure;
            //    var ThreeOfCandidates = cards.Where(p => p.Figure == pairFigure).ToList();
            //    var restOfTempHand = tempHand.Except(pair);

            //    foreach (var card in pair)
            //        _availableCards.Add(card);

            //    foreach (var rest in restOfTempHand) 
            //    { 
            //        var pairCandidates = cards.Where(p => p.Figure == rest.Figure).ToList();
            //        outs.AddRange(pairCandidates);
            //    }
            //}

            //var deck = GetDeckExceptTempHand();
            //foreach(var card in deck)
            //{
            //    var cardsFromTemp = tempHand.Where(p => p.Figure == card.Figure);
            //    if(cardsFromTemp.Count() >= 1)
            //    {
            //        outs.Add(card);
            //    }
            //}
            return outs;
        }

        private int GetNeededCardsCount()
        {
            if (!AreThreeOfKindAvailable())
            {
                if(AreTwoPairsAvailable())
                {
                    return 1;
                }

                if(GetGroup(tempHand, 2).Any())
                {
                    return 2;
                }

                return 3;
            }

            if (AreTwoPairsAvailable()) return 0;
            return 1;
        }

        private bool AreTwoPairsAvailable()
        {
            if (tempHand.Count() < 4) return false;

            var pairs = GetAllPairs(tempHand);

            return pairs.Item1.Any() && pairs.Item2.Any();
        }

        private Tuple<IList<ICard>, IList<ICard>> GetAllPairs(List<ICard> tempHand)
        {
            var firstPair = GetGroup(tempHand, 2);

            if (!firstPair.Any()) return new Tuple<IList<ICard>, IList<ICard>>(new List<ICard>(), new List<ICard>());

            tempHand.Remove(firstPair[0]);
            tempHand.Remove(firstPair[1]);
            var secondPair = GetGroup(tempHand, 2);

            return new Tuple<IList<ICard>, IList<ICard>>(firstPair, secondPair);
        }

        private bool AreThreeOfKindAvailable()
        {
            var threeOf = new ThreeOfKind(hand, desk, container);
            threeOf.Check();

            return threeOf.Probability == 100;
        }
    }
}
