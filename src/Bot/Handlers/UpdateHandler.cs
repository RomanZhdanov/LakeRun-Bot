using LakeRun.Bot.Commands;
using LakeRun.Bot.Commands.Roles;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LakeRun.Bot.Handlers;

public class UpdateHandler : IUpdateHandler
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<string, Type> _commands;

    public UpdateHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _commands = new Dictionary<string, Type>
        {
            { "/start", typeof(StartCommand) },
            { "/roles", typeof(RolesCommand) }
        };
    }

    public async Task HandleMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        var input = message.Text?.Split(' ')[0];

        if (input != null)
        {
            if (_commands.TryGetValue(input, out var commandType))
            {
                using var scope = _serviceProvider.CreateScope();
                var command = scope.ServiceProvider.GetRequiredService(commandType) as IBotCommand ?? throw new InvalidOperationException();
                await command.ExecuteAsync(botClient, message, cancellationToken);
            }
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
