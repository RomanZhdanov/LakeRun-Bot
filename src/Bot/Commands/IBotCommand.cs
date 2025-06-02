using Telegram.Bot;
using Telegram.Bot.Types;

namespace LakeRun.Bot.Commands;

public interface IBotCommand
{
   Task ExecuteAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken);
}