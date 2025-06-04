using LakeRun.Bot.Data;

namespace LakeRun.Bot.Extensions;

public static class IHostExtensions
{
    public static async Task InitializeDatabaseAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<LakeRunDbContextInitializer>();

        await initializer.InitializeAsync();

        await initializer.SeedAsync();
    }
}
