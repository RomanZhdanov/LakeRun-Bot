using Telegram.Bot;
using Telegram.Bot.Types;

namespace LakeRun.Bot.Handlers;

public interface IBotQueryHandler
{
    Task HandleAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken);
}