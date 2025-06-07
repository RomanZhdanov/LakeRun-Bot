using System.Text;
using LakeRun.Bot.Data;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LakeRun.Bot.Commands.Roles;

public class RolesCommand : IBotCommand
{
    private readonly LakeRunDbContext _dbContext;

    public RolesCommand(LakeRunDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ExecuteAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        var roles = await _dbContext.Roles.ToListAsync(cancellationToken);
        var text = new StringBuilder();

        foreach (var role in roles)
        {
            text.AppendLine($"{role.Name}");
        }
        
        await botClient.SendMessage(
            chatId: message.Chat.Id,
            text: text.ToString(),
            cancellationToken: cancellationToken);
    }
}