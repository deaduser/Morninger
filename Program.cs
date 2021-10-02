namespace CompetitorsBot.Bot
{
    using System.Net;
    using System;
    using Telegram.Bot.Args;
    using Telegram.Bot;
    using System.Linq;
    using CompetitorsBot.Bot.Models;

    public class Program
    {
        private static ITelegramBotClient telegramBot;

        public static void Main(string[] args)
        {
            using (var db = new CompetitorsBotContext())
            {
                // Note: This sample requires the database to be created before running.

                // Create
                Console.WriteLine("Inserting a new blog");
                db.Add(new User { FirstName = "Kavya" });
                db.SaveChanges();

                // Read
                Console.WriteLine("Querying for a blog");
                var entity = db.Users.OrderBy(b => b.FirstName).First();

                // Update
                Console.WriteLine("Updating the blog and adding a post");
                entity.FirstName = "https://devblogs.microsoft.com/dotnet";
                entity.Entries.Add(new Entry { Status = "Hello World" });
                db.SaveChanges();

                // Delete
                Console.WriteLine("Delete the blog");
                db.Remove(entity);
                db.SaveChanges();
            }

            //dataHelper = InitMorninger(InitDataBase(args[0]));
            telegramBot = InitTelegramBot(InitProxy(args[1]), args[2]);
            while (Console.ReadLine() != "exit") ;
        }

        static void OnMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine(nameof(OnMessage));

            try
            {
                if (e.Message.Text == null && e.Message.Text == string.Empty) return;
                var answer = ""; //dataHelper.ProcessMessage(e.Message);

                if (answer == string.Empty) return;
                telegramBot.SendTextMessageAsync(e.Message.Chat.Id, answer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #region Init

        //private static SQLiteProvider InitDataBase(string path)
        //{
        //    Console.WriteLine(nameof(InitDataBase));
        //    return new SQLiteProvider();
        //}

        //private static DataHelper InitMorninger(SQLiteProvider db)
        //{
        //    Console.WriteLine(nameof(InitMorninger));
        //    return new DataHelper(db);
        //}

        private static IWebProxy InitProxy(string address)
        {
            Console.WriteLine(nameof(InitProxy));
            return address != string.Empty ? new WebProxy(address) : null;
        }

        private static ITelegramBotClient InitTelegramBot(IWebProxy proxy, string apitoken)
        {
            Console.WriteLine(nameof(InitTelegramBot));
            var bot = proxy == null
                ? new TelegramBotClient(apitoken)
                : new TelegramBotClient(apitoken, proxy);
            bot.SetWebhookAsync("");
            bot.OnMessage += OnMessage;
            bot.StartReceiving();
            return bot;

        }

        #endregion Init
    }
}