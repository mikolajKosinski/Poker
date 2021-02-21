﻿using CoreBusinessLogic.Interfaces;
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
            Probability = 0;
        }

        public IList<ICard> GetOuts()
        {
            var highestCardHand = tempHand.OrderByDescending(x => x.Figure).First();
            var highestPossibleFigure = highestCardHand.Figure + 2;

            if ((int)highestPossibleFigure > 14) highestPossibleFigure = CardFigure._As;

            var lowestPossible = highestPossibleFigure - 5;
            var lowestByHand = highestCardHand.Figure - 5;
            var restFromDeck = GetDeckExceptTempHand();
            var outs = new List<ICard>();
            
            for(var figure = highestPossibleFigure; figure >= lowestPossible; figure--)
            {
                if(!tempHand.Any(x => x.Figure == figure))
                {
                    outs.AddRange(restFromDeck.Where(p => p.Figure == figure).ToList());
                }
            }

            for (var figure = highestCardHand.Figure; figure >= lowestByHand; figure--)
            {
                if (!tempHand.Any(x => x.Figure == figure))
                {
                    outs.AddRange(restFromDeck.Where(p => p.Figure == figure && !outs.Any(o => o.Color == p.Color && o.Figure == p.Figure)).ToList());
                }
            }

            return outs;
        }

        public void Check()
        {
            var th = tempHand.OrderBy(c => c.Figure).ToList();

            if (th.Count < 5) return;
            if (!IsInOrder(tempHand)) return;

            Probability = 100;
        }
    }
}
