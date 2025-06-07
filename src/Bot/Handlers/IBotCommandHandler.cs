using Telegram.Bot;
using Telegram.Bot.Types;

namespace LakeRun.Bot.Handlers.Commands;

public interface IBotCommandHandler
{
   Task HandleAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken);
}