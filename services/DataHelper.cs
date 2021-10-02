namespace CompetitorsBot.Bot.Services
{
    using System;
    using System.Linq;
    using Properties;

    internal class DataHelper
    {
        SQLiteProvider dbProvider;

        internal DataHelper(SQLiteProvider db)
        {
            dbProvider = db;
        }

        internal string ProcessMessage(Telegram.Bot.Types.Message m)
        {
            Console.WriteLine();
            Console.WriteLine(nameof(ProcessMessage));
            Console.WriteLine($"Time: {DateTime.UtcNow} From: {m.From.Id}\nMessage: '{m.Text}'");

            var command = m.Text.Replace(Resources.MorningerBotName, string.Empty);

            if (command == "/start") return CreateUser(m.From);

            var user = UserSelect(m.From);
            if (user == null) return Resources.YouAreNotRegisteredMessage;

            switch (command)
            {
                case "/done": return InsertEntry(user, "done");
                case "/dayoff": return InsertEntry(user, "dayoff");
                case "/stat": return GetStatistic(user);
                default: return ProcessDefaultMessage(user);
            }
        }

        private string CreateUser(Telegram.Bot.Types.User user)
        {
            Console.WriteLine(nameof(CreateUser));

            var checkUser = UserSelect(user);
            if (checkUser != null) return Resources.YouAreRegisteredMessage;

            var newUser = new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username
            };

            newUser.Entries.Add(new Entry(newUser.Id));

            dbProvider.UserInsert(newUser);

            return Resources.WelcomeMessage;
        }

        private string ProcessDefaultMessage(User user)
        {
            Console.WriteLine(nameof(ProcessDefaultMessage));
            return string.Empty;
        }

        private string GetStatistic(User user)
        {
            Console.WriteLine(nameof(GetStatistic));

            if (user.Entries.Count == 0) return Resources.ThereIsNoStats;

            var m = user.Entries.Where(e => IsThisMonthEntry(e));
            var done = m.Where(e => e.Status == "done").Count();
            var dayoff = m.Where(e => e.Status == "dayoff").Count();
            var undone = DateTime.UtcNow.Day - done - dayoff;

            return $"In this month:\nDone: {done}\nUndone: {undone}\nDayOff: {dayoff}";
        }

        private string InsertEntry(User user, string status)
        {
            Console.WriteLine(nameof(InsertEntry));

            if (user.Entries.Any(s => IsThisMonthEntry(s)))
                return Resources.AlreadyCheckedInMessage;

            var e = new Entry(user.Id) { Status = status };
            dbProvider.EntryInsert(e);

            return string.Empty;
        }

        private User UserSelect(Telegram.Bot.Types.User user)
        {
            Console.WriteLine(nameof(UserSelect));
            return dbProvider.UserSelect(user.Id);
        }

        private bool IsTodayEntry(Entry s)
        {
            return s.Year == DateTime.UtcNow.Year && s.Month == DateTime.UtcNow.Month && s.Day == DateTime.UtcNow.Day;
        }

        private bool IsThisMonthEntry(Entry s)
        {
            return s.Year == DateTime.UtcNow.Year && s.Month == DateTime.UtcNow.Month;
        }
    }
}