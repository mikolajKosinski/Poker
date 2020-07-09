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
            Console.WriteLine();
            return null;// cardsDict[card];
        }

        private Dictionary<string, Card> GetCardsDict()
        {
            var dict = new Dictionary<string, Card>();           

            dict.Add("2C", new Card("2C"));
            dict.Add("3C", new Card("3C"));
            dict.Add("4C", new Card("4C"));
            dict.Add("5C", new Card("5C"));
            dict.Add("6C", new Card("6C"));
            dict.Add("7C", new Card("7C"));
            dict.Add("8C", new Card("8C"));
            dict.Add("9C", new Card("9C"));
            dict.Add("10C", new Card("10C"));
            dict.Add("JC", new Card("JC"));
            dict.Add("QC", new Card("QC"));
            dict.Add("KC", new Card("KC"));
            dict.Add("AC", new Card("AC"));

            dict.Add("2D", new Card("2D"));
            dict.Add("3D", new Card("3D"));
            dict.Add("4D", new Card("4D"));
            dict.Add("5D", new Card("5D"));
            dict.Add("6D", new Card("6D"));
            dict.Add("7D", new Card("7D"));
            dict.Add("8D", new Card("8D"));
            dict.Add("9D", new Card("9D"));
            dict.Add("10D", new Card("10D"));
            dict.Add("JD", new Card("JD"));
            dict.Add("QD", new Card("QD"));
            dict.Add("KD", new Card("KD"));
            dict.Add("AD", new Card("AD"));

            dict.Add("2H", new Card("2H"));
            dict.Add("3H", new Card("3H"));
            dict.Add("4H", new Card("4H"));
            dict.Add("5h", new Card("5H"));
            dict.Add("6h", new Card("6H"));
            dict.Add("7h", new Card("7H"));
            dict.Add("8h", new Card("8H"));
            dict.Add("9h", new Card("9H"));
            dict.Add("10H", new Card("10H"));
            dict.Add("JH", new Card("JH"));
            dict.Add("QH", new Card("QH"));
            dict.Add("KH", new Card("KH"));
            dict.Add("AH", new Card("AH"));

            dict.Add("2S", new Card("2S"));
            dict.Add("3S", new Card("3S"));
            dict.Add("4S", new Card("4S"));
            dict.Add("5S", new Card("5S"));
            dict.Add("6S", new Card("6S"));
            dict.Add("7S", new Card("7S"));
            dict.Add("8S", new Card("8S"));
            dict.Add("9S", new Card("9S"));
            dict.Add("10S", new Card("10S"));
            dict.Add("JS", new Card("JS"));
            dict.Add("QS", new Card("QS"));
            dict.Add("KS", new Card("KS"));
            dict.Add("AS", new Card("AS"));

            return dict;
        }
    }
}
