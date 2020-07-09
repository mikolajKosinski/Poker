using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
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

        public static Image CropImage(Bitmap source, int width, int height)
        {
            return source.Clone(new System.Drawing.Rectangle(0, 0, width, height), source.PixelFormat);
        }

        public static Image MiddleCropImage(Bitmap source, int width, int height)
        {
            return source.Clone(new System.Drawing.Rectangle(50, 50, width, height), source.PixelFormat);
        }



        static void ShakeImages(string path)
        {

            var files = Directory.GetFiles(path);

            var dirName = new DirectoryInfo(path).Name;


            using (Bitmap bitmap = (Bitmap)Image.FromFile($"{path}\\{dirName}_1.jpg"))
            {
                using (Bitmap newBitmap = new Bitmap(bitmap))
                {
                    for (int q = 1; q < 60; q++)//60
                    {
                        int x = 0, y = 0, width = 500, height = 400;
                        Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat);
                        var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        RotateImage(CroppedImage, (float)q, CroppedImage.Width - 150, CroppedImage.Height - 20).Save(rotatePath, ImageFormat.Jpeg);
                    }

                    for (int q = 61; q < 160; q++)//100
                    {
                        int x = 0, y = 0, width = 500, height = 400;
                        Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat);
                        var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        RotateImage(CroppedImage, (float)q, CroppedImage.Width - 240, CroppedImage.Height - 150).Save(rotatePath, ImageFormat.Jpeg);
                    }
                    for (int q = 160; q < 220; q++)//60
                    {
                        int x = 0, y = 0, width = 500, height = 400;
                        Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat);
                        var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        RotateImage(CroppedImage, (float)q, CroppedImage.Width - 240, CroppedImage.Height - 220).Save(rotatePath, ImageFormat.Jpeg);
                    }
                    for (int q = 220; q < 360; q++)//140
                    {
                        int x = 0, y = 0, width = 500, height = 400;
                        Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat);

                        var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        RotateImage(CroppedImage, (float)q, CroppedImage.Width - 50, CroppedImage.Height - 280).Save(rotatePath, ImageFormat.Jpeg);
                    }
                    for (int q = 360; q < 900; q++)
                    {
                        var rand = new Random();
                        for (int r = 0; r < 1000; r++)
                        {
                            newBitmap.SetPixel(rand.Next(1, 400), rand.Next(1, 400), Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256)));
                        }
                        for (int r = 0; r < 1000; r++)
                        {
                            newBitmap.SetPixel(rand.Next(1, 400), rand.Next(1, 400), Color.White);
                        }
                        var cropPath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        var img = CropImage(newBitmap, rand.Next(100, 200), rand.Next(250, 400));
                        img.Save(cropPath);
                    }
                }
            }

            using (Bitmap bitmap = (Bitmap)Image.FromFile($"{path}\\{dirName}_2.jpg"))
            {
                using (Bitmap newBitmap = new Bitmap(bitmap))
                {
                    for (int q = 900; q < 960; q++)
                    {
                        int x = 0, y = 0, width = 500, height = 400;
                        Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat);
                        var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        RotateImage(CroppedImage, (float)q, CroppedImage.Width - 150, CroppedImage.Height - 20).Save(rotatePath, ImageFormat.Jpeg);
                    }

                    for (int q = 960; q < 1060; q++)
                    {
                        int x = 0, y = 0, width = 500, height = 400;
                        Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat);
                        var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        RotateImage(CroppedImage, (float)q, CroppedImage.Width - 240, CroppedImage.Height - 150).Save(rotatePath, ImageFormat.Jpeg);
                    }
                    for (int q = 1060; q < 1120; q++)
                    {
                        int x = 0, y = 0, width = 500, height = 400;
                        Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat);
                        var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        RotateImage(CroppedImage, (float)q, CroppedImage.Width - 240, CroppedImage.Height - 220).Save(rotatePath, ImageFormat.Jpeg);
                    }
                    for (int q = 1120; q < 1260; q++)
                    {
                        int x = 0, y = 0, width = 500, height = 400;
                        Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat);

                        var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        RotateImage(CroppedImage, (float)q, CroppedImage.Width - 50, CroppedImage.Height - 280).Save(rotatePath, ImageFormat.Jpeg);
                    }
                    for (int q = 1260; q < 1800; q++)
                    {
                        var rand = new Random();
                        for (int r = 0; r < 1000; r++)
                        {
                            newBitmap.SetPixel(rand.Next(1, 400), rand.Next(1, 400), Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256)));
                        }
                        for (int r = 0; r < 1000; r++)
                        {
                            newBitmap.SetPixel(rand.Next(1, 400), rand.Next(1, 400), Color.White);
                        }
                        var cropPath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        var img = CropImage(newBitmap, rand.Next(100, 200), rand.Next(250, 400));
                        img.Save(cropPath);
                    }
                }
            }

            using (Bitmap bitmap = (Bitmap)Image.FromFile($"{path}\\{dirName}_3.jpg"))
            {
                using (Bitmap newBitmap = new Bitmap(bitmap))
                {
                    for (int q = 1800; q < 1860; q++)//60
                    {
                        int x = 0, y = 0, width = 500, height = 400;
                        Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat);
                        var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        RotateImage(CroppedImage, (float)q, CroppedImage.Width - 150, CroppedImage.Height - 20).Save(rotatePath, ImageFormat.Jpeg);
                    }

                    for (int q = 1860; q < 1960; q++)//100
                    {
                        int x = 0, y = 0, width = 500, height = 400;
                        Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat);
                        var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        RotateImage(CroppedImage, (float)q, CroppedImage.Width - 240, CroppedImage.Height - 150).Save(rotatePath, ImageFormat.Jpeg);
                    }
                    for (int q = 1960; q < 2020; q++)
                    {
                        int x = 0, y = 0, width = 500, height = 400;
                        Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat);
                        var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        RotateImage(CroppedImage, (float)q, CroppedImage.Width - 240, CroppedImage.Height - 220).Save(rotatePath, ImageFormat.Jpeg);
                    }
                    for (int q = 2020; q < 2160; q++)
                    {
                        int x = 0, y = 0, width = 500, height = 400;
                        Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat);

                        var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        RotateImage(CroppedImage, (float)q, CroppedImage.Width - 50, CroppedImage.Height - 280).Save(rotatePath, ImageFormat.Jpeg);
                    }
                    for (int q = 2160; q < 2700; q++)
                    {
                        var rand = new Random();
                        for (int r = 0; r < 1000; r++)
                        {
                            newBitmap.SetPixel(rand.Next(1, 400), rand.Next(1, 400), Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256)));
                        }
                        for (int r = 0; r < 1000; r++)
                        {
                            newBitmap.SetPixel(rand.Next(1, 400), rand.Next(1, 400), Color.White);
                        }
                        var cropPath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        var img = CropImage(newBitmap, rand.Next(100, 200), rand.Next(250, 400));
                        img.Save(cropPath);
                    }
                }
            }

            using (Bitmap bitmap = (Bitmap)Image.FromFile($"{path}\\{dirName}_4.jpg"))
            {
                using (Bitmap newBitmap = new Bitmap(bitmap))
                {
                    //for (int q = 2700; q < 2760; q++)
                    //{
                    //    int x = 0, y = 0, width = 500, height = 400;
                    //    Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat);
                    //    var rotatePath = getFinalPath(path, $"{dirName}_validate{q}.jpg");
                    //    RotateImage(CroppedImage, (float)q, CroppedImage.Width - 150, CroppedImage.Height - 20).Save(rotatePath, ImageFormat.Jpeg);
                    //}

                    //for (int q = 2760; q < 2860; q++)
                    //{
                    //    int x = 0, y = 0, width = 500, height = 400;
                    //    Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat);
                    //    var rotatePath = getFinalPath(path, $"{dirName}_validate{q}.jpg");
                    //    RotateImage(CroppedImage, (float)q, CroppedImage.Width - 240, CroppedImage.Height - 150).Save(rotatePath, ImageFormat.Jpeg);
                    //}
                    //for (int q = 2860; q < 2920; q++)
                    //{
                    //    int x = 0, y = 0, width = 500, height = 400;
                    //    Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat);
                    //    var rotatePath = getFinalPath(path, $"{dirName}_validate{q}.jpg");
                    //    RotateImage(CroppedImage, (float)q, CroppedImage.Width - 240, CroppedImage.Height - 220).Save(rotatePath, ImageFormat.Jpeg);
                    //}
                    //for (int q = 2920; q < 3060; q++)
                    //{
                    //    int x = 0, y = 0, width = 500, height = 400;
                    //    Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat);

                    //    var rotatePath = getFinalPath(path, $"{dirName}_validate{q}.jpg");
                    //    RotateImage(CroppedImage, (float)q, CroppedImage.Width - 50, CroppedImage.Height - 280).Save(rotatePath, ImageFormat.Jpeg);
                    //}
                    for (int q = 3060; q < 3600; q++)
                    {
                        var rand = new Random();

                        for (int r = 0; r < 1000; r++)
                        {
                            newBitmap.SetPixel(rand.Next(1, 400), rand.Next(1, 400), Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256)));
                        }
                        for (int r = 0; r < 1000; r++)
                        {
                            newBitmap.SetPixel(rand.Next(1, 400), rand.Next(1, 400), Color.White);
                        }
                      
                        var cropPath = getFinalPath(path, $"{dirName}_validate{q}.jpg");
                        var img = CropImage(newBitmap, rand.Next(100, 200), rand.Next(250, 400));
                        img.Save(cropPath);
                    }
                }
            }

            //using (Bitmap bitmap = (Bitmap)Image.FromFile($"{path}\\{dirName}_5.jpg"))
            //{
            //    using (Bitmap newBitmap = new Bitmap(bitmap))
            //    {
            //        for (int q = 1; q < 60; q++)//60
            //        {
            //            int x = 0, y = 0, width = 500, height = 400;
            //            Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat);
            //            var rotatePath = getFinalPath(path, $"{dirName}_validate{q}.jpg");
            //            RotateImage(CroppedImage, (float)q, CroppedImage.Width - 150, CroppedImage.Height - 20).Save(rotatePath, ImageFormat.Jpeg);
            //        }

            //        for (int q = 61; q < 160; q++)//100
            //        {
            //            int x = 0, y = 0, width = 500, height = 400;
            //            Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat);
            //            var rotatePath = getFinalPath(path, $"{dirName}__validate{q}.jpg");
            //            RotateImage(CroppedImage, (float)q, CroppedImage.Width - 240, CroppedImage.Height - 150).Save(rotatePath, ImageFormat.Jpeg);
            //        }
            //        for (int q = 160; q < 220; q++)//60
            //        {
            //            int x = 0, y = 0, width = 500, height = 400;
            //            Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat);
            //            var rotatePath = getFinalPath(path, $"{dirName}__validate{q}.jpg");
            //            RotateImage(CroppedImage, (float)q, CroppedImage.Width - 240, CroppedImage.Height - 220).Save(rotatePath, ImageFormat.Jpeg);
            //        }
            //        for (int q = 220; q < 360; q++)//140
            //        {
            //            int x = 0, y = 0, width = 500, height = 400;
            //            Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat);

            //            var rotatePath = getFinalPath(path, $"{dirName}__validate{q}.jpg");
            //            RotateImage(CroppedImage, (float)q, CroppedImage.Width - 50, CroppedImage.Height - 280).Save(rotatePath, ImageFormat.Jpeg);
            //        }
            //    }
            //}

                    //using (Bitmap bitmap = (Bitmap)Image.FromFile($"{path}\\{dirName}_5.jpg"))
                    //{
                    //    using (Bitmap newBitmap = new Bitmap(bitmap))
                    //    {
                    //        for (int q = 3600; q < 3660; q++)
                    //        {
                    //            int x = 0, y = 0, width = 500, height = 400;
                    //            Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat);
                    //            var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                    //            RotateImage(CroppedImage, (float)q, CroppedImage.Width - 150, CroppedImage.Height - 20).Save(rotatePath, ImageFormat.Jpeg);
                    //        }

                    //        for (int q = 3660; q < 3760; q++)
                    //        {
                    //            int x = 0, y = 0, width = 500, height = 400;
                    //            Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat);
                    //            var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                    //            RotateImage(CroppedImage, (float)q, CroppedImage.Width - 240, CroppedImage.Height - 150).Save(rotatePath, ImageFormat.Jpeg);
                    //        }
                    //        for (int q = 3760; q < 3820; q++)
                    //        {
                    //            int x = 0, y = 0, width = 500, height = 400;
                    //            Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat);
                    //            var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                    //            RotateImage(CroppedImage, (float)q, CroppedImage.Width - 240, CroppedImage.Height - 220).Save(rotatePath, ImageFormat.Jpeg);
                    //        }
                    //        for (int q = 3820; q < 3960; q++)
                    //        {
                    //            int x = 0, y = 0, width = 500, height = 400;
                    //            Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat);

                    //            var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                    //            RotateImage(CroppedImage, (float)q, CroppedImage.Width - 50, CroppedImage.Height - 280).Save(rotatePath, ImageFormat.Jpeg);
                    //        }
                    //        for (int q = 3960; q < 4500; q++)
                    //        {
                    //            var rand = new Random();
                    //            var bmp = RotateImage(newBitmap);
                    //            var cropPath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                    //            var img = CropImage(bmp, rand.Next(100, 200), rand.Next(250, 400));
                    //            img.Save(cropPath);
                    //        }
                    //    }
                    //}
                }

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
                    
                    Color inv = pic.GetPixel(rand.Next(1,200), rand.Next(1, 200));
                    inv = Color.FromArgb(255, (rand.Next(1, 255)), (rand.Next(1, 255)), (rand.Next(1, 255)));
                    pic.SetPixel(x, y, inv);
                }
            }
            return pic;
        }

        static Bitmap RotateImage(Bitmap bmp, float angle, float width, float height)
        {
           
            Bitmap rotatedImage = new Bitmap(bmp.Width, bmp.Height);
            rotatedImage.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);

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
