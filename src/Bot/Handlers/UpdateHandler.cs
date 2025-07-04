using LakeRun.Bot.Handlers.Commands.Roles;
using LakeRun.Bot.Handlers.Commands.Start;
using LakeRun.Bot.Handlers.Commands.Volunteer;
using LakeRun.Bot.Handlers.Queries.BecomeVolunteer;
using LakeRun.Bot.Handlers.Queries.GetRoleDescription;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LakeRun.Bot.Handlers;

public class UpdateHandler : IUpdateHandler
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<string, Type> _commands;
    private readonly Dictionary<string, Type> _queries;

    public UpdateHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _commands = new Dictionary<string, Type>
        {
            { "/start", typeof(StartCommandHandler) },
            { "/roles", typeof(RolesCommandHandler) },
            { "/volunteer", typeof(VolunteerCommandHandler) },
        };

        _queries = new Dictionary<string, Type>
        {
            { "GetRoleDescription", typeof(GetRoleDescriptionQueryHandler) },
            { "BecomeVolunteer", typeof(BecomeVolunteerQueryHandler) },
        };
    }

    public async Task HandleMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        var input = message.Text?.Split(' ')[0];

        if (input != null)
        {
            if (_commands.TryGetValue(input, out var commandType))
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService(commandType) as IBotCommandHandler ?? throw new InvalidOperationException();
                    await handler.HandleAsync(botClient, message, cancellationToken);
                }
                catch (Exception e)
                {
                    await botClient.SendMessage(
                        chatId: message.Chat.Id,
                        text: $"Произошла ошибка: {e.Message}",
                        cancellationToken: cancellationToken);
                }
            }
            else
            {
                await botClient.SendMessage(
                    chatId:  message.Chat.Id,
                    text: $"Не найден обработчик для этой команды",
                    cancellationToken: cancellationToken);
            }
        }
    }

    public async Task HandleCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery query, CancellationToken cancellationToken)
    {
        if (query.Message is null || query.Data is null)
        {
            return;
        }
        
        var data = query.Data!;
        var key = data.Split(':')[0];

        if (_queries.TryGetValue(key, out var queryType))
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService(queryType) as IBotQueryHandler ?? throw new InvalidOperationException();
                await handler.HandleAsync(botClient, query, cancellationToken);
            }
            catch (Exception e)
            {
                await botClient.SendMessage(
                    chatId: query.Message.Chat.Id,
                    text: $"Произошла ошибка: {e.Message}",
                    cancellationToken: cancellationToken);
            }
        }
        else
        {
            await botClient.AnswerCallbackQuery(
                callbackQueryId: query.Id,
                text: "Не найден обработчик для этого запроса",
                cancellationToken: cancellationToken);
        }
    }

    public async Task HandleUnknownAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
