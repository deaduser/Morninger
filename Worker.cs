using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;

namespace Morninger.Services
{
    internal class Worker
    {
        private string rootDirectoryPath;

        internal Worker(string directoryPath)
        {
            this.rootDirectoryPath = directoryPath;
        }

        internal string FilePath
        {
            get { return $"{rootDirectoryPath}\\{DateTime.UtcNow.Year}\\{DateTime.UtcNow.Month}.CSV"; }
        }

        internal string DirectoryPath
        {
            get { return $"{rootDirectoryPath}\\{DateTime.UtcNow.Year}"; }
        }

        internal string ProcessMessage(Telegram.Bot.Types.Message message)
        {
            Console.WriteLine($"\nMessage '{message.Text}' received");

            CreateFileIfNotExist();

            var records = Read();

            var r = records.FirstOrDefault(o => o.UserId == message.From.Id);

            if (r == null)
            {
                Console.WriteLine("No records found");
                r = new Record { UserId = message.From.Id };
                records.Add(r);
                Console.WriteLine("First record added");
            }
            else
            {
                Console.WriteLine($"{records.Count} records found");
            }

            switch (message.Text.Replace("@morningerbot", ""))
            {
                case "/done":
                    r.Done = r.Done + 1;
                    break;
                case "/ill":
                    r.Ill = r.Ill + 1;
                    break;
                case "/stat":
                    return $"Done: {r.Done}\nUndone: {r.Undone}\nIll: {r.Ill}";
                default:
                    return "Comand is not recognized";
            }

            if (r.LastUpdate != DateTime.UtcNow.Day)
            {
                r.LastUpdate = DateTime.UtcNow.Day;
            }
            else
            {
                return "You are already marked today";
            }

            Write(records);
            return null;
        }

        internal List<Record> Read()
        {
            using (var reader = new StreamReader(FilePath))
            using (var csv = new CsvReader(reader))
            {
                var res = csv.GetRecords<Record>().ToList();
                Console.WriteLine($"Read from file {res.Count} records");
                return res;
            }
        }

        internal void CreateFileIfNotExist()
        {
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
                Console.WriteLine("Directory created");
            }

            if (!File.Exists(FilePath))
            {
                using (var file = File.Create(FilePath))
                {
                    Console.WriteLine("File created");
                    using (var writer = new StreamWriter(file))
                    using (var csv = new CsvWriter(writer))
                    {
                        csv.WriteHeader<Record>();
                        csv.NextRecord();
                        Console.WriteLine("Headers created");
                    }
                }
            }
        }

        internal void Write(List<Record> records)
        {
            using (var writer = new StreamWriter(FilePath))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(records);
                Console.WriteLine($"{records.Count} written to the file");
            }
        }
    }

    internal class Record
    {
        public int UserId { get; set; }
        public int LastUpdate { get; set; }
        public int Done { get; set; }
        public int Ill { get; set; }
        public int Undone
        {
            get
            {
                return DateTime.UtcNow.Day - Done - Ill;
            }
        }
    }
}