using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace CoreBusinessLogic
{
    public class Settings : ISettings
    {
        public IList<string> CountingSystemsList { get; set; }
        public string SelectedFormula { get; set; }

        public Settings()
        {
            //if (!File.Exists("settings.xml"))
            //    File.Create("settings.xml");

            //XDocument doc = XDocument.Load("settings.xml");

            //foreach (var img in doc.Descendants("Settings"))
            //{
            //    // src will be null if the attribute is missing
            //    string src = (string)img.Attribute("src");
            //    img.SetAttributeValue("src", src + "with-changes");
            //}
        }
    }
}
