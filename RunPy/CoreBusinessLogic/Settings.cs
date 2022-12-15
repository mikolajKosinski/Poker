using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace CoreBusinessLogic
{
    public class Settings : ISettings
    {
        public IList<string> CountingSystemsList { get; set; }
        public string SelectedFormula { get; set; }
        public string SelectedThreshold { get; set; }

        public Settings()
        {
            if (!File.Exists("settings.xml"))
                CreateSettingsFile();

            LoadSettings();
        }

        private void CreateSettingsFile()
        {
            XDocument doc =
                 new XDocument(
                     new XElement("Settings",
                     new XElement("Formula", "algebraic"),
                     new XElement("Threshold", "10")
                     ));
            doc.Save("settings.xml");
        }

        private void LoadSettings()
        {
            XDocument xDoc = XDocument.Load("settings.xml");
            foreach (var node in xDoc.DescendantNodes().OfType<XText>())
            {
                var value = node.Value.Trim();

                if (!string.IsNullOrEmpty(value))
                {
                    if (node.Parent.Name == "Formula")
                        SelectedFormula = value;

                    if (node.Parent.Name == "Threshold")
                        SelectedThreshold = value;
                }
            }
        }
    }
}
