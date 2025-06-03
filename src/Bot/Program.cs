namespace LakeRun.Bot;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddBotServices(builder.Configuration);

        var host = builder.Build();
        await host.RunAsync();
    }
}
