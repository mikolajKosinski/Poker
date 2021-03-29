using CoreBusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace WpfClient
{
    public class CardRecognitionEventArgs : EventArgs
    {
        public ICard Card { get; private set; }

        public CardRecognitionEventArgs(ICard card)
        {
            Card = card;
        }
    }
}
