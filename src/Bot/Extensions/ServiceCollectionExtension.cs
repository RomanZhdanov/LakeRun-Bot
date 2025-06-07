using System.Reflection;
using LakeRun.Bot.Handlers;

namespace LakeRun.Bot.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddBotHandlers(this IServiceCollection services)
    {
        services.AddSingleton<IBotHandler, BotHandler>();
        services.AddSingleton<IUpdateHandler, UpdateHandler>();
        
        var assembly = Assembly.GetExecutingAssembly();
        var implementations = assembly.GetTypes()
            .Where(type => 
                (typeof(IBotCommandHandler).IsAssignableFrom(type) || typeof(IBotQueryHandler).IsAssignableFrom(type)) && 
                type is { IsInterface: false, IsAbstract: false });

        foreach (var implementation in implementations)
        {
            services.AddScoped(implementation);
        }
        
        return services;
    }
}