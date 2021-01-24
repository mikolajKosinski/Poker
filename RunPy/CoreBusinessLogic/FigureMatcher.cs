using CoreBusinessLogic.Hands;
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
        private Dictionary<string, PokerFigure> matchedFigures;
        public IDictionary<PokerHands, IFigureManager> PokerHandsDict { get; set; }

        private List<ICard> deck;

        public FigureMatcher()
        {
            hand = new List<ICard>();
            desk = new List<ICard>();
            matchedFigures = new Dictionary<string, PokerFigure>();
            deck = GetDeck();
            PokerHandsDict = getHands();
        }

        private IDictionary<PokerHands, IFigureManager> getHands()
        {
            return new Dictionary<PokerHands, IFigureManager>
            {
                { PokerHands.Pair, new Pair(hand, desk) },
                { PokerHands.ThreeOfKind, new ThreeOfKind(hand, desk) },
                { PokerHands.Straight, new Straight(hand, desk) },
                { PokerHands.Flush, new Flush(hand, desk) }
            };
        }

        private List<ICard> GetDeck()
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

        private List<int> GetFiguresList()
        {
            return Enum.GetValues(typeof(CardFigure)).Cast<int>().ToList();
        }

        public void CheckHand()
        {
            foreach (var item in PokerHandsDict.Keys)
            {
                PokerHandsDict[item].Check();
            }
            //pokerHandsDict.for
            //pokerHandsDict[PokerHands.Pair].Check();
            //CheckThreeOfKind();
            //CheckFourOfKind();
            //CheckStraight();
            //CheckFlush();
            //CheckFull();

        }

        public void AddCardToFlop(CardFigure figure, CardColor color)
        {
            desk.Add(new Card(figure, color));
        }

        public void AddCardToHand(CardFigure figure, CardColor color)
        {
            hand.Add(new Card(figure, color));
        }

        //private void CheckPair()
        //{
        //    var tempHand = hand.Concat(flop).ToList();
        //    if (!CheckGroupCount(tempHand, 2)) return;
        //    var pair = GetGroup(tempHand, 2);
        //    var name = "Pair";
        //    var number = 1;
        //    while (pair.Any())
        //    {
        //        matchedFigures.Add($"{name}{number}", new PokerFigure("Pair", pair, 100));
        //        tempHand = tempHand.Except(pair).ToList();
        //        number++;
        //        pair = GetGroup(tempHand, 2);
        //    }
        //}

        private void CheckThreeOfKind()
        {
            var tempHand = hand.Concat(desk).ToList();
            var cardList = new List<ICard>();
            var percent = 0;

            if (CheckGroupCount(tempHand, 3)) 
            {
                var threeOf = GetGroup(tempHand, 3);
                foreach (var card in threeOf)
                {
                    cardList.Add(tempHand.First(c => c.ID == card.ID));
                }
                percent = 100;
            }
            else
            {
                CheckThreeOfKindProbability(tempHand);
            }
            matchedFigures.Add("ThreeOfKind", new PokerFigure("ThreeOfKind", cardList, percent));
        }

        private int CheckThreeOfKindProbability(List<ICard> tempHand)
        {
            var percent = 0;
            var figures = tempHand.Select(p => p.Figure);
            var rand = new Random();
            var outs = getOuts(hand, PokerHands.ThreeOfKind);

            return percent;
        }

        private List<ICard> getOuts(List<ICard> hand, PokerHands pokerHand)
        {
            switch(pokerHand)
            {
                case PokerHands.ThreeOfKind:
                    var temp = deck.Where(p => !hand.Contains(p)).ToList();
                    return new List<ICard>();
                default:
                    return new List<ICard>();
            }
        }

        private void CheckFourOfKind()
        {
            var tempHand = hand.Concat(desk).ToList();

            if (!CheckGroupCount(tempHand, 4)) return;

            var fourOf = GetGroup(tempHand, 4);
            var cardList = new List<ICard>();
            foreach (var card in fourOf)
            {
                cardList.Add(tempHand.First(c => c.ID == card.ID));
            }
            matchedFigures.Add("FourOfKind", new PokerFigure("FourOfKind", cardList, 100));
        }

        private void CheckStraight()
        {
            var tempHand = hand.Concat(desk).ToList();
            var cardList = new List<ICard>();
            tempHand = tempHand.OrderBy(c => c.Figure).ToList();

            if (cardList.Count < 5) return;
            if (NotInOrder(tempHand)) return;

            matchedFigures.Add("Straight", new PokerFigure("Straight", cardList, 100));
        }

        private void CheckFlush()
        {
            var tempHand = hand.Concat(desk).ToList();
            var cardList = new List<ICard>();

            if (!tempHand.GroupBy(x => x.Color)
                        .Where(group => group.Count() >= 5)
                        .ToList()
                        .Any()) return;
            var key = tempHand.GroupBy(x => x.Color)
                        .Where(group => group.Count() >= 5)
                        .Select(group => group.Key)
                        .First();
            cardList = tempHand.Where(x => x.Color == key).ToList();
            matchedFigures.Add("Flush", new PokerFigure("Flush", cardList, 100));            

            if (NotInOrder(cardList)) return;

            if (cardList.Any(x => x.Figure == CardFigure._As))
            {
                matchedFigures.Add("Royal Flush", new PokerFigure("Royal Flush", cardList, 100));
            }
            else
            {
                matchedFigures.Add("Straight Flush", new PokerFigure("Straight Flush", cardList, 100));
            }
        }

        private void CheckFull()
        {
            var tempHand = hand.Concat(desk).ToList();

            if (!CheckGroupCount(tempHand, 2)) return;
            if (!CheckGroupCount(tempHand, 3)) return;

            var pair = GetGroup(tempHand, 2);
            var threeOfKind = GetGroup(tempHand, 3);

            if (!pair.Any() || !threeOfKind.Any()) return;

            var cardList = new List<ICard>();
            var full = pair.Concat(threeOfKind).ToList();
            foreach (var card in full)
            {
                cardList.Add(tempHand.First(c => c.ID == card.ID));
            }
            matchedFigures.Add("Full", new PokerFigure("Full", cardList, 100));
        }

        private bool NotInOrder(List<ICard> tempHand)
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

        private bool CheckGroupCount(List<ICard> cards, int elements)
        {
            return cards.GroupBy(x => x.Figure)
                        .Where(group => group.Count() == elements)
                        .Select(group => group)
                        .Any();
        }

        private List<ICard> GetGroup(List<ICard> cards, int groupCount)
        {
            var groups = cards
                .GroupBy(u => u.Figure)
                .Select(grp => grp.ToList())
                .ToList();
            if (groups.Any(x => x.Count == groupCount)) return groups.First(x => x.Count == groupCount);
            return new List<ICard>();
        }
    }
}
