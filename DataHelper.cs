namespace morninger
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System;

    using CsvHelper;

    internal static class DataHelper
    {
        internal static string ProcessMessage(Telegram.Bot.Types.Message message)
        {
            var statistics = ReadStatisticsFromFile();
            var statistic = statistics.FirstOrDefault(o => o.UserId == message.From.Id);
            if (statistic == null)
            {
                statistic = RegisteNewStatistic(statistics, message.From);
            }
            var answer = ProcessMessage(statistic, message.From, message.Text);
            SaveStatisticIntoFile(statistics);
            return answer;
        }

        internal static string ShowStatistic(Statistic s, Telegram.Bot.Types.User user)
        {
            return $"{s.UserName}:\nDone - {s.Done},\nUndone - {s.Undone},\nIll - {s.Ill}";
        }

        private static Statistic RegisteNewStatistic(List<Statistic> statistics, Telegram.Bot.Types.User user)
        {
            var username = user.Username != null? $"{user.FirstName} ({user.Username}) {user.LastName}": $"{user.FirstName} {user.LastName}";
            var statistic = new Statistic(user.Id, username);
            statistics.Add(statistic);
            return statistic;
        }

        private static string ProcessMessage(Statistic s, Telegram.Bot.Types.User user, string message)
        {
            var command = ToCommand(message);
            if (command == Command.Unknown) return null;
            if (command == Command.Statistic) return ShowStatistic(s, user);

            if (s.LastSeen == DateTime.UtcNow.ToString("yy/MM/dd")) return "Today has already been updated";
            s.LastSeen = DateTime.UtcNow.ToString("yy/MM/dd");

            switch (command)
            {
                case Command.Done:
                    s.Done++;
                    return null;
                case Command.Undone:
                    s.Undone++;
                    return null;
                case Command.Ill:
                    s.Ill++;
                    return null;
            }

            return null;
        }

        static private Command ToCommand(string message)
        {
            if (message.StartsWith("/statistic")) return Command.Statistic;
            else if (message.StartsWith("/done")) return Command.Done;
            else if (message.StartsWith("/undone")) return Command.Undone;
            else if (message.StartsWith("/ill")) return Command.Ill;
            return Command.Unknown;
        }

        private static List<Statistic> ReadStatisticsFromFile()
        {
            Directory.CreateDirectory("statistics");
            var path = $"statistics/{DateTime.UtcNow.Year}{DateTime.UtcNow.Month}.csv";
            if (!File.Exists(path))
            {
                CreateFile(path, "UserId,UserName,LastSeen,Done,Undone,Ill");
            }

            using(var f = File.Open(path, FileMode.OpenOrCreate, FileAccess.Read))
            using(var sr = new StreamReader(f))
            using(var csv = new CsvReader(sr))
            {
                var result = csv.GetRecords<Statistic>();
                return result.ToList();
            }
        }

        private static void SaveStatisticIntoFile(List<Statistic> records)
        {
            Directory.CreateDirectory("statistics");
            var path = $"statistics/{DateTime.UtcNow.Year}{DateTime.UtcNow.Month}.csv";
            CreateFile(path, "UserId,UserName,LastSeen,Done,Undone,Ill");

            using(var f = File.Open(path, FileMode.Open, FileAccess.Write))
            using(var sr = new StreamWriter(f))
            using(var csv = new CsvWriter(sr))
            {
                csv.WriteRecords(records);
            }
        }

        private static void CreateFile(string path, string headers)
        {
            using(var f = File.Open(path, FileMode.Create, FileAccess.ReadWrite))
            using(var sw = new StreamWriter(f))
            {
                sw.WriteLine(headers);
            }
        }
    }
}