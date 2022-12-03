using CoreBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBusinessLogic.Hands
{
    public class FourOfKind : BaseHandManager, IFigureManager
    {
        public FourOfKind(IList<ICard> hand, IList<ICard> desk) : base(hand, desk)
        {

        }

        public IList<ICard> GetCards() => new List<ICard>();
        private IList<ICard> _cardsOnHand = new List<ICard>();
        public string Name { get; } = "FourOf";

        public void Check()
        {
            if (_gotFour())
            {               
                Probability = 100;
            }
            else
            {
                decimal outs = GetOuts().Count();
                decimal cardsLeft = 52 - tempHand.Count();
                Probability = decimal.Round((outs / cardsLeft) * 100, 2);
            }

            var group = GetGroup(tempHand, 4);
            if (!group.Any()) group = GetGroup(tempHand, 3);
            if (!group.Any()) group = GetGroup(tempHand, 2);
            OutsList = GetOuts().ToList();
            CardList = group;
        }

        private bool _gotFour()
        {
            return CheckGroupCount(tempHand, 4);
        }

        public IList<ICard> GetOuts()
        {
            OutsCount = GetNeededCardsCount();
            var rest = GetDeckExceptTempHand();
            var groups = GetAllGroupsByFigure(tempHand);
            var outs = new List<ICard>();

            if (tempHand.Count == 5)
            {
                var pair = _cardsOnHand = GetGroup(tempHand, 2);
                var threeOf = _cardsOnHand = GetGroup(tempHand, 3);
                _cardsOnHand = threeOf.Any() ? threeOf : pair;
            }

            if (tempHand.Count == 6)
                _cardsOnHand = GetGroup(tempHand, 3);

            foreach (var list in groups)
            {
                outs.AddRange(rest.Where(p => p.Figure == list[0].Figure));
            }

            return outs;
            //OutsCount = GetNeededCardsCount();
            //return GetMatchingCardsFromDeck();
        }

        private int GetNeededCardsCount()
        {
            var cardNeeded = 0;
            var gotFourOf = CheckGroupCount(tempHand, 4);
            var gotThreeOf = CheckGroupCount(tempHand, 3);
            var gotTwoOf = CheckGroupCount(tempHand, 2);
                        
            if (!gotFourOf) cardNeeded++;
            if (!gotThreeOf) cardNeeded++;
            if (!gotTwoOf) cardNeeded++;

            return cardNeeded;
        }

        private IList<ICard> GetMatchingCardsFromDeck()
        {
            var groups = GetAllGroupsByFigure(tempHand);
            var matchingCards = new List<ICard>();

            foreach(var group in groups)
            {
                var figure = group[0].Figure;
                var cards = GetMatchingCardsFromDeckByFigure(group, figure);
                matchingCards.AddRange(cards);
            }

            return matchingCards;
        }

        private IList<ICard> GetMatchingCardsFromDeckByFigure(List<ICard> group, CardFigure figure)
        {
            return GetDeckExceptTempHand().Where(p => p.Figure == figure && !group.Any(c => c.Color == p.Color)).ToList();
        }
    }
}
