namespace Morninger
{
    using System;
    using System.Linq;

    internal class Service
    {
        internal string ProcessMessage(DB db, Telegram.Bot.Types.Message m)
        {
            Console.WriteLine($"\n{nameof(ProcessMessage)}");
            Console.WriteLine($"{DateTime.UtcNow}\nMessage: '{m.Text}'\nFrom: {m.From.Id} {m.From.FirstName} {m.From.Username} {m.From.LastName}");

            var user = db.SelectUser(m.From.Id);

            if (user == null)
            {
                user = CreateUser(db, m.From);
            }

            switch (m.Text.Replace("@morningerbot", ""))
            {
                case "/done": return ProcessDoneMessage(db, user);
                case "/dayoff": return ProcessDayOffMessage(db, user);
                case "/stat": return ProcessStatMessage(db, user);
                default: return ProcessDefaultMessage(db, user);
            }
        }

        private User CreateUser(DB db, Telegram.Bot.Types.User user)
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

            newUser.Statistic.Add(new Month
            {
                UserId = newUser.Id,
                Year = DateTime.UtcNow.Year,
                Number = DateTime.UtcNow.Month
            });

            db.InsertUser(newUser);
            db.InsertMonth(newUser.Statistic.First());

            Console.WriteLine("User added to DB");
            return newUser;
        }

        private string ProcessDefaultMessage(DB db, User user)
        {
            Console.WriteLine($"{nameof(ProcessDefaultMessage)}");
            return string.Empty;
        }

        private string ProcessStatMessage(DB db, User user)
        {
            Console.WriteLine($"{nameof(ProcessStatMessage)}");
            return string.Empty;
        }

        private string ProcessDayOffMessage(DB db, User user)
        {
            Console.WriteLine($"{nameof(ProcessDayOffMessage)}");
            return string.Empty;
        }

        private string ProcessDoneMessage(DB db, User user)
        {
            Console.WriteLine($"{nameof(ProcessDoneMessage)}");

            if (user.LastUpdate == DateTime.UtcNow.Date)
            {
                return "You already marked today. I wish you a good day!";
            }
            
            return string.Empty;
        }
    }
}