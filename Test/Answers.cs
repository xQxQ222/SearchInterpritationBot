using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using SearchInterpritationBot.ActionsWithWord.Tools;
using SearchInterpritationBot.ActionsWithWord.Parsers;

namespace SearchInterpritationBot.Test
{
    public class Answers
    {
        public readonly static ReplyKeyboardMarkup MainKeyBoard;

        public readonly static InlineKeyboardMarkup actionWithWordsKeyboard;

        private readonly static List<string> lastMessages = new List<string>();

        private readonly static InlineKeyboardMarkup definitionSources;

        public static Dictionary<string, Func<ITelegramBotClient, Chat, Task>> BotAnswers { get; set; }

        private static Dictionary<string,IParser> sources { get; set; }

        static Answers()
        {
            BotAnswers = new Dictionary<string, Func<ITelegramBotClient, Chat, Task>>()
            {
                {"Меню" ,  AnswerToMenu },
                {"Расписание", AnswerToShedule },
                {"Описание", AnswerToDescription },
                {"Контакты", AnswerToContacts },
                {"/start",AnswerToStart }
            };



            MainKeyBoard = new ReplyKeyboardMarkup(new[]
            {
                new[]
                {
                    new KeyboardButton("Меню"),
                    new KeyboardButton("Расписание")
                },
                new[]
                {
                    new KeyboardButton("Контакты"),
                    new KeyboardButton("Описание")
                }
            })
            {
                ResizeKeyboard = true
            };

            actionWithWordsKeyboard = new InlineKeyboardMarkup(
                new InlineKeyboardButton[][]
                {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Найти определение")
                    }
                }
                );

            definitionSources = new InlineKeyboardMarkup(
                new InlineKeyboardButton[][]
                {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Вики словарь"),
                        InlineKeyboardButton.WithCallbackData("Sinonim")
                    }
                });
        }

        public async static Task AnswerToMessage(ITelegramBotClient client,Message message)
        {
            var me = await client.GetMeAsync();
            var myName = me.FirstName;
            lastMessages.Add(message.Text);
            
            switch (BotAnswers.ContainsKey(message.Text))
            {
                case true:
                    var func = BotAnswers[message.Text];
                    await func(client, message.Chat);
                    break;
                case false:
                    await client.SendTextMessageAsync(message.Chat.Id, $"Что вы хотите сделать со словом: {message.Text}",replyMarkup:actionWithWordsKeyboard);
                    
                    break;
            }
        }

        public async static Task FindDefinition(ITelegramBotClient client,ChatId chatId,IHttpClientFactory factory)
        {
            var wiki = new SinonimParser(factory);
            var word = lastMessages.Last().ToLower();
            var definition = await wiki.Parse(word);
            await client.SendTextMessageAsync(chatId, definition);
        }

        public async static Task AnswerToStart(ITelegramBotClient botClient, Chat chat)
        {
            await botClient.SendTextMessageAsync(chat.Id, $"Привет,{chat.FirstName},\nчто вас интересует", replyMarkup: MainKeyBoard);
        }

        public async static Task AnswerToMenu(ITelegramBotClient botClient, Chat chat)
        {
            await botClient.SendTextMessageAsync(chat.Id, $"{"safsafafsa"}\n\nКакую информацию вы ещё хотели бы получить?", replyMarkup: MainKeyBoard);
        }

        public async static Task AnswerToShedule(ITelegramBotClient botClient, Chat chat)
        {
            await botClient.SendTextMessageAsync(chat.Id, $"{"sieduhgfsh"}\n\nКакую информацию вы ещё хотели бы получить?", replyMarkup: MainKeyBoard);
        }

        public async static Task AnswerToDescription(ITelegramBotClient botClient, Chat chat)
        {
            await botClient.SendTextMessageAsync(chat.Id, $"Find Definition Bot - это телеграмм бот, который поможет" +
                $" вам найти значение слова из различных источников и словарей\n\nКакую информацию вы ещё хотели бы получить?", replyMarkup: MainKeyBoard);
        }

        public async static Task AnswerToContacts(ITelegramBotClient botClient, Chat chat)
        {
            await botClient.SendTextMessageAsync(chat.Id, "Наши контакты");
            var text ="Контакты";
            await botClient.SendTextMessageAsync(chat.Id, $"{text}\n\nКакую информацию вы ещё хотели бы получить?", replyMarkup: MainKeyBoard);
        }

        
    }
}
