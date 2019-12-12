namespace Morninger
{
    using System.Net;
    using System;
    using Telegram.Bot.Args;
    using Telegram.Bot;
    using DataLayer;
    using System.IO;
    using Models;
    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.FileExtensions;
    using Microsoft.Extensions.Configuration.Json;
    using Services;
    using Settings;

    public class Program
    {
        internal static ITelegramBotClient botClient;
        internal static IConfigurationRoot Configuration;
        internal static Storage Storage;


        static void Main(string[] args)
        {
            Storage = new Storage();
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();

            Configuration.Bind("Storage", Storage);

            using (var db = new MorningerBotContext())
            {
                db.Add(new Setting { Name = "Test" });
                db.SaveChanges();
                var s = db.Settings.OrderBy(b => b.Name).First();
                Console.WriteLine($"Settings name: {s.Name}");
            }

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

            Logger.Log($"{DateTime.Now}: {e.Message.From.FirstName} {e.Message.Text}");
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