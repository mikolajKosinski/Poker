using CoreBusinessLogic;
using CoreDomain;
using System;

namespace ConsoleEndpoint
{
    class Program
    {
        static void Main(string[] args)
        {
            ICardRecognition cardReco = new CardRecognition();
            ICardManager manager = new CardManager(cardReco);
            IFigureMatcher matcher = new FigureMatcher();

            matcher.AddCardToFlop("3C");

            matcher.AddCardToHand("QS");
            matcher.AddCardToHand("KS");

            var card = manager.GetCardByImage("C:\\Users\\mkosi\\PycharmProjects\\tensorEnv\\dataset\\2C\\test.jpg");

            var res = matcher.CheckHand();
            Console.WriteLine();
            
        }
    }
}
