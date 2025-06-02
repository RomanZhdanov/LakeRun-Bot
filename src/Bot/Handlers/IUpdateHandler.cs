using Telegram.Bot;
using Telegram.Bot.Types;

namespace LakeRun.Bot.Handlers;

public interface IUpdateHandler
{
    Task HandleMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken);
    
    Task HandleCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery query, CancellationToken cancellationToken);
    
    Task HandleUnknownAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
}