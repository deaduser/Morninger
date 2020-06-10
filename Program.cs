namespace Edomozh
{
    using System.Net;
    using System;
    using Telegram.Bot.Args;
    using Telegram.Bot;

    public class Program
    {
        private static ITelegramBotClient telegramBot;
        private static DataHelper dataHelper;

        public static void Main(string[] args)
        {
            dataHelper = InitMorninger(InitDataBase(args[0]));
            telegramBot = InitTelegramBot(InitProxy(args[1]), args[2]);
            while (Console.ReadLine() != "exit") ;
        }

        static void OnMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine(nameof(OnMessage));

            try
            {
                if (e.Message.Text == null && e.Message.Text == string.Empty) return;
                var answer = dataHelper.ProcessMessage(e.Message);

                if (answer == string.Empty) return;
                telegramBot.SendTextMessageAsync(e.Message.Chat.Id, answer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #region Init

        private static SQLiteProvider InitDataBase(string path)
        {
            Console.WriteLine(nameof(InitDataBase));
            return new SQLiteProvider();
        }

        private static DataHelper InitMorninger(SQLiteProvider db)
        {
            Console.WriteLine(nameof(InitMorninger));
            return new DataHelper(db);
        }

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