using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBusinessLogic
{
    public interface ISettings
    {
        IList<string> CountingSystemsList { get; set; }
        string SelectedFormula { get; set; }
    }
}
