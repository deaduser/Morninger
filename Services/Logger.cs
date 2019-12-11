namespace Morninger.Services
{
    using System.IO;
    using System;

    internal  static class Logger
    {
        internal  static void Log(string line)
        {
            Directory.CreateDirectory("../log");
            var path = $"../log/{DateTime.UtcNow.Year}{DateTime.UtcNow.Month}{DateTime.UtcNow.Day}.txt";
            using(var f = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
            using(var sw = new StreamWriter(f))
            {
                sw.WriteLine(line);
                Console.WriteLine(line);
            }
        }
    }
}