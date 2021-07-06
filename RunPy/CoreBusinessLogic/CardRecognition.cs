﻿
using System;
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
                {"c", CardColor.club },
                {"d", CardColor.diamond },
                {"h", CardColor.heart },
                {"s", CardColor.spade }
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

        public Tuple<int,int,int,int> GetSingleCardArea()
        {
            Process process = new Process();
            string argument = @"C:\Users\Mikolaj\PycharmProjects\pythonProject1\externals.py";
            process.StartInfo = new System.Diagnostics.ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = @"C:\Users\Mikolaj\AppData\Local\Programs\Python\Python37\python.exe",
                Arguments = argument,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            process.EnableRaisingEvents = true;
            process.Start();
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
            string argument = @"C:\Users\Mikolaj\PycharmProjects\pythonProject1\GetArea.py";
            process.StartInfo = new System.Diagnostics.ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = @"C:\Users\Mikolaj\AppData\Local\Programs\Python\Python37\python.exe",
                Arguments = argument,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            process.EnableRaisingEvents = true;
            process.Start();
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

        private Tuple<int, int, int, int> GetShapePosition(string path)
        {
            Process process = new Process();
            string argument = $@"C:\Users\Mikolaj\PycharmProjects\pythonProject1\Segmentation.py {path}";
            process.StartInfo = new System.Diagnostics.ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = @"C:\Users\Mikolaj\AppData\Local\Programs\Python\Python37\python.exe",
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
                FileName = @"C:\Users\Mikolaj\AppData\Local\Programs\Python\Python37\python.exe",
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
                $@"C:\Users\Mikolaj\PycharmProjects\pythonProject1\PredictFigure.py {imagePath}" :
                $@"C:\Users\Mikolaj\PycharmProjects\pythonProject1\PredictColor.py {imagePath}";
            process.StartInfo = new System.Diagnostics.ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = @"C:\Users\Mikolaj\AppData\Local\Programs\Python\Python37\python.exe",
                Arguments = argument,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
                        
            process.Start();
            var result = process.StandardOutput.ReadToEnd();
            return result.Replace("\t", "").Replace("\r", "").Replace("\n", "");
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
