using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace LakeRun.Bot.Handlers;

public class BotHandler : IBotHandler
{
    private readonly IUpdateHandler _updateHandler;

    public BotHandler(IUpdateHandler updateHandler)
    {
        _updateHandler = updateHandler;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var handler = update.Type switch
        {
            UpdateType.Message => _updateHandler.HandleMessageAsync(botClient, update.Message!, cancellationToken),
            UpdateType.CallbackQuery => _updateHandler.HandleCallbackQueryAsync(botClient, update.CallbackQuery!, cancellationToken),
            _ => _updateHandler.HandleUnknownAsync(botClient, update, cancellationToken)
        };

        try
        {
            await handler;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }
}