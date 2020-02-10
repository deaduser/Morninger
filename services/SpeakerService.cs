namespace Morninger
{
    using System;
    using System.Linq;

    internal class SpeakerService
    {
        internal string ProcessMessage(SQLiteProvider db, Telegram.Bot.Types.Message m)
        {
            Console.WriteLine($"\n{nameof(ProcessMessage)}");
            Console.WriteLine($"{DateTime.UtcNow}\nMessage: '{m.Text}'\nFrom: {m.From.Id} {m.From.FirstName} {m.From.Username} {m.From.LastName}");

            var user = SelectOrCreateUser(db, m.From);

            switch (m.Text.Replace("@morningerbot", ""))
            {
                case "/done": return ProcessDoneMessage(db, user);
                case "/dayoff": return ProcessDayOffMessage(db, user);
                case "/start": 
                case "/stat": return ProcessStatMessage(db, user);
                default: return ProcessDefaultMessage(db, user);
            }
        }

        private User CreateUser(SQLiteProvider db, Telegram.Bot.Types.User user)
        {
            Console.WriteLine($"{nameof(CreateUser)}");

            var newUser = new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                LastUpdate = DateTime.UtcNow,
            };

            newUser.Statistic.Add(new Month(newUser.Id) { LastUpdate = DateTime.MinValue });

            db.InsertUser(newUser);
            db.InsertMonth(newUser.Statistic.First());

            Console.WriteLine("User added to DB");
            return newUser;
        }

        private string ProcessDefaultMessage(SQLiteProvider db, User user)
        {
            Console.WriteLine($"{nameof(ProcessDefaultMessage)}");
            return string.Empty;
        }

        private string ProcessStatMessage(SQLiteProvider db, User user)
        {
            Console.WriteLine($"{nameof(ProcessStatMessage)}");
            var m = SelectOrCreateCurrentMonth(db, user);
            return $"Done: {m.Done}\nUndone: {m.Undone}\nDayOff: {m.DayOff}";
        }

        private string ProcessDayOffMessage(SQLiteProvider db, User user)
        {
            Console.WriteLine($"{nameof(ProcessDayOffMessage)}");
            var m = SelectOrCreateCurrentMonth(db, user);

            if (m.LastUpdate.Date == DateTime.UtcNow.Date)
            {
                return "You already marked today. I wish you a good day!";
            }

            m.DayOff = m.DayOff + 1;
            m.LastUpdate = DateTime.UtcNow;
            db.UpdateMonth(m);
            return string.Empty;
        }

        private string ProcessDoneMessage(SQLiteProvider db, User user)
        {
            Console.WriteLine($"{nameof(ProcessDoneMessage)}");
            var m = SelectOrCreateCurrentMonth(db, user);

            if (m.LastUpdate.Date == DateTime.UtcNow.Date)
            {
                return "You already marked today. I wish you a good day!";
            }

            m.Done = m.Done + 1;
            m.LastUpdate = DateTime.UtcNow;
            db.UpdateMonth(m);
            return string.Empty;
        }

        private Month SelectOrCreateCurrentMonth(SQLiteProvider db, User user)
        {
            Console.WriteLine($"{nameof(SelectOrCreateCurrentMonth)}");

            var m = user.Statistic.Where(s => s.Year == DateTime.UtcNow.Year && s.Number == DateTime.UtcNow.Month).FirstOrDefault();
            if (m == null)
            {
                m = new Month(user.Id);
                m.LastUpdate = DateTime.MinValue;
                db.InsertMonth(m);
            }
            return m;
        }

        private User SelectOrCreateUser(SQLiteProvider db, Telegram.Bot.Types.User tUser)
        {
            Console.WriteLine($"{nameof(SelectOrCreateUser)}");

            var user = db.SelectUser(tUser.Id);

            if (user == null)
            {
                user = CreateUser(db, tUser);
            }
            else
            {
                user.FirstName = tUser.FirstName;
                user.LastName = tUser.LastName;
                user.Username = tUser.Username;
                user.LastUpdate = DateTime.UtcNow;
                db.UpdateUser(user);
            }

            return user;
        }
    }
}