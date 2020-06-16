using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Numpy;
using NumpyDotNet;
using Keras.PreProcessing.Image;
using Keras;

namespace RunPy
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "C:\\Python\\python.exe";
            start.Arguments = string.Format("C:\\Users\\mkosi\\PycharmProjects\\tensorEnv\\PP.py {0} ", "C:\\Users\\mkosi\\PycharmProjects\\tensorEnv\\dataset\\AD\\test.jpg");
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd().Replace("\r\n", "");
                    Console.Write(result);
                    Console.ReadKey();
                }
            }
        }
    }
}
