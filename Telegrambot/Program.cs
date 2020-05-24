using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Net.Http;
using Newtonsoft.Json;
using ApiAiSDK;

namespace Telegrambot
{
    // 1198087347:AAEJev36e5wO_hc8lITueqFCYwBO4Vsotsw     
    // 1f16b4d3f69c42ba80c54de1ae14c2ae
    class Program
    {
        static Dictionary<int, string> status = new Dictionary<int, string>();
        static TelegramBotClient Bot;
        static Dictionary<int, int> n = new Dictionary<int, int>();
        static ApiAi apiAi;
        static string host = "https://webapplication120200523232618.azurewebsites.net";
        static string localhost = "https://localhost:44338";
        static void Main(string[] args)
        {
            host = localhost;
            AIConfiguration config = new AIConfiguration("1f16b4d3f69c42ba80c54de1ae14c2ae", SupportedLanguage.Russian);
            apiAi = new ApiAi(config);
            Console.WriteLine("Hello World!");
            Bot = new TelegramBotClient("1198087347:AAEJev36e5wO_hc8lITueqFCYwBO4Vsotsw");
            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            var me = Bot.GetMeAsync().Result;
            Console.WriteLine(me.FirstName);
            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        class log
        {
            public int id { get; set; }
            public string info { get; set; }
        }
        private static async void BotOnCallbackQueryReceived(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            var client = new HttpClient();
            string buttonText = e.CallbackQuery.Data;
            string name = $"{ e.CallbackQuery.From.FirstName} {e.CallbackQuery.From.LastName}";
            Console.WriteLine($"{name} натиснув кнопку {buttonText}");

            log l = new log { id = e.CallbackQuery.From.Id, info = $"{name} натиснув кнопку {buttonText}" };
            var json = JsonConvert.SerializeObject(l);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = host + "/api/Test";
            var response = await client.PostAsync(url, data);
            string result = response.Content.ReadAsStringAsync().Result;

            try
            {
                switch (buttonText)
                {
                    case "Цитата":
                        string content = await client.GetStringAsync(host + "/api/Test");
                        await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, content.Replace("\\n", "\n"));
                        break;
                    case "Погода":
                        await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Для отримання погоди, введи в чат назву мiста англ мовою");
                        status[e.CallbackQuery.From.Id] = "Погода";
                        break;
                    case "Фільм":
                        await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Для отримання інформації про фільм, введи в чат назву фільму англ мовою");
                        status[e.CallbackQuery.From.Id] = "Фільм";
                        break;
                    case "Рецепт":
                        await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Для отримання рецептів, введи в чат назву рецепту англ мовою");
                        status[e.CallbackQuery.From.Id] = "Рецепт";
                        break;
                    case "Анекдот":
                        content = await client.GetStringAsync(host + "/api/Test/text/1");
                        await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, content);
                        break;
                    case "історія чату":
                        content = await client.GetStringAsync(host + "/api/Test/hisory/" + e.CallbackQuery.From.Id);
                        await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, content.Replace("\\n", "\n"));
                        break;
                    case "видалити запис з історії":
                        await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Введи номер запису, який бажаєш видалити (-1 якщо бажаєш повністю очистити історію)");
                        status[e.CallbackQuery.From.Id] = "видалити запис з історії";
                        break;
                    case "редагувати історію":
                        await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Введи номер запису, який бажаєш редагувати ");
                        status[e.CallbackQuery.From.Id] = "редагувати історію";
                        break;
                }
                await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"ви нажали кнопку {buttonText}");
            }
            catch
            {

            }


        }

        private static async void BotOnMessageReceived(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var client = new HttpClient();
            var message = e.Message;
            if (message == null || message.Type != MessageType.Text)
                return;

            string name = $"{message.From.FirstName} {message.From.LastName}";
            Console.WriteLine($"{name} відправив повідомлення: '{message.Text}'");

            log l = new log { id = message.From.Id, info = $"{name} відправив повідомлення: '{message.Text}'" };
            var json = JsonConvert.SerializeObject(l);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = host + "/api/Test";
            var response = await client.PostAsync(url, data);
            string result = response.Content.ReadAsStringAsync().Result;
            //Console.WriteLine(result);


            try
            {
                if (status[message.From.Id] == null) status[message.From.Id] = "";
            }
            catch
            {
                status[message.From.Id] = "";
            }
            if (status[message.From.Id] == "")
            {
                switch (message.Text)
                {
                    case "/start":
                        string text =
    @"Список команд:
/start - запуск бота
/callback - вывод меню";
                        //keyboard - вывод клавиатуры";
                        await Bot.SendTextMessageAsync(message.From.Id, text);
                        break;
                    case "/callback":
                        var inlineKeyboard = new InlineKeyboardMarkup(new[]
                        {
                        new[]
                        {
                            //InlineKeyboardButton.WithUrl("VK","https://vk.com"),
                            //InlineKeyboardButton.WithUrl("Telegram","https://t.me"),
                            InlineKeyboardButton.WithCallbackData("Рецепт"),
                            InlineKeyboardButton.WithCallbackData("Анекдот")
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Цитата"),
                            InlineKeyboardButton.WithCallbackData("Погода"),
                            InlineKeyboardButton.WithCallbackData("Фільм"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("історія чату"),
                            InlineKeyboardButton.WithCallbackData("видалити запис з історії"),
                            InlineKeyboardButton.WithCallbackData("редагувати історію"),
                        }
                    });
                        await Bot.SendTextMessageAsync(message.From.Id, "Вибери пункт меню", replyMarkup: inlineKeyboard);
                        break;
                    //case "/keyboard":
                    //    var replyKeyboard = new ReplyKeyboardMarkup(new[]
                    //        {
                    //    new[]
                    //    {
                    //        new KeyboardButton("Привет"),
                    //        new KeyboardButton("Как дела?"),
                    //    },
                    //    new[]
                    //    {
                    //        new KeyboardButton("Контакт") { RequestContact = true},
                    //        new KeyboardButton("геолокация") {RequestLocation = true}
                    //    }
                    //});
                    //    await Bot.SendTextMessageAsync(message.Chat.Id, "Сообщение", replyMarkup: replyKeyboard);
                    //    break;
                    default:
                        var response2 = apiAi.TextRequest(message.Text);
                        string answer = response2.Result.Fulfillment.Speech;
                        if (answer == "")
                            answer = "Прости, я тебя не понял";
                        await Bot.SendTextMessageAsync(message.From.Id, answer);
                        break;
                }
            }
            else
            {
                switch (status[message.From.Id])
                {
                    case "Погода":
                        string content = await client.GetStringAsync(host + "/api/Test/" + message.Text);
                        await Bot.SendTextMessageAsync(message.From.Id, content.Replace("\\n", "\n"));
                        status[message.From.Id] = "";
                        break;
                    case "Фільм":
                        content = await client.GetStringAsync(host + "/api/Test/film/" + message.Text);
                        await Bot.SendTextMessageAsync(message.From.Id, content.Replace("\\n", "\n").Replace("\\\"", "\""));
                        status[message.From.Id] = "";
                        break;
                    case "Рецепт":
                        content = await client.GetStringAsync(host + "/api/Test/resept/" + message.Text);
                        await Bot.SendTextMessageAsync(message.From.Id, content.Replace("\\n", "\n").Replace("\\\"", "\""));
                        status[message.From.Id] = "";
                        break;
                    case "видалити запис з історії":
                        await client.DeleteAsync(host + "/api/Test/" + message.Text + "/" + message.From.Id);
                        status[message.From.Id] = "";
                        break;
                    case "редагувати історію":
                        try
                        {
                            n[message.From.Id] = int.Parse(message.Text);
                            await Bot.SendTextMessageAsync(message.From.Id, "Введи на що його треба змінити: ");
                            status[message.From.Id] = "продовження редагування";
                        }
                        catch
                        {
                            await Bot.SendTextMessageAsync(message.From.Id, "Не вдалося його перетворити на ціле число, ще раз введи номер запису, який бажаєш редагувати");
                        }
                        break;
                    case "продовження редагування":
                        l = new log { id = message.From.Id, info = message.Text };
                        json = JsonConvert.SerializeObject(l);
                        data = new StringContent(json, Encoding.UTF8, "application/json");
                        url = host + "/api/Test/" + n[message.From.Id];
                        response = await client.PutAsync(url, data);
                        status[message.From.Id] = "";
                        break;
                }
            }
        }
    }
}
