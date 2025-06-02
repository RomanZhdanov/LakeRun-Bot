using LakeRun.Bot.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LakeRun.Bot.Handlers;

public class UpdateHandler : IUpdateHandler
{
    private readonly CommandsManager _commandsManager;

    public UpdateHandler(CommandsManager commandsManager)
    {
        _commandsManager = commandsManager;
    }

    public async Task HandleMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        var input = message.Text?.Split(' ')[0];
        
        if (input != null)
        {
            var command = _commandsManager.GetCommand(input);
            await command.ExecuteAsync(botClient, message, cancellationToken);
        }
    }

    public async Task HandleCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task HandleUnknownAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}