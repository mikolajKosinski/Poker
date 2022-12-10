using Autofac;
using CoreBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace CoreBusinessLogic.Hands
{
    public class Pair : BaseHandManager, IFigureManager
    {
        public Pair(IList<ICard> hand, IList<ICard> desk, ISettings container) : base (hand, desk, container)
        {
            Probability = 0;
        }

        public IList<ICard> GetCards() => _cardsOnHand;
        private IList<ICard> _cardsOnHand = new List<ICard>();

        public string Name { get; } = "Pair";

        public IList<ICard> GetOuts()
        {
            var deck = GetDeckExceptTempHand();
            var matches = new List<ICard>();

            foreach(var item in tempHand)
            {
                var cards = GetDeckExceptTempHand().Where(p => p.Figure == item.Figure).ToList();

                foreach (var card in cards)
                {
                    if (!matches.Any(p => p.Figure == card.Figure && p.Color == card.Color))
                    {
                        matches.Add(card);
                    }
                }
            }

            return matches;
        }

        public Suggestion GetSuggestion()
        {
            var pairs = GetGroup(tempHand, 2);
            var premium = new List<CardFigure> { CardFigure._As, CardFigure._King, CardFigure._Queen, CardFigure._Jack };
            var middle = new List<CardFigure> { CardFigure._10, CardFigure._9, CardFigure._8, CardFigure._7, CardFigure._6 };
            if (tempHand.Count == 2 )
            {
                if (Probability == 100 && pairs.Any(p => premium.Contains(p.Figure)))
                {
                    return new Suggestion { Bet = 2, Probability = 100 };
                }
                if (Probability == 100 && pairs.Any(p => middle.Contains(p.Figure)))
                {
                    return new Suggestion { Bet = 2, Probability = 50 };
                }
            }

            if (tempHand.Count == 5)
            {
                if (Probability == 100 && pairs.Any(p => premium.Contains(p.Figure)))
                {
                    return new Suggestion { Bet = 2, Probability = 80 };
                }
                if (Probability == 100 && pairs.Any(p => middle.Contains(p.Figure)))
                {
                    return new Suggestion { Bet = 2, Probability = 50 };
                }
            }

            return new Suggestion { Bet = 0, Probability = 0 };
        }

        public void Check()
        {
            var tempHand = hand.Concat(desk).ToList();
            //if (!CheckGroupCount(tempHand, 2)) return;
            var pair = GetGroup(tempHand, 2);
            var threeOf = GetGroup(tempHand, 3);
            var fourOf = GetGroup(tempHand, 4);
            decimal outs = GetOuts().Count();
            decimal cardsLeft = 52 - tempHand.Count();
            OutsList = GetOuts().ToList();
            OutsCount = GetOuts().Count();

            if (pair.Any() || threeOf.Any() || fourOf.Any())
            {
                Probability = 100;
                if (pair.Any())
                {
                    pair.ForEach(p => CardList.Add(p));
                    _cardsOnHand = pair;
                }
                if (threeOf.Any())
                {
                    threeOf.ForEach(p => CardList.Add(p));
                    _cardsOnHand = threeOf;
                }
                if (fourOf.Any())
                {
                    fourOf.ForEach(p => CardList.Add(p));
                    _cardsOnHand = fourOf;
                }
            }
            else
            {
                Probability = GetProbability();
            }
        }
    }
}
