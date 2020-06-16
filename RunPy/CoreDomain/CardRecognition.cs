using System;
using System.Diagnostics;
using System.IO;

namespace CoreDomain
{
    public class CardRecognition : ICardRecognition
    {
        public string RecogniseByPath(string path)
        {
            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = "C:\\Python\\python.exe",
                Arguments = $"C:\\Users\\mkosi\\PycharmProjects\\tensorEnv\\PP.py {path}",
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            using Process process = Process.Start(start);
            using StreamReader reader = process.StandardOutput;
            return reader.ReadToEnd().Replace("\r\n", "");
        }
    }
}
