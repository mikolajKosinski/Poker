using CoreBusinessLogic;
using CoreBusinessLogic.Hands;
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

        [Fact]
        public void Flush_GetOuts()
        {
            var hand = new List<ICard>
            {
                new Card(CardFigure._2, CardColor.spade) ,
                new Card(CardFigure._10, CardColor.diamond)
            };
            var desk = new List<ICard>
            {
                new Card(CardFigure._As, CardColor.club) ,
                new Card(CardFigure._Jack, CardColor.heart) ,
                new Card(CardFigure._Queen, CardColor.club) ,
                new Card(CardFigure._King, CardColor.spade) ,
                new Card(CardFigure._7, CardColor.club)
            };
            var flush = new Flush(hand, desk);

            var outs = flush.GetOuts();
            
        }

        [Fact]
        public void sdfsdf()
        {
            var hand = new List<ICard>
            {
                new Card(CardFigure._King, CardColor.spade)
            };
            var desk = new List<ICard>
            {
                new Card(CardFigure._10, CardColor.club) ,
                new Card(CardFigure._Queen, CardColor.club) ,
                new Card(CardFigure._King, CardColor.diamond) ,
                new Card(CardFigure._2, CardColor.club)
            };
            var FourOf = new FourOfKind(hand, desk);

            var outs = FourOf.GetOuts();

            Console.WriteLine();
        }

        [Fact]
        public void sfdsf()
        {
            var hand = new List<ICard>
            {
                new Card(CardFigure._King, CardColor.spade)
            };
            var desk = new List<ICard>
            {
                new Card(CardFigure._10, CardColor.club) ,
                new Card(CardFigure._Queen, CardColor.club) ,
                new Card(CardFigure._King, CardColor.diamond) ,
                new Card(CardFigure._2, CardColor.club)
            };
            var manager = new BaseHandManager(hand, desk);

            var cards = manager.GetDeckExceptTempHand();

            Console.WriteLine();
            
        }

        [Fact]
        public void sfdfdsf()
        {
            var hand = new List<ICard>
            {
                new Card(CardFigure._King, CardColor.spade),
                new Card(CardFigure._8, CardColor.spade)
            };
            var desk = new List<ICard>
            {
                new Card(CardFigure._10, CardColor.club) ,
                new Card(CardFigure._King, CardColor.club) ,
                new Card(CardFigure._King, CardColor.diamond) ,
                new Card(CardFigure._2, CardColor.club)
            };
            var full = new Full(hand, desk);

            var outs = full.GetOuts();

        }

        [Fact]
        public void sdsdsdwerwer()
        {
            var hand = new List<ICard>
            {
                new Card(CardFigure._9, CardColor.heart),
                new Card(CardFigure._Queen, CardColor.heart)
            };
            var desk = new List<ICard>
            {
                new Card(CardFigure._As, CardColor.heart) ,
                new Card(CardFigure._King, CardColor.heart) ,
                new Card(CardFigure._7, CardColor.spade) ,
                new Card(CardFigure._4, CardColor.spade)
            };
            var rf = new Flush(hand, desk);
            rf.Check();
            var outs = rf.GetOuts();

            Console.WriteLine();
        }
    }
}
