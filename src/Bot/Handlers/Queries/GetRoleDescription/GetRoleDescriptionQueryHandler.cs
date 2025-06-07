using LakeRun.Bot.Data;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LakeRun.Bot.Handlers.Queries.GetRoleDescription;

public class GetRoleDescriptionQueryHandler :  IBotQueryHandler
{
    private readonly LakeRunDbContext _dbContext;

    public GetRoleDescriptionQueryHandler(LakeRunDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        var roleId = GetRoleId(callbackQuery.Data);
        var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == roleId, cancellationToken);

        if (role == null)
        {
            await botClient.AnswerCallbackQuery(callbackQuery.Id, "Роль не найдена, попробуйте обновить список", cancellationToken: cancellationToken);
            return;
        }

        var msgText = $"Описание роли {role.Name}: \n\n{role.Description}";
        
        await botClient.SendMessage(
            chatId:  callbackQuery.Message!.Chat.Id,
            text: msgText,
            cancellationToken: cancellationToken);
    }

    private int GetRoleId(string? data)
    {
        if (data == null) return 0;
        
        var parts = data.Split("=");
        return int.Parse(parts[1]);
    }
}