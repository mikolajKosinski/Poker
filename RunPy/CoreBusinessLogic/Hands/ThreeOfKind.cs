using CoreBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBusinessLogic.Hands
{
    public class ThreeOfKind : BaseHandManager, IFigureManager
    {
        public ThreeOfKind(IList<ICard> hand, IList<ICard> desk) : base(hand, desk) { }

        public string Name { get; } = "ThreeOfKind";

        public IList<ICard> GetOuts()
        {
            OutsCount = GetNeededCardsCount();
            var rest = GetDeckExceptTempHand();
            var groups = GetAllGroupsByFigure(tempHand);
            var outs = new List<ICard>();

            foreach(var list in groups)
            {
                outs.AddRange(rest.Where(p => p.Figure == list[0].Figure));
            }

            return outs;
        }

        public IList<ICard> GetCards() => new List<ICard>();

        public void Check()
        {
            var tempHand = hand.Concat(desk).ToList();
            if (CheckGroupCount(tempHand, 3))
            {
                var threeOf = GetGroup(tempHand, 3);
                foreach (var card in threeOf)
                {
                    CardList.Add(tempHand.First(c => c.ID == card.ID));
                }
                Probability = 100;
            }
            else
            {
                decimal outs = GetOuts().Count();
                decimal cardsLeft = 52 - tempHand.Count();
                Probability = decimal.Round((outs / cardsLeft) * 100, 2);

                //if (CheckGroupCount(tempHand, 2)) CardList = GetGroup(tempHand, 2);
                //Probability = (int)GetOddsPercentage(GetOuts().Count());
                //OutsList = GetOuts().ToList();
                //OutsCount = GetOuts().Count();
            }
        }

        private int GetNeededCardsCount()
        {
            if (GetGroup(tempHand, 3).Any()) return 0;
            if (GetGroup(tempHand, 2).Any()) return 1;
            return 2;
        }
    }
}
