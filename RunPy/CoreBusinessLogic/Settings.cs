using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBusinessLogic
{
    public class Settings : ISettings
    {
        public IList<string> CountingSystemsList { get; set; }
        public string SelectedFormula { get; set; }
    }
}
