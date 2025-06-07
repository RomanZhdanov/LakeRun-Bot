using Telegram.Bot;
using Telegram.Bot.Types;

namespace LakeRun.Bot.Handlers.Queries;

public interface IBotQueryHandler
{
    Task HandleAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken);
}