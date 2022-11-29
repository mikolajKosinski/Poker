
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace CoreBusinessLogic
{
    public class CardRecognition : ICardRecognition
    {
        private Dictionary<string, CardFigure> _figureDict;
        private Dictionary<string, CardColor> _colorDict;

        public CardRecognition()
        {
            _figureDict = GetFigureDict();
            _colorDict = GetColorDict();
        }

        private Dictionary<string, CardFigure> GetFigureDict()
        {
            return new Dictionary<string, CardFigure>
            {
                {"2", CardFigure._2 },
                {"3", CardFigure._3 },
                {"4", CardFigure._4 },
                {"5", CardFigure._5 },
                {"6", CardFigure._6 },
                {"7", CardFigure._7 },
                {"8", CardFigure._8 },
                {"9", CardFigure._9 },
                {"10", CardFigure._10 },
                {"j", CardFigure._Jack },
                {"q", CardFigure._Queen },
                {"k", CardFigure._King },
                {"a", CardFigure._As }
            };
        }

        private Dictionary<string, CardColor> GetColorDict()
        {
            return new Dictionary<string, CardColor>
            {
                {"C", CardColor.club },
                {"D", CardColor.diamond },
                {"H", CardColor.heart },
                {"S", CardColor.spade }
            };
        }

        public ICard GetCard(string figurePath, string colorPath)
        {
            var figure = RecognizeFigure(figurePath);
            var color = RecognizeColor(colorPath);            
            return new Card(figure, color);
        }

        private CardFigure RecognizeFigure(string imagePath)
        {
            var result = RecognizeImage(recoType.figure, imagePath);
            return _figureDict[result];
        }

        private CardColor RecognizeColor(string imagePath)
        {
            var result = RecognizeImage(recoType.color, imagePath);
            return _colorDict[result];            
        }

        public Tuple<int, int, int, int> GetPosition(string path)
        {
            return GetShapePosition(path);
        }

        public List<Tuple<decimal, decimal, decimal, decimal>> GetDesk()
        {
            Process process = new Process();
            string argument = @"C:\Users\Mikolaj\PycharmProjects\pythonProject\GetDeskPoints.py";
            process.StartInfo = new System.Diagnostics.ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = @"C:\Users\Mikolaj\PycharmProjects\pythonProject\venv\Scripts\python.exe",
                Arguments = argument,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            process.EnableRaisingEvents = true;
            process.Start();
            var error = process
                .StandardError
                .ReadToEnd();
            var result = process.StandardOutput.ReadToEnd();
            var cards = result.Split('#');
            var points = new List<Tuple<decimal, decimal, decimal, decimal>>();
            foreach (var item in cards)
            {
                try
                {
                    var card = item.Split('-');
                    var middleX = card[0].Replace("\r\n", "").Replace(".",",");
                    var middleY = card[1].Replace("\r\n", "").Replace(".", ",");
                    var width = card[2].Replace("\r\n", "").Replace(".", ",");
                    var height = card[3].Replace("\r\n", "").Replace(".", ",");
                    points.Add(new Tuple<decimal, decimal, decimal, decimal>(Convert.ToDecimal(middleX),Convert.ToDecimal( middleY), Convert.ToDecimal(width), Convert.ToDecimal(height)));
                }
                catch (Exception x)
                {

                }
            }
            return points;
        }

        public Tuple<int,int,int,int> GetSingleCardArea()
        {
            Process process = new Process();
            string argument = @"C:\Users\Mikolaj\PycharmProjects\pythonProject\externals.py";
            process.StartInfo = new System.Diagnostics.ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = @"C:\Users\Mikolaj\PycharmProjects\pythonProject\venv\Scripts\python.exe",
                Arguments = argument,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            process.EnableRaisingEvents = true;
            process.Start();
            var error = process
              .StandardError
              .ReadToEnd();
            var result = process
                .StandardOutput
                .ReadToEnd()
                .Replace("\t", "")
                .Replace("\r", "")
                .Replace("\n", "");
            var points = result.Split(',');
            return new Tuple<int, int, int, int>(
                Convert.ToInt32(points[0]), 
                Convert.ToInt32(points[1]), 
                Convert.ToInt32(points[2]), 
                Convert.ToInt32(points[3]));
        }

        public Tuple<int, int, int, int> GetArea()
        {
            Process process = new Process();
            string argument = @"C:\Users\mkosi\PycharmProjects\pythonProject\GetArea.py";
            process.StartInfo = new System.Diagnostics.ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = @"C:\Users\Mikolaj\PycharmProjects\pythonProject\venv\Scripts\python.exe",
                Arguments = argument,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            process.EnableRaisingEvents = true;
            process.Start();
            var error = process
               .StandardError
               .ReadToEnd();
            var result = process
                .StandardOutput
                .ReadToEnd()
                .Replace("\t", "")
                .Replace("\r", "")
                .Replace("\n", "");
            var points = result.Split(',');
            return new Tuple<int, int, int, int>(
                Convert.ToInt32(points[0]),
                Convert.ToInt32(points[1]),
                Convert.ToInt32(points[2]),
                Convert.ToInt32(points[3]));

        }

        public string GetAllCards(string fileName)
        {
            Process process = new Process();
            string script = @"C:\Users\mkosi\PycharmProjects\pythonProject\cardScreen.py";
            process.StartInfo = new System.Diagnostics.ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = @"C:\Users\mkosi\PycharmProjects\pythonProject\venv\Scripts\python.exe",
                Arguments = $"{script} {fileName}",
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            process.EnableRaisingEvents = true;
            process.Start();
            var error = process
               .StandardError
               .ReadToEnd();
            var result = process
                .StandardOutput
                .ReadToEnd();
            return result;
        }

        public string GetColorFigure(int cardsCount, string cardName)
        {
            Process process = new Process();
            string argument = @$"C:\Users\mkosi\PycharmProjects\pythonProject\FC.py";
            process.StartInfo = new System.Diagnostics.ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = @"C:\Users\mkosi\PycharmProjects\pythonProject\venv\Scripts\python.exe",
                Arguments = string.Format("{0} {1} {2}", argument, cardsCount, cardName),
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            process.EnableRaisingEvents = true;
            process.Start();
            var error = process
               .StandardError
               .ReadToEnd();
            var result = process
                .StandardOutput
                .ReadToEnd();
            return result;
        }

        public string GetHand()
        {
            Process process = new Process();
            string argument = @$"C:\Users\mkosi\PycharmProjects\pythonProject\predictFigures.py";
            process.StartInfo = new System.Diagnostics.ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = @"C:\Users\mkosi\PycharmProjects\pythonProject\venv\Scripts\python.exe",
                Arguments = string.Format("{0}", argument),
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            process.EnableRaisingEvents = true;
            process.Start();
            var error = process
               .StandardError
               .ReadToEnd();
            var result = process
                .StandardOutput
                .ReadToEnd();
            return result.Replace("\r\n","");
        }

        private Tuple<int, int, int, int> GetShapePosition(string path)
        {
            Process process = new Process();
            string argument = $@"C:\Users\Mikolaj\PycharmProjects\pythonProject1\Segmentation.py {path}";
            process.StartInfo = new System.Diagnostics.ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = @"C:\Users\Mikolaj\AppData\Local\Programs\Python\Python39\python.exe",
                Arguments = argument,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            process.Start();
            var result = process.StandardOutput.ReadToEnd().Split('-');
            if (result.Length == 1) return new Tuple<int, int, int, int>(0,0,0,0);
            return new Tuple<int, int, int, int>(
                Convert.ToInt32(result[0]), 
                Convert.ToInt32(result[1]), 
                Convert.ToInt32(result[2]), 
                Convert.ToInt32(result[3]));
        }

        public string CenterFigure(string path)
        {
            Process process = new Process();
            string argument = $@"C:\Users\Mikolaj\PycharmProjects\pythonProject1\CenterFigure.py {path}";
            process.StartInfo = new System.Diagnostics.ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = @"C:\Users\Mikolaj\AppData\Local\Programs\Python\Python39\python.exe",
                Arguments = argument,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            process.Start();
            var result = process.StandardOutput.ReadToEnd();
            return result.Replace("\t", "").Replace("\r", "").Replace("\n", "");
        }

        private string RecognizeImage(recoType rc, string imagePath)
        {
            Process process = new Process();
            string argument = rc == recoType.figure ?
                $@"C:\Users\mkosi\PycharmProjects\pythonProject\predictFigure.py {imagePath.Replace(" ", "")}" :
                $@"C:\Users\mkosi\PycharmProjects\pythonProject\predictColor.py {imagePath.Replace(" ", "")}";
            process.StartInfo = new System.Diagnostics.ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = @"C:\Users\mkosi\PycharmProjects\pythonProject\venv\Scripts\python.exe",
                Arguments = argument,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
                        
            process.Start();
            var error = process
               .StandardError
               .ReadToEnd();
            var result = process.StandardOutput.ReadToEnd();
            var list = result.Split("\n");
            return list[3].Replace("\r", "");
        }

        public string RecogniseByPath(string path)
        {
            return "";
        }

        private enum recoType
        {
            figure,
            color
        }
    }
}
