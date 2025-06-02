using LakeRun.Bot.Handlers;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace LakeRun.Bot;

public class Worker : BackgroundService
{
    private readonly IBotHandler _handler;
    private readonly ITelegramBotClient _bot;
    private readonly ILogger<Worker> _logger;

    public Worker(IBotHandler handler, ITelegramBotClient bot, ILogger<Worker> logger)
    {
        _handler = handler;
        _bot = bot;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _bot.StartReceiving(
            updateHandler: _handler.HandleUpdateAsync,
            errorHandler: _handler.HandleErrorAsync,
            receiverOptions: new ReceiverOptions()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            },
            cancellationToken: stoppingToken);
        
        var me = await _bot.GetMe(cancellationToken: stoppingToken);
        _logger.LogInformation($"Start listening for @{me.Username}");
    }
}
