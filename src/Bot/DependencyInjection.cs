using LakeRun.Bot.Extensions;
using LakeRun.Bot.Data;
using LakeRun.Bot.Services;
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
        services.AddHostedService<EventsTrackerService>();
        
        var botToken = config.GetValue<string>("TelegramBotToken");
        
        if (string.IsNullOrEmpty(botToken))
        {
            throw new InvalidOperationException("TelegramBotToken is not configured.");
        }
        
        services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));
        services.AddBotHandlers();

        return services;
    }
}
