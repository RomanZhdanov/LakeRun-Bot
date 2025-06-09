using LakeRun.Bot.Data;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace LakeRun.Bot.Handlers.Commands.Volunteer;

public class VolunteerCommandHandler : IBotCommandHandler
{
    private readonly LakeRunDbContext _dbContext;

    public VolunteerCommandHandler(LakeRunDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        var curEvent = await _dbContext.Events
            .OrderByDescending(e => e.Date)
            .SingleOrDefaultAsync(e => !e.Completed, cancellationToken);
        
        if (curEvent == null)
        {
            return;
        }
        
        var roles = await _dbContext.Roles.ToListAsync(cancellationToken);
        var volunteers = await _dbContext.EventVolunteers
            .Where(e => e.EventId == curEvent.Id)
            .ToListAsync(cancellationToken);
        var leftRoles = roles.Where(r => !volunteers.Exists(v => v.RoleId == r.Id));
        
        ReplyMarkup keyboard = new ReplyKeyboardRemove();
        var buttonsRows = new List<List<InlineKeyboardButton>>();

        foreach (var role in leftRoles)
        {
            var row = new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(role.Name, $"BecomeVolunteer:{curEvent.Id}|{role.Id.ToString()}")
            };
            buttonsRows.Add(row);
        }
        
        keyboard = new InlineKeyboardMarkup(buttonsRows);

        await botClient.SendMessage(
            chatId: message.Chat.Id,
            text: "Доступные роли:",
            replyMarkup: keyboard,
            cancellationToken: cancellationToken);

    }
}