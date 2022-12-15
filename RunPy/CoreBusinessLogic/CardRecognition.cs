
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CoreBusinessLogic
{
    public class CardRecognition : ICardRecognition
    {
        public Dictionary<string, CardFigure> FigureDict { get; set; }
        public Dictionary<string, CardColor> ColorDict { get; set; }

        public CardRecognition()
        {
            FigureDict = GetFigureDict();
            ColorDict = GetColorDict();
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

        public ICard GetCard(string figurePath, string colorPath, int number, string area)
        {
            //var figure = RecognizeFigure(figurePath, number);
            //var color = RecognizeColor(colorPath, number);
            RecoImage(recoType.figure, figurePath, number, area);
            RecoImage(recoType.color, colorPath, number, area);

            return new Card(new CardFigure(), new CardColor());
        }

        public CardFigure RecognizeFigure(string imagePath, int number)
        {
            var result =  RecognizeImage(recoType.figure, imagePath, number);
            return FigureDict[result];
        }

        public CardColor RecognizeColor(string imagePath, int number)
        {
            var result =  RecognizeImage(recoType.color, imagePath, number);
            return ColorDict[result];
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
                    var middleX = card[0].Replace("\r\n", "").Replace(".", ",");
                    var middleY = card[1].Replace("\r\n", "").Replace(".", ",");
                    var width = card[2].Replace("\r\n", "").Replace(".", ",");
                    var height = card[3].Replace("\r\n", "").Replace(".", ",");
                    points.Add(new Tuple<decimal, decimal, decimal, decimal>(Convert.ToDecimal(middleX), Convert.ToDecimal(middleY), Convert.ToDecimal(width), Convert.ToDecimal(height)));
                }
                catch (Exception x)
                {

                }
            }
            return points;
        }

        public Tuple<int, int, int, int> GetSingleCardArea()
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

        public string GetCardsCountOnDesk()
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

            //process.EnableRaisingEvents = true;
            process.Start();
            //var error = process
            //   .StandardError
            //   .ReadToEnd();
            var result = process
                .StandardOutput
                .ReadToEnd();
            return result.Replace("\r\n", "");
        }

        public Task<string> GetHandAsync()
        {
            var t = new TaskCompletionSource<string>(); //Using bool, because TaskCompletionSource needs at least one generic param
            string result = "";
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
            process.Exited += (object sender, EventArgs e) =>
            {
                ////TODO: Exceptions will go first, followed by `return;`
                //t.SetException();

                //TODO: Finally, if there are no problems, return successfully

                t.SetResult(process.StandardOutput.ReadToEnd().Replace("\r\n", ""));
                result = process.StandardOutput.ReadToEnd().Replace("\r\n", "");
            };
            process.Start();
            var error = process
               .StandardError
               .ReadToEnd();
            //var result = process
            //    .StandardOutput
            //    .ReadToEnd();
            return t.Task;
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
            if (result.Length == 1) return new Tuple<int, int, int, int>(0, 0, 0, 0);
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

       
        public Stream ToStream(Image image)
        {
            var stream = new MemoryStream();

            image.Save(stream, image.RawFormat);
            stream.Position = 0;

            return stream;
        }

        private string RecognizeImage(recoType rc, string imagePath, int number)
        {
            try
            {
                var t = new TaskCompletionSource<string>(); //Using bool, because TaskCompletionSource needs at least one generic param
                string result = "";
                string img = string.Empty;
                using (var image = Bitmap.FromFile(imagePath))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, image.RawFormat);
                        byte[] array = ms.ToArray();
                        img = Convert.ToBase64String(array);
                    }
                }

                Process process = new Process();
                string argument = rc == recoType.figure ?
                    $@"C:\Users\mkosi\PycharmProjects\pythonProject\predictFigure.py {img} {number}" ://{imagePath.Replace(" ", "")}" :
                    $@"C:\Users\mkosi\PycharmProjects\pythonProject\predictColor.py {img} {number}";
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
                process.Exited += (object sender, EventArgs e) =>
                {
                    ////TODO: Exceptions will go first, followed by `return;`
                    //t.SetException();

                    //TODO: Finally, if there are no problems, return successfully

                    t.SetResult(process.StandardOutput.ReadToEnd().Replace("\r\n", ""));
                    result = process.StandardOutput.ReadToEnd().Replace("\r\n", "");
                };
                process.Start();
                Debug.WriteLine($"process {process.Id} started");
                //var error = process
                //   .StandardError
                //   .ReadToEnd();
                //var result = process.StandardOutput.ReadToEnd().Split("\n");
                ////var list = result.Split("\n");
                //var ress = rc == recoType.figure ? result[3].Replace("\r", "") : result[0].Replace("\r", "");
                return "";
            }
            catch(Exception x)
            {
                return "";
            }
        }

        private async void RecoImage(recoType rc, string imagePath, int number, string area)
        {
            try
            {
                var t = new TaskCompletionSource<string>(); //Using bool, because TaskCompletionSource needs at least one generic param
                string result = "";
                string img = string.Empty;
                using (var image = Bitmap.FromFile(imagePath))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, image.RawFormat);
                        byte[] array = ms.ToArray();
                        img = Convert.ToBase64String(array);
                    }
                }

                Process process = new Process();
                string argument = rc == recoType.figure ?
                    $@"C:\Users\mkosi\PycharmProjects\pythonProject\predictFigure.py {img} {number} {area}" ://{imagePath.Replace(" ", "")}" :
                    $@"C:\Users\mkosi\PycharmProjects\pythonProject\predictColor.py {img} {number} {area}";
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
                process.Exited += (object sender, EventArgs e) =>
                {
                    t.SetResult(process.StandardOutput.ReadToEnd().Replace("\r\n", ""));
                    //result = process.StandardOutput.ReadToEnd().Replace("\r\n", "");
                    var FC = t.Task.Result.Split('|')[0];
                    var result = $"{FC}|{rc}|{number}|{area}";
                    var ff = process.StandardError.ReadToEnd();
                    //Debug.WriteLine($"{rc}|{number}|{ress}");
                    OnCardRecognised(result);
                };

                if(number%3 > 0)
                    await Task.Delay(1000);

                process.Start();
                Debug.WriteLine($"process {process.Id} started");
                //var error = process
                //   .StandardError
                //   .ReadToEnd();
                //var result = process.StandardOutput.ReadToEnd().Split("\n");
                ////var list = result.Split("\n");
                //var ress = rc == recoType.figure ? result[3].Replace("\r", "") : result[0].Replace("\r", "");
            }
            catch (Exception x)
            {
            }
        }

        public delegate void CardHandler(string cardReco);
        public event CardHandler CardRecognised;
        private void OnCardRecognised(string cardReco)
        {
            if (CardRecognised != null)
                CardRecognised(cardReco);
        }

        private string rr;

        public MemoryStream SerializeToStream(object o)
        {
            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, o);
            return stream;
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
