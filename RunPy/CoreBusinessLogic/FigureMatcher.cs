﻿using CoreBusinessLogic.Hands;
using CoreBusinessLogic.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static CoreBusinessLogic.Enums;

namespace CoreBusinessLogic
{
    public class FigureMatcher : IFigureMatcher
    {
        private List<ICard> desk;
        private List<ICard> hand;
        private IDictionary<PokerHands, IFigureManager> handsDict;
        public IDictionary<PokerHands, IFigureManager> PokerHandsDict { get; set; }


        public FigureMatcher()
        {
            hand = new List<ICard>();
            desk = new List<ICard>();
            PokerHandsDict = getHands();
        }

        public IList<ICard> GetHand(PokerHands hand)
        {
            return getHands()[hand].GetCards();
        }

        private IDictionary<PokerHands, IFigureManager> getHands()
        {
            if (handsDict == null)
                handsDict = getNewHandsDictionary();

            return handsDict;
        }

        private IDictionary<PokerHands, IFigureManager> getNewHandsDictionary()
        {
            var straight = new Straight(hand, desk);
            return new Dictionary<PokerHands, IFigureManager>
            {
                { PokerHands.Pair, new Pair(hand, desk) },
                { PokerHands.ThreeOfKind, new ThreeOfKind(hand, desk) },
                { PokerHands.Straight, straight },
                { PokerHands.Flush, new Flush(hand, desk) },
                { PokerHands.FourOfKind, new FourOfKind(hand, desk) },
                { PokerHands.Full, new Full(hand, desk) },
                { PokerHands.RoyalFlush, new RoyalFlush(hand, desk, straight) },
                { PokerHands.StraightFlush, new StraightFlush(hand, desk, straight) }
            };
        }

        public void Clean()
        {
            desk.Clear();
            hand.Clear();
            PokerHandsDict = getHands();
        }

        public void CheckHand()
        {
            foreach (var item in PokerHandsDict.Keys)
            {
                try
                {
                    PokerHandsDict[item].Check();
                }
                catch(Exception x)
                {

                }
            }
        }

        public void AddCardToFlop(CardFigure figure, CardColor color)
        {
            desk.Add(new Card(figure, color));
        }

        public void AddCardToHand(CardFigure figure, CardColor color)
        {
            hand.Add(new Card(figure, color));
        }

        public void AddCardToFlop(ICard card)
        {
            desk.Add(card);
        }

        public void AddCardToHand(ICard card)
        {
            hand.Add(card);
        }
    }
}
