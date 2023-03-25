
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
using System.Net.Http;
using static CoreBusinessLogic.Enums;
using System.Net.Http.Headers;
using System.Text;
using System.Linq;
using static System.Net.WebRequestMethods;
using Azure.Storage.Blobs;
using static System.Net.Mime.MediaTypeNames;
using static System.Collections.Specialized.BitVector32;
using System.ComponentModel.DataAnnotations;

namespace CoreBusinessLogic
{
    public class CardRecognition : ICardRecognition
    {
        List<Tuple<double, double, double, double>> cardsPositions;
        Stage cardStage { get; set; }

        public Dictionary<string, CardFigure> FigureDict { get; set; }
        public Dictionary<string, CardColor> ColorDict { get; set; }

        public CardRecognition()
        {
            FigureDict = GetFigureDict();
            ColorDict = GetColorDict();
            cardsPositions = new List<Tuple<double, double, double, double>>();
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

        public async Task<ICard> GetCard(string figurePath, string colorPath, int number, AnalyzeArea area, Stage stage)
        {
            //var figure = RecognizeFigure(figurePath, number);
            //var color = RecognizeColor(colorPath, number);
            //RecoImage(recoType.figure, figurePath, number, area);
            //RecoImage(recoType.color, colorPath, number, area);
            //var path = @"C:\Users\mkosi\Documents\GitHub\Poker\RunPy\WpfClient\obj\Debug\net5.0-windows\C1.PNG";
            
            //await UploadImageToBlob(figurePath);
            PredictCard(RandomString(10), figurePath, area, recoType.figure, number.ToString(), stage);
            PredictCard(RandomString(10), colorPath, area, recoType.color, number.ToString(), stage);
            return new Card(new CardFigure(), new CardColor());
        }

        public async Task UploadImageToBlob(string imagePath, string filename, Stage stage)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=detectimages;AccountKey=SprSVOOvXy65WHaCgRU1tS5NHvYHkEp5srNTEmyuudUpLmf38MHo7SQzlOmgfdnwVf5J9O40hYB2+AStVljy8Q==;EndpointSuffix=core.windows.net";
            string containerName = "images";
            string blobName = filename;
            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
            BlobClient blob = container.GetBlobClient(blobName);
            var leftEdge = GetStartingPointToCutOff(stage);

            try
            {
                using (var image = Bitmap.FromFile(imagePath))
                {
                    var startingPoint = leftEdge != 0 ? image.Width * leftEdge : 0;
                    Rectangle section = Rectangle.Round(new RectangleF(startingPoint, 0,(float)image.Width, (float)image.Height));
                    Bitmap CroppedImage = CropImage(new Bitmap(image), section);

                    using (var ms = new MemoryStream())
                    {
                        ((System.Drawing.Image)CroppedImage).Save("test.jpg");
                        image.Save(ms, image.RawFormat);
                        ms.Position= 0;
                        blob.Upload(ms, true);
                        var urib = blob.Uri;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public Bitmap CropImage(Bitmap source, Rectangle section)
        {
            var bitmap = new Bitmap(section.Width, section.Height);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);
                return bitmap;
            }
        }

        private float GetStartingPointToCutOff(Stage stage)
        {
            if (cardsPositions.Count == 2) return 0;

            var bufforToCutOff = 5;
            float leftPoint = 0;
            var cardInOrder = cardsPositions.OrderBy(x => x.Item1).ToList();

            if(stage ==Stage.Turn) 
            {
                leftPoint = (float)cardInOrder[3].Item1 - (float)(cardInOrder[3].Item1 * 0.05);
            }

            return leftPoint;
        }

        private string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //private void handdesk()
        //{
        //    string hand = string.Empty;
        //    using (var image = Bitmap.FromFile(@"C:\Users\mkosi\Documents\GitHub\Poker\RunPy\WpfClient\obj\Debug\net5.0-windows\hand.png"))
        //    {
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            image.Save(ms, image.RawFormat);
        //            byte[] array = ms.ToArray();
        //            hand = Convert.ToBase64String(array);
        //        }
        //    }

        //    string desk = string.Empty;
        //    using (var image = Bitmap.FromFile(@"C:\Users\mkosi\Documents\GitHub\Poker\RunPy\WpfClient\obj\Debug\net5.0-windows\allCards.png"))
        //    {
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            image.Save(ms, image.RawFormat);
        //            byte[] array = ms.ToArray();
        //            desk = Convert.ToBase64String(array);
        //        }
        //    }

        //    Console.WriteLine();
        //}

        public async Task PredictCard(string fileName, string imagePath, AnalyzeArea area, recoType fc, string number, Stage stage)
        {
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

            var runId = await RunPredictionJobForCard(fileName, img);
            await IsPredictionFinished(runId);
            var predicted = await ReadPredictionResult(fileName);
            var response = $"{predicted}||{fc}|{number}";
            OnCardRecognised(response, area, cardStage);
            DeletePrediction($"{fileName}.txt");
            DeletePrediction($"{fileName}.PNG");
        }

        public async Task<int> DetectCard(string imagePath, AnalyzeArea area, Stage stage)
        {
            //if(area == AnalyzeArea.Desk && cardsPositions.Any()) return CutIntoParts("", imagePath);

            var nameForBlob = RandomString(10)+".PNG";
            var nameForPrediction = RandomString(10);
            
            await UploadImageToBlob(imagePath, nameForBlob, stage);            

            var runId = await RunDetectionJobForCard(nameForPrediction, nameForBlob);
            await IsPredictionFinished(runId);
            var predicted = await ReadPredictionResult(nameForPrediction);
            var count = CutIntoParts(predicted, imagePath);

            DeletePrediction($"{nameForBlob}.txt");
            DeletePrediction($"{nameForBlob}.PNG");
            return count;
        }

        private int CutIntoParts(string predicted, string imgPath)
        {
            var cords = GetParts(predicted).OrderBy(x => x.Item1).ToList();
            var bm = new Bitmap(imgPath);
            
            for(int q=0; q < cords.Count; q++) 
            {
                var left = Convert.ToInt32(bm.Width * cords[q].Item1);
                var top = Convert.ToInt32(bm.Height * cords[q].Item2);
                var width = Convert.ToInt32(bm.Width * cords[q].Item3);
                var height = Convert.ToInt32(bm.Height * cords[q].Item4);

                var fig = bm.Clone(new Rectangle(left, top, width, height), bm.PixelFormat);
                fig.Save(@$"C:\Users\mkosi\Documents\GitHub\Poker\RunPy\WpfClient\obj\Debug\net5.0-windows\F{q}.PNG");

                int heightC = Convert.ToInt32(height * 0.7);
                var color = bm.Clone(new Rectangle(left, top + height, width, heightC), bm.PixelFormat);
                color.Save(@$"C:\Users\mkosi\Documents\GitHub\Poker\RunPy\WpfClient\obj\Debug\net5.0-windows\C{q}.PNG");

                if(cords.Count == 3 && cardsPositions.Count < 3)
                {
                    cardsPositions.Add(new Tuple<double, double, double, double>(cords[q].Item1, cords[q].Item2, cords[q].Item3, cords[q].Item4));
                    cardStage = Stage.Flop;
                }
                if (cords.Count == 4 && cardsPositions.Count < 4 && q == 3)
                {
                    cardsPositions.Add(new Tuple<double, double, double, double>(cords[q].Item1, cords[q].Item2, cords[q].Item3, cords[q].Item4));
                    cardStage = Stage.Turn;
                }
                if (cords.Count == 5 && cardsPositions.Count < 5 && q == 4)
                {
                    cardsPositions.Add(new Tuple<double, double, double, double>(cords[q].Item1, cords[q].Item2, cords[q].Item3, cords[q].Item4));
                    cardStage = Stage.River;
                }
            }
            return cords.Count;
        }

        private List<Tuple<double, double, double, double>> GetParts(string predicted = "")
        {
            //if(cardsPositions.Any()) return cardsPositions;

            var split = predicted.Split("probability").Where(p => p.Length > 4).ToList();
            var fcList = new List<Tuple<double, double, double, double>>();
            try
            {
                foreach (var item in split)
                {
                    var prob = Convert.ToDouble(item.Substring(2, 4).Replace(".",","));
                    if (prob < 0.30000)
                        continue;

                    var leftIdx = item.IndexOf("left");
                    var topIdx = item.IndexOf("top");
                    var widthIdx = item.IndexOf("width");
                    var heightIdx = item.IndexOf("height");

                    var left = Clean(item.Substring(leftIdx + 7, 10).Replace('.', ','));
                    var top = Clean(item.Substring(topIdx + 6, 10).Replace('.', ','));
                    var width = Clean(item.Substring(widthIdx + 8, 10).Replace('.', ','));
                    var height = Clean(item.Substring(heightIdx + 9, 10).Replace('.', ','));

                    var leftD = Convert.ToDouble(left);
                    var topD = Convert.ToDouble(top);
                    var widthD = Convert.ToDouble(width);
                    var heightD = Convert.ToDouble(height);

                    fcList.Add(new Tuple<double, double, double, double>(leftD, topD, widthD, heightD));
                }
            }
            catch(Exception x)
            {
                
            }

            return fcList;
        }

        private string Clean(string val)
        {
            var result = val;

            if(val.Count(f => f == ',') > 1)
            {
                var lastIndex = val.LastIndexOf(",");
                result = val.Substring(0, lastIndex);
            }

            if (result.Contains("}"))
            {
                var idx = result.IndexOf("}");
                result = result.Substring(0, idx);
            }

            return result;
        }

        public async Task<string> ReadPredictionResult(string fileName)
        {
            HttpClient client = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://adb-4958543948294936.16.azuredatabricks.net/api/2.0/dbfs/read");
            request.Headers.Add("Authorization", "Bearer dapie16ca829366e80ba514e77c6d7aeee6c-2");
            request.Content = new StringContent("{ \"path\": \"/FileStore/tables/"+fileName+".txt\" }");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            string message3 = responseBody.Split(":")[2].Replace("\"", "").Replace("}", "");
            var result = (string)message3;
            byte[] data = Convert.FromBase64String(result);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;            
        }

        public async Task<bool> IsPredictionFinished(string runId)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://adb-4958543948294936.16.azuredatabricks.net/api/2.0/jobs/runs/get-output?run_id="+runId);
            request.Headers.Add("Authorization", "Bearer dapie16ca829366e80ba514e77c6d7aeee6c-2");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = (string)responseBody;
            if (result.Contains("TERMINATED") && result.Contains("SUCCESS"))
                Console.WriteLine("Success");

            while (!result.Contains("SUCCESS"))
            {
                await Task.Delay(1000);
                client = new HttpClient();
                request = new HttpRequestMessage(HttpMethod.Get, "https://adb-4958543948294936.16.azuredatabricks.net/api/2.0/jobs/runs/get-output?run_id=" + runId);
                request.Headers.Add("Authorization", "Bearer dapie16ca829366e80ba514e77c6d7aeee6c-2");
                response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
                result = (string)responseBody;
            }
            return true;
        }

        public async Task<string> RunPredictionJobForCard(string fileName, string imageContent)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://adb-4958543948294936.16.azuredatabricks.net/api/2.0/jobs/run-now");
            request.Headers.Add("Authorization", "Bearer dapie16ca829366e80ba514e77c6d7aeee6c-2");
            var img = $"{imageContent}";           
            request.Content = new StringContent("{ \"job_id\": \"29718829608156\",\"notebook_params\":{\"name\":\"pred\",\"filename\":\""+fileName+"\",\"content\":\""+img+"\"} }");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var runId = responseBody.ToString().Split(',')[0].Split(':')[1];
            return runId;
        }

        public async Task<string> RunDetectionJobForCard(string fileName, string imgToPred)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://adb-4958543948294936.16.azuredatabricks.net/api/2.0/jobs/run-now");
            request.Headers.Add("Authorization", "Bearer dapie16ca829366e80ba514e77c6d7aeee6c-2");
            
            request.Content = new StringContent("{ \"job_id\": \"726745793126357\",\"notebook_params\":{\"name\":\"pred\",\"filename\":\""+fileName+"\",\"content\":\""+imgToPred+"\"} }");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var runId = responseBody.ToString().Split(',')[0].Split(':')[1];
            return runId;
        }

        public void DeletePrediction(string fileName)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://adb-4958543948294936.16.azuredatabricks.net/api/2.0/jobs/run-now");
            request.Headers.Add("Authorization", "Bearer dapie16ca829366e80ba514e77c6d7aeee6c-2");
            request.Content = new StringContent("{ \"job_id\": \"854896764317231\",\"notebook_params\":{\"name\":\"" + fileName + "\"} }");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            client.SendAsync(request);
        }

        //public CardFigure RecognizeFigure(string imagePath, int number)
        //{
        //    var result =  RecognizeImage(recoType.figure, imagePath, number);
        //    return FigureDict[result];
        //}

        //public CardColor RecognizeColor(string imagePath, int number)
        //{
        //    var result =  RecognizeImage(recoType.color, imagePath, number);
        //    return ColorDict[result];
        //}

        //public Tuple<int, int, int, int> GetPosition(string path)
        //{
        //    return GetShapePosition(path);
        //}

        //public List<Tuple<decimal, decimal, decimal, decimal>> GetDesk()
        //{
        //    Process process = new Process();
        //    string argument = @"C:\Users\Mikolaj\PycharmProjects\pythonProject\GetDeskPoints.py";
        //    process.StartInfo = new System.Diagnostics.ProcessStartInfo()
        //    {
        //        UseShellExecute = false,
        //        CreateNoWindow = true,
        //        WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
        //        FileName = @"C:\Users\Mikolaj\PycharmProjects\pythonProject\venv\Scripts\python.exe",
        //        Arguments = argument,
        //        RedirectStandardError = true,
        //        RedirectStandardOutput = true
        //    };

        //    process.EnableRaisingEvents = true;
        //    process.Start();
        //    var error = process
        //        .StandardError
        //        .ReadToEnd();
        //    var result = process.StandardOutput.ReadToEnd();
        //    var cards = result.Split('#');
        //    var points = new List<Tuple<decimal, decimal, decimal, decimal>>();
        //    foreach (var item in cards)
        //    {
        //        try
        //        {
        //            var card = item.Split('-');
        //            var middleX = card[0].Replace("\r\n", "").Replace(".", ",");
        //            var middleY = card[1].Replace("\r\n", "").Replace(".", ",");
        //            var width = card[2].Replace("\r\n", "").Replace(".", ",");
        //            var height = card[3].Replace("\r\n", "").Replace(".", ",");
        //            points.Add(new Tuple<decimal, decimal, decimal, decimal>(Convert.ToDecimal(middleX), Convert.ToDecimal(middleY), Convert.ToDecimal(width), Convert.ToDecimal(height)));
        //        }
        //        catch (Exception x)
        //        {

        //        }
        //    }
        //    return points;
        //}

        //public Tuple<int, int, int, int> GetSingleCardArea()
        //{
        //    Process process = new Process();
        //    string argument = @"C:\Users\Mikolaj\PycharmProjects\pythonProject\externals.py";
        //    process.StartInfo = new System.Diagnostics.ProcessStartInfo()
        //    {
        //        UseShellExecute = false,
        //        CreateNoWindow = true,
        //        WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
        //        FileName = @"C:\Users\Mikolaj\PycharmProjects\pythonProject\venv\Scripts\python.exe",
        //        Arguments = argument,
        //        RedirectStandardError = true,
        //        RedirectStandardOutput = true
        //    };

        //    process.EnableRaisingEvents = true;
        //    process.Start();
        //    var error = process
        //      .StandardError
        //      .ReadToEnd();
        //    var result = process
        //        .StandardOutput
        //        .ReadToEnd()
        //        .Replace("\t", "")
        //        .Replace("\r", "")
        //        .Replace("\n", "");
        //    var points = result.Split(',');
        //    return new Tuple<int, int, int, int>(
        //        Convert.ToInt32(points[0]),
        //        Convert.ToInt32(points[1]),
        //        Convert.ToInt32(points[2]),
        //        Convert.ToInt32(points[3]));
        //}

        //public Tuple<int, int, int, int> GetArea()
        //{
        //    Process process = new Process();
        //    string argument = @"C:\Users\mkosi\PycharmProjects\pythonProject\GetArea.py";
        //    process.StartInfo = new System.Diagnostics.ProcessStartInfo()
        //    {
        //        UseShellExecute = false,
        //        CreateNoWindow = true,
        //        WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
        //        FileName = @"C:\Users\Mikolaj\PycharmProjects\pythonProject\venv\Scripts\python.exe",
        //        Arguments = argument,
        //        RedirectStandardError = true,
        //        RedirectStandardOutput = true
        //    };

        //    process.EnableRaisingEvents = true;
        //    process.Start();
        //    var error = process
        //       .StandardError
        //       .ReadToEnd();
        //    var result = process
        //        .StandardOutput
        //        .ReadToEnd()
        //        .Replace("\t", "")
        //        .Replace("\r", "")
        //        .Replace("\n", "");
        //    var points = result.Split(',');
        //    return new Tuple<int, int, int, int>(
        //        Convert.ToInt32(points[0]),
        //        Convert.ToInt32(points[1]),
        //        Convert.ToInt32(points[2]),
        //        Convert.ToInt32(points[3]));

        //}

        //public string GetAllCards(string fileName)
        //{
        //    Process process = new Process();
        //    string script = @"C:\Users\mkosi\PycharmProjects\pythonProject\cardScreen.py";
        //    process.StartInfo = new System.Diagnostics.ProcessStartInfo()
        //    {
        //        UseShellExecute = false,
        //        CreateNoWindow = true,
        //        WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
        //        FileName = @"C:\Users\mkosi\PycharmProjects\pythonProject\venv\Scripts\python.exe",
        //        Arguments = $"{script} {fileName}",
        //        RedirectStandardError = true,
        //        RedirectStandardOutput = true
        //    };

        //    process.EnableRaisingEvents = true;
        //    process.Start();
        //    var error = process
        //       .StandardError
        //       .ReadToEnd();
        //    var result = process
        //        .StandardOutput
        //        .ReadToEnd();
        //    return result;
        //}

        //public string GetColorFigure(int cardsCount, string cardName)
        //{
        //    Process process = new Process();
        //    string argument = @$"C:\Users\mkosi\PycharmProjects\pythonProject\FC.py";
        //    process.StartInfo = new System.Diagnostics.ProcessStartInfo()
        //    {
        //        UseShellExecute = false,
        //        CreateNoWindow = true,
        //        WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
        //        FileName = @"C:\Users\mkosi\PycharmProjects\pythonProject\venv\Scripts\python.exe",
        //        Arguments = string.Format("{0} {1} {2}", argument, cardsCount, cardName),
        //        RedirectStandardError = true,
        //        RedirectStandardOutput = true
        //    };

        //    process.EnableRaisingEvents = true;
        //    process.Start();
        //    var error = process
        //       .StandardError
        //       .ReadToEnd();
        //    var result = process
        //        .StandardOutput
        //        .ReadToEnd();
        //    return result;
        //}

        public string GetCardsCountOnDesk()
        {
            return "";
            //Process process = new Process();
            //string argument = @$"C:\Users\mkosi\PycharmProjects\pythonProject\predictFigures.py";
            //process.StartInfo = new System.Diagnostics.ProcessStartInfo()
            //{
            //    UseShellExecute = false,
            //    CreateNoWindow = true,
            //    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
            //    FileName = @"C:\Users\mkosi\PycharmProjects\pythonProject\venv\Scripts\python.exe",
            //    Arguments = string.Format("{0}", argument),
            //    RedirectStandardError = true,
            //    RedirectStandardOutput = true
            //};

            //process.Start();
            //var error = process
            //   .StandardError
            //   .ReadToEnd();
            //var result = process
            //    .StandardOutput
            //    .ReadToEnd();
            //return result.Replace("\r\n", "");
        }

        public string GetDetect(string path, string area, int number)
        {
            var t = new TaskCompletionSource<string>();
            Process process = new Process();
            string argument = @$"C:\Users\mkosi\PycharmProjects\pythonProject\Detect.py";
            process.StartInfo = new System.Diagnostics.ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = @"C:\Users\mkosi\PycharmProjects\pythonProject\venv\Scripts\python.exe",
                Arguments = string.Format("{0} {1} {2}", argument, path, number),
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            process.EnableRaisingEvents = true;

            

            //process.Exited += (object sender, EventArgs e) =>
            //{
            //    t.SetResult(process.StandardOutput.ReadToEnd());
            //    var ress = t.Task.Result;
            //    Debug.WriteLine(ress);
            //};

            process.Start();
            var result = process
                .StandardOutput
                .ReadToEnd();
            Debug.WriteLine(result);
            Enum.TryParse<AnalyzeArea>(area, out AnalyzeArea areaResult);

            var rrrr = $"{result[0]}||color|{number}";
            var mdoel = result.Substring(1, 2);
            Debug.WriteLine(mdoel);

            //OnCardRecognised(rrrr, areaResult);
            return "";// result.Replace("\r\n", "");
        }

        //public Task<string> GetHandAsync()
        //{
        //    var t = new TaskCompletionSource<string>(); //Using bool, because TaskCompletionSource needs at least one generic param
        //    string result = "";
        //    Process process = new Process();
        //    string argument = @$"C:\Users\mkosi\PycharmProjects\pythonProject\predictFigures.py";
        //    process.StartInfo = new System.Diagnostics.ProcessStartInfo()
        //    {
        //        UseShellExecute = false,
        //        CreateNoWindow = true,
        //        WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
        //        FileName = @"C:\Users\mkosi\PycharmProjects\pythonProject\venv\Scripts\python.exe",
        //        Arguments = string.Format("{0}", argument),
        //        RedirectStandardError = true,
        //        RedirectStandardOutput = true
        //    };

        //    process.EnableRaisingEvents = true;
        //    process.Exited += (object sender, EventArgs e) =>
        //    {
        //        ////TODO: Exceptions will go first, followed by `return;`
        //        //t.SetException();

        //        //TODO: Finally, if there are no problems, return successfully

        //        t.SetResult(process.StandardOutput.ReadToEnd().Replace("\r\n", ""));
        //        result = process.StandardOutput.ReadToEnd().Replace("\r\n", "");
        //    };
        //    process.Start();
        //    var error = process
        //       .StandardError
        //       .ReadToEnd();
        //    //var result = process
        //    //    .StandardOutput
        //    //    .ReadToEnd();
        //    return t.Task;
        //}

        //private Tuple<int, int, int, int> GetShapePosition(string path)
        //{
        //    Process process = new Process();
        //    string argument = $@"C:\Users\Mikolaj\PycharmProjects\pythonProject1\Segmentation.py {path}";
        //    process.StartInfo = new System.Diagnostics.ProcessStartInfo()
        //    {
        //        UseShellExecute = false,
        //        CreateNoWindow = true,
        //        WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
        //        FileName = @"C:\Users\Mikolaj\AppData\Local\Programs\Python\Python39\python.exe",
        //        Arguments = argument,
        //        RedirectStandardError = true,
        //        RedirectStandardOutput = true
        //    };

        //    process.Start();
        //    var result = process.StandardOutput.ReadToEnd().Split('-');
        //    if (result.Length == 1) return new Tuple<int, int, int, int>(0, 0, 0, 0);
        //    return new Tuple<int, int, int, int>(
        //        Convert.ToInt32(result[0]),
        //        Convert.ToInt32(result[1]),
        //        Convert.ToInt32(result[2]),
        //        Convert.ToInt32(result[3]));
        //}

        //public string CenterFigure(string path)
        //{
        //    Process process = new Process();
        //    string argument = $@"C:\Users\Mikolaj\PycharmProjects\pythonProject1\CenterFigure.py {path}";
        //    process.StartInfo = new System.Diagnostics.ProcessStartInfo()
        //    {
        //        UseShellExecute = false,
        //        CreateNoWindow = true,
        //        WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
        //        FileName = @"C:\Users\Mikolaj\AppData\Local\Programs\Python\Python39\python.exe",
        //        Arguments = argument,
        //        RedirectStandardError = true,
        //        RedirectStandardOutput = true
        //    };

        //    process.Start();
        //    var result = process.StandardOutput.ReadToEnd();
        //    return result.Replace("\t", "").Replace("\r", "").Replace("\n", "");
        //}


        //public Stream ToStream(Image image)
        //{
        //    var stream = new MemoryStream();

        //    image.Save(stream, image.RawFormat);
        //    stream.Position = 0;

        //    return stream;
        //}

        //private string RecognizeImage(recoType rc, string imagePath, int number)
        //{
        //    try
        //    {
        //        var t = new TaskCompletionSource<string>(); //Using bool, because TaskCompletionSource needs at least one generic param
        //        string result = "";
        //        string img = string.Empty;
        //        using (var image = Bitmap.FromFile(imagePath))
        //        {
        //            using (MemoryStream ms = new MemoryStream())
        //            {
        //                image.Save(ms, image.RawFormat);
        //                byte[] array = ms.ToArray();
        //                img = Convert.ToBase64String(array);
        //            }
        //        }

        //        Process process = new Process();
        //        string argument = rc == recoType.figure ?
        //            $@"C:\Users\mkosi\PycharmProjects\pythonProject\predictFigure.py {img} {number}" ://{imagePath.Replace(" ", "")}" :
        //            $@"C:\Users\mkosi\PycharmProjects\pythonProject\predictColor.py {img} {number}";
        //        process.StartInfo = new System.Diagnostics.ProcessStartInfo()
        //        {
        //            UseShellExecute = false,
        //            CreateNoWindow = true,
        //            WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
        //            FileName = @"C:\Users\mkosi\PycharmProjects\pythonProject\venv\Scripts\python.exe",
        //            Arguments = string.Format("{0}", argument),
        //            RedirectStandardError = true,
        //            RedirectStandardOutput = true
        //        };

        //        process.EnableRaisingEvents = true;
        //        process.Exited += (object sender, EventArgs e) =>
        //        {
        //            ////TODO: Exceptions will go first, followed by `return;`
        //            //t.SetException();

        //            //TODO: Finally, if there are no problems, return successfully

        //            t.SetResult(process.StandardOutput.ReadToEnd().Replace("\r\n", ""));
        //            result = process.StandardOutput.ReadToEnd().Replace("\r\n", "");
        //        };
        //        process.Start();
        //        Debug.WriteLine($"process {process.Id} started");
        //        //var error = process
        //        //   .StandardError
        //        //   .ReadToEnd();
        //        //var result = process.StandardOutput.ReadToEnd().Split("\n");
        //        ////var list = result.Split("\n");
        //        //var ress = rc == recoType.figure ? result[3].Replace("\r", "") : result[0].Replace("\r", "");
        //        return "";
        //    }
        //    catch(Exception x)
        //    {
        //        return "";
        //    }
        //}
        int reqCount = 0;
        async Task<string> GetFigure(string image, string mode, string info)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    await Task.Delay(1000);
                    var path = $"https://predapp.azurewebsites.net/api/TestAlive?code=y5MaFGr54uaiah8ACRNNp_u26SV3oBfzbQxzb0XcW6MXAzFuFmAkiQ==&path={image}&mode={mode}&info={info}";
                    var response = await client.GetAsync(path);
                   
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    //reqCount++;
                    Enum.TryParse<AnalyzeArea>(info.Split("|")[3], out AnalyzeArea area);
                    Debug.WriteLine(responseBody);                    
                    OnCardRecognised(responseBody, area, cardStage);
                    
                    //response.ContinueWith((async t) =>
                    //{
                    //    reqCount++;
                    //    Debug.WriteLine(reqCount.ToString());
                    //});

                    //if (response.IsSuccessStatusCode)
                    //{
                    //    var responseContent = response.Content;
                    //    var result = await responseContent.ReadAsStringAsync();
                    //    Debug.WriteLine($"RESULT    {result}");
                    //    OnCardRecognised(result);
                    //}
                }
                catch (AggregateException z)
                {

                }
                catch(Exception ex)
                {

                }


                return "";
            }
        }

        //void OnReturnFigure(Task<HttpResponseMessage> task)
        //{
        //    var ttt = task.GetAwaiter().GetResult();
        //    reqCount++;
        //    Debug.WriteLine(reqCount.ToString());
        //}

        //private void recognised(Task<string> t)
        //{
        //    //var info = t.Result.Split('|')[0];
        //    //var result = t.Result;
        //    //Debug.WriteLine($"RESULT    {t}");
        //    //OnCardRecognised(result);
        //}

        private async void RecoImage(recoType rc, string imagePath, int number, string area)
        {
            //GetFigure().ContinueWith(OnReturnFigure);
            try
            {                                
                //var t = new TaskCompletionSource<string>(); //Using bool, because TaskCompletionSource needs at least one generic param
                //string result = "";
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

                //Process process = new Process();
                string argument = rc == recoType.figure ?
                    $@"C:\Users\mkosi\PycharmProjects\pythonProject\predictFigure.py {img} {number} {area}" ://{imagePath.Replace(" ", "")}" :
                    $@"C:\Users\mkosi\PycharmProjects\pythonProject\predictColor.py {img} {number} {area}";

                //GetDetect(imagePath);

                if (rc == recoType.color)
                {
                    GetFigure(img, "color", $"notNeeded|{rc}|{number}|{area}");                    
                }
                else
                    GetFigure(img, "figure", $"notNeeded|{rc}|{number}|{area}");

                //process.StartInfo = new System.Diagnostics.ProcessStartInfo()
                //{
                //    UseShellExecute = false,
                //    CreateNoWindow = true,
                //    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                //    FileName = @"C:\Users\mkosi\PycharmProjects\pythonProject\venv\Scripts\python.exe",
                //    Arguments = string.Format("{0}", argument),
                //    RedirectStandardError = true,
                //    RedirectStandardOutput = true
                //};

                //process.EnableRaisingEvents = true;
                //process.Exited += (object sender, EventArgs e) =>
                //{
                //    t.SetResult(process.StandardOutput.ReadToEnd().Replace("\r\n", ""));
                //    //result = process.StandardOutput.ReadToEnd().Replace("\r\n", "");
                //    var FC = t.Task.Result.Split('|')[0];
                //    var result = $"{FC}|{rc}|{number}|{area}";
                //    var ff = process.StandardError.ReadToEnd();
                //    //Debug.WriteLine($"{rc}|{number}|{ress}");
                //    //OnCardRecognised(result);
                //};

                ////if(number%3 > 0)
                ////    await Task.Delay(1000);

                //process.Start();
                
                //Debug.WriteLine($"process CARD {process.Id} started");
                
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

        public delegate void CardHandler(string cardReco, AnalyzeArea area, Stage cardStage);
        public event CardHandler CardRecognised;
        private void OnCardRecognised(string cardReco, AnalyzeArea area, Stage cardStage)
        {
            if (CardRecognised != null)
                CardRecognised(cardReco, area, cardStage);
        }

        private string rr;

        //public MemoryStream SerializeToStream(object o)
        //{
        //    MemoryStream stream = new MemoryStream();
        //    IFormatter formatter = new BinaryFormatter();
        //    formatter.Serialize(stream, o);
        //    return stream;
        //}

        //public string RecogniseByPath(string path)
        //{
        //    return "";
        //}

        public enum recoType
        {
            figure,
            color
        }
    }    
}
