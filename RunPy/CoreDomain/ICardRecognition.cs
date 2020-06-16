using System;
using System.Collections.Generic;
using System.Text;

namespace CoreDomain
{
    public interface ICardRecognition
    {
        string RecogniseByPath(string path);
    }
}
