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

        public void Check()
        {
            if (_gotFour())
            {               
                Probability = 100;
            }
            else
            {
                var outs = GetOuts().Count();
                Probability = (int)GetOddsPercentage(outs);
            }

            var group = GetGroup(tempHand, 4);
            if (!group.Any()) group = GetGroup(tempHand, 3);
            if (!group.Any()) group = GetGroup(tempHand, 2);
            CardList = group;
        }

        private bool _gotFour()
        {
            return CheckGroupCount(tempHand, 4);
        }

        public IList<ICard> GetOuts()
        {
            return GetMatchingCardsFromDeck();
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
