using LakeRun.Bot.Data;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace LakeRun.Bot.Handlers.Commands.Roles;

public class RolesCommandHandler : IBotCommandHandler
{
    private readonly LakeRunDbContext _dbContext;

    public RolesCommandHandler(LakeRunDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        var roles = await _dbContext.Roles.ToListAsync(cancellationToken);

        ReplyMarkup keyboard = new ReplyKeyboardRemove();
        var buttonsRows = new List<List<InlineKeyboardButton>>();

        foreach (var role in roles)
        {
            var row = new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(role.Name, $"GetRoleDescription:roleId={role.Id.ToString()}")
            };
            buttonsRows.Add(row);
        }
        
        keyboard = new InlineKeyboardMarkup(buttonsRows);
        
        await botClient.SendMessage(
            chatId: message.Chat.Id,
            text: "Роли волонтеров:",
            replyMarkup: keyboard,
            cancellationToken: cancellationToken);
    }
}