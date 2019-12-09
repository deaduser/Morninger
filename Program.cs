namespace morninger
{
    using System.Net;
    using System;

    using Telegram.Bot.Args;
    using Telegram.Bot;

    class Program
    {
        static ITelegramBotClient botClient;

        static void Main(string[] args)
        {
            var webProxy = Console.ReadLine();
            var botId = Console.ReadLine();
            var httpProxy = new WebProxy(webProxy);
            botClient = new TelegramBotClient(botId, httpProxy);
            botClient.SetWebhookAsync("");
            botClient.OnMessage += onMessage;
            botClient.StartReceiving();
            while (Console.ReadLine() != "Exit") ;
        }

        static void onMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text == null) return;
            if (!e.Message.Text.StartsWith("/")) return;

            var answer = DataHelper.ProcessCommand(e.Message);
            if (answer == null) return;
            SendMessage(e.Message.Chat.Id, answer);
        }

        static async void SendMessage(long chatId, string message)
        {
            await botClient.SendTextMessageAsync(chatId, message);
        }
    }
}