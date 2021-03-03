using System;
using System.Collections.Generic;

namespace CoreBusinessLogic
{
    public class CardManager : ICardManager
    {
        readonly Dictionary<string, Card> cardsDict;
        readonly ICardRecognition cardRecognition;

        public CardManager(ICardRecognition cardRecognition)
        {
            this.cardRecognition = cardRecognition;
            this.cardsDict = GetCardsDict();
        }

        public Card GetCardByImage(string path)
        {
            var card = cardRecognition.RecogniseByPath(path);
            Console.WriteLine();
            return null;// cardsDict[card];
        }

        private Dictionary<string, Card> GetCardsDict()
        {
            var dict = new Dictionary<string, Card>();           

            dict.Add("2C", new Card(CardFigure._2, CardColor.club));
            dict.Add("3C", new Card(CardFigure._3, CardColor.club));
            dict.Add("4C", new Card(CardFigure._4, CardColor.club));
            dict.Add("5C", new Card(CardFigure._5, CardColor.club));
            dict.Add("6C", new Card(CardFigure._6, CardColor.club));
            dict.Add("7C", new Card(CardFigure._7, CardColor.club));
            dict.Add("8C", new Card(CardFigure._8, CardColor.club));
            dict.Add("9C", new Card(CardFigure._9, CardColor.club));
            dict.Add("10C", new Card(CardFigure._10, CardColor.club));
            dict.Add("JC", new Card(CardFigure._Jack, CardColor.club));
            dict.Add("QC", new Card(CardFigure._Queen, CardColor.club));
            dict.Add("KC", new Card(CardFigure._King, CardColor.club));
            dict.Add("AC", new Card(CardFigure._As, CardColor.club));

            dict.Add("2D", new Card(CardFigure._2, CardColor.diamond));
            dict.Add("3D", new Card(CardFigure._3, CardColor.diamond));
            dict.Add("4D", new Card(CardFigure._4, CardColor.diamond));
            dict.Add("5D", new Card(CardFigure._5, CardColor.diamond));
            dict.Add("6D", new Card(CardFigure._6, CardColor.diamond));
            dict.Add("7D", new Card(CardFigure._7, CardColor.diamond));
            dict.Add("8D", new Card(CardFigure._8, CardColor.diamond));
            dict.Add("9D", new Card(CardFigure._9, CardColor.diamond));
            dict.Add("10D", new Card(CardFigure._10, CardColor.diamond));
            dict.Add("JD", new Card(CardFigure._Jack, CardColor.diamond));
            dict.Add("QD", new Card(CardFigure._Queen, CardColor.diamond));
            dict.Add("KD", new Card(CardFigure._King, CardColor.diamond));
            dict.Add("AD", new Card(CardFigure._As, CardColor.diamond));

            dict.Add("2H", new Card(CardFigure._2, CardColor.heart));
            dict.Add("3H", new Card(CardFigure._3, CardColor.heart));
            dict.Add("4H", new Card(CardFigure._4, CardColor.heart));
            dict.Add("5h", new Card(CardFigure._5, CardColor.heart));
            dict.Add("6h", new Card(CardFigure._6, CardColor.heart));
            dict.Add("7h", new Card(CardFigure._7, CardColor.heart));
            dict.Add("8h", new Card(CardFigure._8, CardColor.heart));
            dict.Add("9h", new Card(CardFigure._9, CardColor.heart));
            dict.Add("10H", new Card(CardFigure._10, CardColor.heart));
            dict.Add("JH", new Card(CardFigure._Jack, CardColor.heart));
            dict.Add("QH", new Card(CardFigure._Queen, CardColor.heart));
            dict.Add("KH", new Card(CardFigure._King, CardColor.heart));
            dict.Add("AH", new Card(CardFigure._As, CardColor.heart));

            dict.Add("2S", new Card(CardFigure._2, CardColor.spade));
            dict.Add("3S", new Card(CardFigure._3, CardColor.spade));
            dict.Add("4S", new Card(CardFigure._4, CardColor.spade));
            dict.Add("5S", new Card(CardFigure._5, CardColor.spade));
            dict.Add("6S", new Card(CardFigure._6, CardColor.spade));
            dict.Add("7S", new Card(CardFigure._7, CardColor.spade));
            dict.Add("8S", new Card(CardFigure._8, CardColor.spade));
            dict.Add("9S", new Card(CardFigure._9, CardColor.spade));
            dict.Add("10S", new Card(CardFigure._10, CardColor.spade));
            dict.Add("JS", new Card(CardFigure._Jack, CardColor.spade));
            dict.Add("QS", new Card(CardFigure._Queen, CardColor.spade));
            dict.Add("KS", new Card(CardFigure._King, CardColor.spade));
            dict.Add("AS", new Card(CardFigure._As, CardColor.spade));

            return dict;
        }
    }
}
