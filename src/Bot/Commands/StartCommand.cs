using Telegram.Bot;
using Telegram.Bot.Types;

namespace LakeRun.Bot.Commands;

public class StartCommand : IBotCommand
{
    public async Task ExecuteAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await botClient.SendMessage(
            chatId: message.Chat.Id,
            text: "Hello! Welcome to the bot. Type /help to see available commands.",
            cancellationToken: cancellationToken); 
    }
}