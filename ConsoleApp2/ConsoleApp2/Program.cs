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

            if (dirName != "QS") return;

            //if (dirName == "2C"
            //    || dirName == "2D"
            //        || dirName == "2H"
            //        || dirName == "2S"
            //        || dirName == "3C"
            //        || dirName == "3D"
            //        || dirName == "3H"
            //        || dirName == "3S"
            //        || dirName == "4C"
            //        || dirName == "4D"
            //        || dirName == "4H"
            //        || dirName == "4S"
            //        || dirName == "5C"
            //        || dirName == "5D"
            //        || dirName == "5H"
            //        || dirName == "5S"
            //        || dirName == "6C"
            //        || dirName == "6D"
            //        || dirName == "6H"
            //        || dirName == "6S"
            //        || dirName == "7C"
            //        || dirName == "7D"
            //        || dirName == "7H"
            //        || dirName == "7S"
            //        || dirName == "8C"
            //        || dirName == "8D"
            //        || dirName == "8H"
            //        || dirName == "8S"
            //        || dirName == "9C"
            //        || dirName == "9D"
            //        || dirName == "9H"
            //        || dirName == "9S"                    
            //        || dirName == "10C"
            //        || dirName == "10D"
            //        || dirName == "10H"
            //        || dirName == "10S"
            //        || dirName == "AC"
            //        || dirName == "AD"
            //        || dirName == "AH"
            //        || dirName == "AS"
            //        || dirName == "JC"
            //        || dirName == "JD"
            //        || dirName == "JH"
            //        || dirName == "JS"
            //        || dirName == "KC"
            //        || dirName == "KD"
            //        || dirName == "KH"
            //        || dirName == "KS"
            //        || dirName == "QC"
            //        || dirName == "QD"
            //        || dirName == "QH"
            //        )
            //{
            //    return;
            //}
            using (Bitmap newBitmap = ((Bitmap)System.Drawing.Image.FromFile($"{path}\\{dirName}_1.jpg")))
            {

                try
                {
                    var rand = new Random();

                    for (int q = 1; q < 60; q++)//60
                    {
                        int x = 0, y = 0, width = 500, height = 400;

                        var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        RotateImage(newBitmap, (float)q, newBitmap.Width - 150, newBitmap.Height - 20).Save(rotatePath, ImageFormat.Jpeg);
                        GC.Collect();
                    }

                    for (int q = 60; q < 160; q++)//100
                    {
                        int x = 0, y = 0, width = 500, height = 400;

                        var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        RotateImage(newBitmap, (float)q, newBitmap.Width - 240, newBitmap.Height - 150).Save(rotatePath, ImageFormat.Jpeg);
                        GC.Collect();
                    }
                    for (int q = 160; q < 220; q++)//60
                    {
                        int x = 0, y = 0, width = 500, height = 400;

                        var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        RotateImage(newBitmap, (float)q, newBitmap.Width - 240, newBitmap.Height - 220).Save(rotatePath, ImageFormat.Jpeg);
                        GC.Collect();
                    }
                    for (int q = 220; q < 360; q++)//140
                    {
                        int x = 0, y = 0, width = 500, height = 400;
                        //using (Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat))
                        //{

                        var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        RotateImage(newBitmap, (float)q, newBitmap.Width - 50, newBitmap.Height - 280).Save(rotatePath, ImageFormat.Jpeg);
                        GC.Collect();
                    }
                    for (int q = 360; q < 900; q++)
                    {
                        for (int r = 0; r < 150; r++)
                        {
                            var xx = 1;
                            var yx = 0;
                            while (xx > yx)
                            {
                                xx = rand.Next(1, 300);
                                yx = rand.Next(1, 300);
                            }

                            if (xx >= newBitmap.Width) xx = newBitmap.Width / 2;
                            if (yx >= newBitmap.Height) yx = newBitmap.Height / 2;

                            var xx1 = rand.Next(1, 256);
                            newBitmap.SetPixel(xx, yx, Color.FromArgb(xx1, xx1, xx1));
                        }
                        //for (int r = 0; r < 1000; r++)
                        //{
                        //    var xx = 1;
                        //    var yx = 0;
                        //    while (xx > yx)
                        //    {
                        //        xx = rand.Next(1, 300);
                        //        yx = rand.Next(1, 300);
                        //    }

                        //    if (xx >= newBitmap.Width) xx = newBitmap.Width / 2;
                        //    if (yx >= newBitmap.Height) yx = newBitmap.Height / 2;

                        //    var xx1 = rand.Next(1, 256);
                        //    newBitmap.SetPixel(xx, yx, Color.White);
                        //}
                        var cropPath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        var img = CropImage(newBitmap, rand.Next(100, 200), rand.Next(250, 400));
                        img.Save(cropPath);
                        GC.Collect();
                    }
                }
                catch (OutOfMemoryException x)
                {
                    //System.Runtime.GCSettings.LargeObjectHeapCompactionMode = System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;
                    //GC.Collect();
                }
            }

            using (Bitmap newBitmap = ((Bitmap)System.Drawing.Image.FromFile($"{path}\\{dirName}_2.jpg")))
            {

                try
                {
                    var rand = new Random();

                    for (int q = 900; q < 960; q++)
                    {
                        int x = 0, y = 0, width = 500, height = 400;
                        //using (Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat))
                        //{
                        var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");

                        RotateImage(newBitmap, (float)q, newBitmap.Width - 150, newBitmap.Height - 20).Save(rotatePath, ImageFormat.Jpeg);
                        GC.Collect();
                        //}
                    }

                    for (int q = 960; q < 1060; q++)
                    {
                        int x = 0, y = 0, width = 500, height = 400;
                        //using (Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat))
                        //{
                        var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        RotateImage(newBitmap, (float)q, newBitmap.Width - 240, newBitmap.Height - 150).Save(rotatePath, ImageFormat.Jpeg);
                        GC.Collect();
                        //}
                    }
                    for (int q = 1060; q < 1120; q++)
                    {
                        int x = 0, y = 0, width = 500, height = 400;
                        //using (Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat))
                        //{
                        var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        RotateImage(newBitmap, (float)q, newBitmap.Width - 240, newBitmap.Height - 220).Save(rotatePath, ImageFormat.Jpeg);
                        GC.Collect();
                        //}
                    }
                    for (int q = 1120; q < 1260; q++)
                    {
                        int x = 0, y = 0, width = 500, height = 400;
                        //using (Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat))
                        //{
                        var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        RotateImage(newBitmap, (float)q, newBitmap.Width - 50, newBitmap.Height - 280).Save(rotatePath, ImageFormat.Jpeg);
                        GC.Collect();
                        //}
                    }
                    for (int q = 1260; q < 1800; q++)
                    {
                        for (int r = 0; r < 150; r++)
                        {
                            var xx = 1;
                            var yx = 0;
                            while (xx > yx)
                            {
                                xx = rand.Next(1, 300);
                                yx = rand.Next(1, 300);
                            }

                            if (xx >= newBitmap.Width) xx = newBitmap.Width / 2;
                            if (yx >= newBitmap.Height) yx = newBitmap.Height / 2;

                            var x1 = rand.Next(1, 256);
                            newBitmap.SetPixel(xx, yx, Color.FromArgb(x1, x1, x1));
                        }
                        //for (int r = 0; r < 1000; r++)
                        //{
                        //    var xx = 1;
                        //    var yx = 0;
                        //    while (xx > yx)
                        //    {
                        //        xx = rand.Next(1, 300);
                        //        yx = rand.Next(1, 300);
                        //    }

                        //    if (xx >= newBitmap.Width) xx = newBitmap.Width / 2;
                        //    if (yx >= newBitmap.Height) yx = newBitmap.Height / 2;

                        //    var x1 = rand.Next(1, 256);
                        //    newBitmap.SetPixel(xx, yx, Color.White);
                        //}
                        var cropPath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        var img = CropImage(newBitmap, rand.Next(100, 200), rand.Next(250, 400));
                        img.Save(cropPath);
                        GC.Collect();
                    }
                }
                catch (OutOfMemoryException x)
                {
                    //System.Runtime.GCSettings.LargeObjectHeapCompactionMode = System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;
                    //GC.Collect();
                }
                GC.Collect();
            }

            try
            {
                using (Bitmap newBitmap = ((Bitmap)System.Drawing.Image.FromFile($"{path}\\{dirName}_3.jpg")))
                {


                    for (int q = 1800; q < 1860; q++)//60
                    {
                        int x = 0, y = 0, width = 500, height = 400;
                        //using (Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat))
                        //{
                        var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        RotateImage(newBitmap, (float)q, newBitmap.Width - 150, newBitmap.Height - 20).Save(rotatePath, ImageFormat.Jpeg);
                        GC.Collect();
                        //}
                    }

                    for (int q = 1860; q < 1960; q++)//100
                    {
                        int x = 0, y = 0, width = 500, height = 400;
                        //using (Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat))
                        //{
                        var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        RotateImage(newBitmap, (float)q, newBitmap.Width - 240, newBitmap.Height - 150).Save(rotatePath, ImageFormat.Jpeg);
                        GC.Collect();
                        //}
                    }
                    for (int q = 1960; q < 2020; q++)
                    {
                        int x = 0, y = 0, width = 500, height = 400;
                        //using (Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat))
                        //{
                        var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        RotateImage(newBitmap, (float)q, newBitmap.Width - 240, newBitmap.Height - 220).Save(rotatePath, ImageFormat.Jpeg);
                        GC.Collect();
                        //}
                    }
                    for (int q = 2020; q < 2160; q++)
                    {
                        int x = 0, y = 0, width = 500, height = 400;
                        //using (Bitmap CroppedImage = newBitmap.Clone(new Rectangle(20, 20, 280, 480), newBitmap.PixelFormat))
                        //{
                        var rotatePath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        RotateImage(newBitmap, (float)q, newBitmap.Width - 50, newBitmap.Height - 280).Save(rotatePath, ImageFormat.Jpeg);
                        GC.Collect();
                        //}
                    }
                    for (int q = 2160; q < 2700; q++)
                    {
                        var rand = new Random();

                        for (int r = 0; r < 150; r++)
                        {
                            var x = 1;
                            var y = 0;
                            while (x > y)
                            {
                                x = rand.Next(1, 300);
                                y = rand.Next(1, 300);
                            }

                            if (x >= newBitmap.Width) x = newBitmap.Width / 2;
                            if (y >= newBitmap.Height) y = newBitmap.Height / 2;

                            var x1 = rand.Next(1, 256);
                            newBitmap.SetPixel(x, y, Color.FromArgb(x1, x1, x1));
                        }
                        //for (int r = 0; r < 1000; r++)
                        //{
                        //    var x = 1;
                        //    var y = 0;
                        //    while (x > y)
                        //    {
                        //        x = rand.Next(1, 300);
                        //        y = rand.Next(1, 300);
                        //    }
                            

                        //    if (x >= newBitmap.Width) x = newBitmap.Width / 2;
                        //    if (y >= newBitmap.Height) y = newBitmap.Height / 2;

                        //    var x1 = rand.Next(1, 256);
                        //    newBitmap.SetPixel(x, y, Color.White);
                        //}
                        var cropPath = getFinalPath(path, $"{dirName}_v{q}.jpg");
                        var img = CropImage(newBitmap, rand.Next(100, 200), rand.Next(250, 400));
                        img.Save(cropPath);
                        GC.Collect();
                    }


                }
            }
            catch (OutOfMemoryException x)
            {
               
            }

            GC.Collect();

            try
            {
                using (Bitmap newBitmap = ((Bitmap)System.Drawing.Image.FromFile($"{path}\\{dirName}_4.jpg")))
                {

                    try
                    {

                        for (int q = 3060; q < 3600; q++)
                        {
                            var rand = new Random();


                            for (int r = 0; r < 150; r++)
                            {
                                var xx = 1;
                                var yx = 0;
                                while (xx > yx)
                                {
                                    xx = rand.Next(1, 300);
                                    yx = rand.Next(1, 300);
                                }

                                if (xx >= newBitmap.Width) xx = newBitmap.Width / 2;
                                if (yx >= newBitmap.Height) yx = newBitmap.Height / 2;

                                var x1 = rand.Next(1, 256);

                                newBitmap.SetPixel(xx, yx, Color.FromArgb(x1, x1, x1));
                            }
                            //for (int r = 0; r < 1000; r++)
                            //{
                            //    var x = 1;
                            //    var y = 0;
                            //    while (x > y)
                            //    {
                            //        x = rand.Next(1, 300);
                            //        y = rand.Next(1, 300);
                            //    }

                            //    if (x >= newBitmap.Width) x = newBitmap.Width / 2;
                            //    if (y >= newBitmap.Height) y = newBitmap.Height / 2;

                            //    var x1 = rand.Next(1, 256);
                            //    newBitmap.SetPixel(x, y, Color.White);
                            //}

                            var cropPath = getFinalPath(path, $"{dirName}_validate{q}.jpg");
                            var img = CropImage(newBitmap, rand.Next(200, 250), rand.Next(350, 400));
                            img.Save(cropPath);
                            GC.Collect();
                        }
                    }
                    catch (OutOfMemoryException x)
                    {
                        //System.Runtime.GCSettings.LargeObjectHeapCompactionMode = System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;
                        //GC.Collect();
                    }
                }
            }
            catch (OutOfMemoryException x)
            {

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
