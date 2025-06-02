using System.Reflection;
using LakeRun.Bot.Commands;

namespace LakeRun.Bot.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddBotCommands(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var implementations = assembly.GetTypes()
            .Where(type => typeof(IBotCommand).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

        foreach (var implementation in implementations)
        {
            services.AddTransient(implementation);
        }
        return services;
    }
}