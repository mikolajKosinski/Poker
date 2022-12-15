using System;
using System.Collections.Generic;
using System.Text;

namespace WpfClient
{
    public interface ILoggerWrapper
    {
        void Info(string message);
        void Error(Exception x);
    }
}
