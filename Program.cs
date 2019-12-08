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
            var httpProxy = new WebProxy(Address: "70.169.132.131:48678");
            botClient = new TelegramBotClient("995252338:AAE0SzILY9wBPeM5KmHsbFNtPZMr8AhvYXo", httpProxy);
            botClient.SetWebhookAsync("");
            botClient.OnMessage += onMessage;
            botClient.StartReceiving();

            do
            {
                Console.WriteLine("Input 'exit' for closing");
            }
            while (Console.ReadLine() == "exit");
        }

        static void onMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text == null || e.Message.Chat.Title == null)return;
            if (!e.Message.Text.StartsWith("/"))return;

            var answer = DataHelper.ProcessMessage(e.Message);
            if (answer == null) return;
            SendMessage(e.Message.Chat.Id, answer);
        }

        static async void SendMessage(long chatId, string message)
        {
            await botClient.SendTextMessageAsync(chatId, message);
        }
    }
}