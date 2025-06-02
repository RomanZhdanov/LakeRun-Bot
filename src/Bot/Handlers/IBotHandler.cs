using Telegram.Bot;
using Telegram.Bot.Types;

namespace LakeRun.Bot.Handlers;

public interface IBotHandler
{
   Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,  CancellationToken cancellationToken);
   
   Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken);
}