using Microsoft.Extensions.Configuration;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using SearchInterpritationBot.Test;
using Microsoft.Extensions.DependencyInjection;

namespace SearchInterpritationBot
{
    class Program
    {
        static IHttpClientFactory _httpClientFactory;
        static async Task Main(string[] args)
        {

            var serviceProvider = new ServiceCollection()
            .AddHttpClient()
            .BuildServiceProvider();

            // Получение IHttpClientFactory
            _httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();


            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            Config.Config.SetProperties(config);
            var _botClient = new TelegramBotClient(Config.Config.BotSettings.BotTocken);
            var _receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { },
                ThrowPendingUpdates = true,
            };
            using var cts = new CancellationTokenSource();
            _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token);
            var me = await _botClient.GetMeAsync();
            Console.WriteLine($"{me.FirstName} started");
            Console.ReadLine();
        }

        private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    await Answers.AnswerToMessage(botClient, update.Message);
                    break;
                case UpdateType.CallbackQuery:
                    var callbackQuery = update.CallbackQuery;
                    await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                    await Answers.FindDefinition(botClient,callbackQuery.Message.Chat.Id,_httpClientFactory);
                    break;
            }
        }

        private static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            var ErrorMessage = error switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => error.ToString()
            };
            Console.WriteLine("Было вызвано исключение:");
            Console.WriteLine(ErrorMessage);
            Console.WriteLine();
            return Task.CompletedTask;
        }
    }
}