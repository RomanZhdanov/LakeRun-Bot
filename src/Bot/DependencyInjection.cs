using LakeRun.Bot.Commands;
using LakeRun.Bot.Handlers;
using LakeRun.Bot.Extensions;
using LakeRun.Bot.Data;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace LakeRun.Bot;

public static class DependencyInjection
{
    public static IServiceCollection AddBotServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<LakeRunDbContext>(options =>
            options.UseSqlite(config.GetConnectionString("DefaultConnection")));
        services.AddScoped<LakeRunDbContextInitializer>();
        services.AddHostedService<Worker>();
        var botToken = config.GetValue<string>("TelegramBotToken");
        if (string.IsNullOrEmpty(botToken))
        {
            throw new InvalidOperationException("TelegramBotToken is not configured.");
        }
        services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));
        services.AddSingleton<IBotHandler, BotHandler>();
        services.AddSingleton<IUpdateHandler, UpdateHandler>();
        services.AddSingleton<CommandsManager>();
        services.AddBotCommands();

        return services;
    }
}
