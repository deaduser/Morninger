namespace Edomozh
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
            Console.WriteLine($"Time: {DateTime.UtcNow} From: {m.From.Id}\n Message: '{m.Text}'");

            var user = SelectUser(m.From);
            if (user == null) return Resources.YouAreNotRegisteredMessage;

            switch (m.Text.Replace(Resources.MorningerBotName, string.Empty))
            {
                case "/done": return ProcessDoneMessage(user);
                case "/dayoff": return ProcessDayOffMessage(user);
                case "/start": return CreateUser(m.From);
                case "/stat": return ProcessStatMessage(user);
                default: return ProcessDefaultMessage(user);
            }
        }

        private string CreateUser(Telegram.Bot.Types.User user)
        {
            Console.WriteLine(nameof(CreateUser));

            var checkUser = SelectUser(user);
            if (checkUser != null) return Resources.YouAreRegisteredMessage;

            var newUser = new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                LastUpdate = DateTime.UtcNow,
            };

            newUser.Statistic.Add(new Month(newUser.Id) { LastUpdate = DateTime.MinValue });

            dbProvider.InsertUser(newUser);
            dbProvider.InsertMonth(newUser.Statistic.First());

            return Resources.WelcomeMessage;
        }

        private string ProcessDefaultMessage(User user)
        {
            Console.WriteLine(nameof(ProcessDefaultMessage));
            return string.Empty;
        }

        private string ProcessStatMessage(User user)
        {
            Console.WriteLine(nameof(ProcessStatMessage));
            var m = SelectOrCreateCurrentMonth(user);
            return $"Done: {m.Done}\nUndone: {m.Undone}\nDayOff: {m.DayOff}";
        }

        private string ProcessDayOffMessage(User user)
        {
            Console.WriteLine(nameof(ProcessDayOffMessage));
            var m = SelectOrCreateCurrentMonth(user);

            if (m.LastUpdate.Date == DateTime.UtcNow.Date) return Resources.AlreadyCheckedInMessage;

            m.DayOff = m.DayOff + 1;
            m.LastUpdate = DateTime.UtcNow;
            dbProvider.UpdateMonth(m);
            return string.Empty;
        }

        private string ProcessDoneMessage(User user)
        {
            Console.WriteLine(nameof(ProcessDoneMessage));
            var m = SelectOrCreateCurrentMonth(user);

            if (m.LastUpdate.Date == DateTime.UtcNow.Date) return Resources.AlreadyCheckedInMessage;

            m.Done = m.Done + 1;
            m.LastUpdate = DateTime.UtcNow;
            dbProvider.UpdateMonth(m);
            return string.Empty;
        }

        private Month SelectOrCreateCurrentMonth(User user)
        {
            Console.WriteLine(nameof(SelectOrCreateCurrentMonth));

            var m = user.Statistic.Where(s => s.Year == DateTime.UtcNow.Year && s.Number == DateTime.UtcNow.Month).FirstOrDefault();
            if (m == null)
            {
                m = new Month(user.Id);
                m.LastUpdate = DateTime.MinValue;
                dbProvider.InsertMonth(m);
            }
            return m;
        }

        private User SelectUser(Telegram.Bot.Types.User tUser)
        {
            Console.WriteLine(nameof(SelectUser));
            return dbProvider.SelectUser(tUser.Id);
        }
    }
}