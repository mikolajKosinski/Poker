using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleApp1
{
    class Program
    {
        static double total;
        static void Main(string[] args)
        {
            var foldersPath = "C:\\Users\\mkosi\\PycharmProjects\\tensorEnv\\dataset";
            var dirs = System.IO.Directory.GetDirectories(foldersPath);

            foreach (var dir in dirs)
            {
                ShakeImages(dir);
                Console.WriteLine(dir);
            }
        }

        private const int bytesPerPixel = 4;

        public static System.Drawing.Image CropImage(Bitmap source, int width, int height)
        {
            int xWidth = width;
            int xHeight = height;
            if(width > source.Width || height > source.Height)
            {
                xWidth = source.Width/2;
                xHeight = source.Height/2;
            }
            return source.Clone(new System.Drawing.Rectangle(0, 0, xWidth, xHeight), source.PixelFormat);
        }

        public static System.Drawing.Image MiddleCropImage(Bitmap source, int width, int height)
        {
            return source.Clone(new System.Drawing.Rectangle(50, 50, width, height), source.PixelFormat);
        }



        static void ShakeImages(string path)
        {

            var files = Directory.GetFiles(path);

            var dirName = new DirectoryInfo(path).Name;



            if (dirName == "2C"
                || dirName == "2D"
                    || dirName == "2H"
                    || dirName == "2S"
                    || dirName == "3C"
                    || dirName == "3D"
                    || dirName == "3H"
                    || dirName == "3S"
                    || dirName == "4C"
                    || dirName == "4D"
                    || dirName == "4H"
                    || dirName == "4S"
                    || dirName == "5C"
                    || dirName == "5D"
                    || dirName == "5H"
                    || dirName == "5S"
                    || dirName == "6C"
                    || dirName == "6D"
                    || dirName == "6H"
                    || dirName == "6S"
                    || dirName == "7C"
                    || dirName == "7D"
                    || dirName == "7H"
                    || dirName == "7S"
                    || dirName == "8C"
                    || dirName == "8D"
                    || dirName == "8H"
                    || dirName == "8S"
                    || dirName == "9C"
                    || dirName == "9D"
                    || dirName == "9H"
                    || dirName == "9S"
                    || dirName == "10C"
                    || dirName == "10D"
                    || dirName == "10H"
                    || dirName == "10S"
                    || dirName == "AC"
                    || dirName == "AD"
                    || dirName == "AH"
                    || dirName == "AS"
                    || dirName == "JC"
                    || dirName == "JD"
                    || dirName == "JH"
                    || dirName == "JS"
                    //|| dirName == "KC"
                    || dirName == "KD"
                    || dirName == "KH"
                    || dirName == "KS"
                    || dirName == "QC"
                    || dirName == "QD"
                    || dirName == "QH"
                    )
            {
                return;
            }


            var rrand = new Random();
            //int pxr;
            //int pyr;
            int number = 1;



            //int imgN = 126;
            //int validate = 10;
            //int fileNumber = 1;

            //foreach (var file in Directory.GetFiles(path))
            //{

            //    if (fileNumber > 21) continue;

            //    for (int x = 0; x < 10; x++)
            //    {
            //        imgN++;
            //        using (Bitmap newBitmap = (Bitmap)System.Drawing.Image.FromFile($"{file}"))
            //        {
            //            var start = DateTime.Now;
            //            for (int y = 0; y < x; y++)
            //            {
            //                for (int px = 1; px < newBitmap.Width; px++)
            //                {
            //                    for (int py = 1; py < newBitmap.Height; py++)
            //                    {
            //                        var shouldShake = rrand.Next(5) % 5 == 0;
            //                        if (shouldShake)
            //                        {
            //                            //pxr = rrand.Next(px);
            //                            //pyr = rrand.Next(py);
            //                            newBitmap.SetPixel(px, py, Color.Gray);
            //                        }
            //                    }
            //                }
            //            }

            //            var finalPath = path.Replace("kartyX", "dataset");
            //            var cropPath = "";

            //            if (fileNumber == 21)
            //            {
            //                cropPath = getFinalPath(finalPath, $"{dirName}_validate{validate}.jpg");
            //                validate++;
            //            }
            //            else
            //            {
            //                cropPath = getFinalPath(finalPath, $"{dirName}_v{imgN}.jpg");
            //            }
            //            //number++;
            //            newBitmap.Save(cropPath);
            //            var end = DateTime.Now;
            //            total += (end - start).TotalSeconds;                        
            //            Console.WriteLine($"{dirName} round {imgN}  - {total} seconds");
            //        }
            //    }
            //    fileNumber++;
            //}


            //for (int q = 1; q < 10; q++)
            //{

            //    using (Bitmap newBitmap = ((Bitmap)System.Drawing.Image.FromFile($"{path}\\{dirName}_{q}.jpg")))
            //    {
            //        for (int r = 1; r < 15; r++)
            //        {
            //            for (int px = 1; px < newBitmap.Width; px++)
            //            {
            //                for (int py = 1; py < newBitmap.Height; py++)
            //                {
            //                    var shouldShake = rrand.Next(50) % 5 == 0;
            //                    if (shouldShake)
            //                    {
            //                        //pxr = rrand.Next(px);
            //                        //pyr = rrand.Next(py);
            //                        newBitmap.SetPixel(px, py, Color.Gray);
            //                    }
            //                }
            //            }
            //            var cropPath = getFinalPath(path, $"{dirName}_v{number}.jpg");
            //            number++;
            //            newBitmap.Save(cropPath);
            //            Console.WriteLine($"{dirName} round {number}");
            //        }
            //    }
            //}
            number = 1;

            //dirName = "6C_v1";


            //var ppp = "C:\\Users\\mkosi\\PycharmProjects\\tensorEnv\\kartyX";
            //var vpath = Path.Combine(path, ppp);
            //vpath = Path.Combine(vpath, dirName);
            //var lastOne = Directory.GetFiles(vpath);
            //var last = lastOne.Last();

            using (Bitmap newBitmap = ((Bitmap)System.Drawing.Image.FromFile($"{path}\\{dirName}_6.jpg")))
            //using (Bitmap newBitmap = ((Bitmap)System.Drawing.Image.FromFile(last)))
            {
                for (int r = 10; r < 20; r++)
                {
                    number = r;

                    var cropPath = getFinalPath(path, $"{dirName}_validate{number}.jpg");
                    if (File.Exists(cropPath))
                    {
                        Console.WriteLine($"{dirName} validate {number}");
                        continue;
                    }
                        for (int px = 1; px < newBitmap.Width; px++)
                    {
                        for (int py = 1; py < newBitmap.Height; py++)
                        {
                            var shouldShake = rrand.Next(50) % 5 == 0;
                            if (shouldShake)
                            {
                                //pxr = rrand.Next(px);
                                //pyr = rrand.Next(py);
                                newBitmap.SetPixel(px, py, Color.Gray);
                            }
                        }
                    }
                    //var cropPath = getFinalPath(path, $"{dirName}_validate{number}.jpg");
                    
                    
                    if (!File.Exists(cropPath))
                    {
                        //number++;
                        newBitmap.Save(cropPath);
                        Console.WriteLine($"{dirName} validate {number}");
                    }
                }
            }


        }

        //static string getFinalPathX(string path, string fileName)
        //{
        //    var card = Path.GetDirectoryName(path);
        //    return Path.Combine("C:\\Users\\mkosi\\PycharmProjects\\tensorEnv\\dataset\\2C", fileName);
        //}

        static string getFinalPath(string path, string fileName)
        {
            return Path.Combine(path, fileName);
        }

        static Bitmap Reverse(Bitmap img)
        {
            Random rand = new Random();
            Bitmap pic = new Bitmap(img);
            for (int y = 0; (y <= (pic.Height - 1)); y++)
            {
                for (int x = 0; (x <= (pic.Width - 1)); x++)
                {

                    Color inv = pic.GetPixel(rand.Next(1, 200), rand.Next(1, 200));
                    inv = Color.FromArgb(255, (rand.Next(1, 255)), (rand.Next(1, 255)), (rand.Next(1, 255)));
                    pic.SetPixel(x, y, inv);
                }
            }
            return pic;
        }

        static Bitmap RotateImage(Bitmap bmp, float angle, float width, float height)
        {

            Bitmap rotatedImage = new Bitmap(bmp.Width, bmp.Height);
            //rotatedImage.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);

            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                // Set the rotation point to the center in the matrix
                g.TranslateTransform(width, height);
                // Rotate
                g.RotateTransform(angle);
                // Restore rotation point in the matrix
                g.TranslateTransform(-bmp.Width / 2, -bmp.Height / 2);
                // Draw the image on the bitmap
                var rand = new Random();
                g.DrawImage(bmp, new Point(0, 0));
            }

            return rotatedImage;
        }

        static Bitmap Gray(Bitmap bmp)
        {
            int x, y;
            var rand = new Random();
            // Loop through the images pixels to reset color.
            for (x = 0; x < 50; x++)
            {
                for (y = 0; y < 50; y++)
                {
                    Color pixelColor = bmp.GetPixel(x, y);
                    Color newColor = Color.FromArgb(pixelColor.R, 0, 0);
                    //var numer = rand.Next(1, 3);
                    //if (numer % 2 > 1)
                    //{
                    for (int Xcount = 0; Xcount < 150; Xcount++)
                    {
                        for (int Ycount = 0; Ycount < 150; Ycount++)
                        {
                            var numer = rand.Next(0, 20);
                            if (numer % 20 == 0)
                            {
                                bmp.SetPixel(Xcount, Ycount, newColor); // Now greyscale
                            }
                        }
                    }

                    //bmp.SetPixel(20, 20, newColor); // Now greyscale
                    //}
                }
            }

            return bmp;
        }

        static Bitmap RotateImage(Bitmap img)
        {
            img.RotateFlip(RotateFlipType.Rotate90FlipX);
            return img;
        }

        //static Image[] SplitIntoPieces(Bitmap img)
        //{
        //    var imgarray = new Image[2];

        //    for (int j = 0; j < 2; j++)
        //    {
        //        var index = j;
        //        imgarray[index] = new Bitmap(104, 104);
        //        var graphics = Graphics.FromImage(imgarray[index]);
        //        graphics.DrawImage(img, new Rectangle(0, 0, 104, 104), new Rectangle(104, j * 52, 104, 104), GraphicsUnit.Pixel);
        //        graphics.Dispose();
        //    }

        //    return imgarray;
        //}
    }
}
