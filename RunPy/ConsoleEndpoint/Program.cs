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

            var card = manager.GetCardByImage("C:\\Users\\mkosi\\PycharmProjects\\tensorEnv\\dataset\\2C\\test.jpg");
            
            Console.WriteLine();
        }
    }
}
