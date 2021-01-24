using CoreBusinessLogic;
using CoreDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace XUnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void GetCardByImage_Proper2CImage_ShouldReturnCardObject()
        {
            //var cardReco = new CardRecognition();
            //var cardManager = new CardManager(cardReco);

            //var res = cardManager.GetCardByImage("C:\\Users\\mkosi\\Documents\\GitHub\\Poker\\RunPy\\XUnitTests\\Resources\\2c.png");

            //Assert.NotNull(res);
            //Assert.Equal("2C", res.Name);
        }

        [Fact]
        public void IsInOrder()
        {
            var hand = new List<ICard>
            {
                new Card(CardFigure._2, CardColor.club) ,
                new Card(CardFigure._10, CardColor.club)
            };
            var desk = new List<ICard>
            {
                new Card(CardFigure._As, CardColor.club) ,
                new Card(CardFigure._Jack, CardColor.club) ,
                new Card(CardFigure._Queen, CardColor.club) ,
                new Card(CardFigure._King, CardColor.club) ,
                new Card(CardFigure._7, CardColor.club)
            };
            var tempHand = hand.Concat(desk).ToList();
            var fManager = new BaseHandManager(hand, desk);

            var order = fManager.IsInOrder(tempHand);
            Console.WriteLine();
        }
    }
}
