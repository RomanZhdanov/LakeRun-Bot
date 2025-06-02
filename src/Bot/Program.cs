using LakeRun.Bot.Commands;
using LakeRun.Bot.Extensions;
using LakeRun.Bot.Handlers;
using Telegram.Bot;

namespace LakeRun.Bot;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHostedService<Worker>();
        var botToken = builder.Configuration.GetValue<string>("TelegramBotToken");
        if (string.IsNullOrEmpty(botToken))
        {
            throw new InvalidOperationException("TelegramBotToken is not configured.");
        } 
        builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));
        builder.Services.AddSingleton<IBotHandler, BotHandler>();
        builder.Services.AddSingleton<IUpdateHandler, UpdateHandler>();
        builder.Services.AddSingleton<CommandsManager>();
        builder.Services.AddBotCommands();
        var host = builder.Build();
        await host.RunAsync();
    }
}