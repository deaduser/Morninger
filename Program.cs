namespace Morninger
{
    using System.Net;
    using System;
    using Telegram.Bot.Args;
    using Telegram.Bot;

    public class Program
    {
        private static ITelegramBotClient botClient;
        private static Service service;
        private static DB db;

        static void Main(string[] args)
        {
            Console.WriteLine("Path to db:");
            var pathToDb = args.Length >= 1 ? args[0] : Console.ReadLine();
            db = new DB(pathToDb);
            service = new Service();

            Console.WriteLine("HTTP proxy address:");
            var webProxy = args.Length >= 2 ? args[1] : Console.ReadLine();
            var httpProxy = webProxy == string.Empty ? null : new WebProxy(webProxy);

            Console.WriteLine("Bot ID:");
            var botId = args.Length >= 3 ? args[2] : Console.ReadLine();
            botClient = webProxy == string.Empty ? new TelegramBotClient(botId) : new TelegramBotClient(botId, httpProxy);

            botClient.SetWebhookAsync("");
            botClient.OnMessage += onMessage;
            botClient.StartReceiving();
            Console.WriteLine("App started...");

            while (Console.ReadLine() != "Exit") ;
        }

        static void onMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null && e.Message.Text != string.Empty)
            {
                var answer = service.ProcessMessage(db, e.Message);
                if (answer != null && answer != string.Empty)
                {
                    botClient.SendTextMessageAsync(e.Message.Chat.Id, answer);
                }
            }
        }
    }
}