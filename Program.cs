namespace Morninger
{
    using System.Net;
    using System;
    using Telegram.Bot.Args;
    using Telegram.Bot;
    using Services;

    public class Program
    {
        internal static ITelegramBotClient botClient;

        static void Main()
        {
            var webProxy = Console.ReadLine();
            var botId = Console.ReadLine();
            var httpProxy = new WebProxy(webProxy);
            botClient = new TelegramBotClient(botId, httpProxy);
            botClient.SetWebhookAsync("");
            botClient.OnMessage += onMessage;
            botClient.StartReceiving();
            Console.WriteLine("Working...");
            while (Console.ReadLine() != "Exit") ;
        }

        static void onMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null && e.Message.Text != string.Empty)
            {
                var answer = Worker.ProcessMessage(e.Message);
                if (answer != null && answer != string.Empty)
                {
                    botClient.SendTextMessageAsync(e.Message.Chat.Id, answer);
                }
            }
        }
    }
}