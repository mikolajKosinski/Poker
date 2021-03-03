using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBusinessLogic
{
    public interface ICardRecognition
    {
        string RecogniseByPath(string path);
    }
}
