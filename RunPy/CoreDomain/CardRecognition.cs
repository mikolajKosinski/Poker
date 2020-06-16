using System;
using System.Diagnostics;
using System.IO;

namespace CoreDomain
{
    public class ImageRecognition
    {
        public string RecogniseByPath(string path)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "C:\\Python\\python.exe";
            start.Arguments = string.Format("C:\\Users\\mkosi\\PycharmProjects\\tensorEnv\\PP.py {0} ", "C:\\Users\\mkosi\\PycharmProjects\\tensorEnv\\dataset\\7H\\test.jpg");
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    return reader.ReadToEnd().Replace("\r\n", "");                    
                }
            }
        }
    }
}
