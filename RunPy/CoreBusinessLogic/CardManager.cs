using CoreDomain;
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
            return cardsDict[card];
        }

        private Dictionary<string, Card> GetCardsDict()
        {
            var dict = new Dictionary<string, Card>();           

            dict.Add("2C", new Card("2C"));
            dict.Add("3C", new Card("2C"));
            dict.Add("4C", new Card("2C"));
            dict.Add("5C", new Card("2C"));
            dict.Add("6C", new Card("2C"));
            dict.Add("7C", new Card("2C"));
            dict.Add("8C", new Card("2C"));
            dict.Add("9C", new Card("2C"));
            dict.Add("10C", new Card("2C"));
            dict.Add("JC", new Card("2C"));
            dict.Add("QC", new Card("2C"));
            dict.Add("KC", new Card("2C"));
            dict.Add("AC", new Card("2C"));

            dict.Add("2D", new Card("2C"));
            dict.Add("3D", new Card("2C"));
            dict.Add("4D", new Card("2C"));
            dict.Add("5D", new Card("2C"));
            dict.Add("6D", new Card("2C"));
            dict.Add("7D", new Card("2C"));
            dict.Add("8D", new Card("2C"));
            dict.Add("9D", new Card("2C"));
            dict.Add("10D", new Card("2C"));
            dict.Add("JD", new Card("2C"));
            dict.Add("QD", new Card("2C"));
            dict.Add("KD", new Card("2C"));
            dict.Add("AD", new Card("2C"));

            dict.Add("2H", new Card("2C"));
            dict.Add("3H", new Card("2C"));
            dict.Add("4H", new Card("2C"));
            dict.Add("5h", new Card("2C"));
            dict.Add("6h", new Card("2C"));
            dict.Add("7h", new Card("2C"));
            dict.Add("8h", new Card("2C"));
            dict.Add("9h", new Card("2C"));
            dict.Add("10H", new Card("2C"));
            dict.Add("JH", new Card("2C"));
            dict.Add("Qh", new Card("2C"));
            dict.Add("Kh", new Card("2C"));
            dict.Add("Ah", new Card("2C"));

            dict.Add("2S", new Card("2C"));
            dict.Add("3S", new Card("2C"));
            dict.Add("4S", new Card("2C"));
            dict.Add("5S", new Card("2C"));
            dict.Add("6S", new Card("2C"));
            dict.Add("7S", new Card("2C"));
            dict.Add("8S", new Card("2C"));
            dict.Add("9S", new Card("2C"));
            dict.Add("10S", new Card("2C"));
            dict.Add("JS", new Card("2C"));
            dict.Add("QS", new Card("2C"));
            dict.Add("KS", new Card("2C"));
            dict.Add("AS", new Card("2C"));

            return dict;
        }
    }
}
