﻿using Telegram.Bot;
using Telegram.Bot.Polling;

namespace Bot.TelegramBot;

public static class Bot
{
    private const string Token = "TOKEN";
    private static TelegramBotClient? _botClient;
    
    private static async Task Main()
    {
        _botClient = new TelegramBotClient(Token);
        var cts = new CancellationTokenSource();
        await _botClient.GetMe(cancellationToken: cts.Token);
        
        _botClient.StartReceiving(
            MessageHandler.HandleUpdate,
            MessageHandler.HandleError,
            new ReceiverOptions(),
            cts.Token);
      
        Scheduler.RunDailyTask(10, 00, async () =>
        {
            await MessageHandler.GetNewArticles(_botClient, cts.Token);
        });
        
        Console.ReadKey();
        await cts.CancelAsync();
    }
}