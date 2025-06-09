using LakeRun.Bot.Data;
using LakeRun.Bot.Models;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LakeRun.Bot.Handlers.Queries.BecomeVolunteer;

public class BecomeVolunteerQueryHandler : IBotQueryHandler
{
    private readonly LakeRunDbContext _dbContext;

    public BecomeVolunteerQueryHandler(LakeRunDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        if (callbackQuery.Data == null)
        {
            throw new ArgumentNullException(nameof(callbackQuery.Data));
        }
        
        var args = callbackQuery.Data.Split(':')[1].Split('|');
        
        var curEvent = await _dbContext.Events
            .SingleOrDefaultAsync(e => e.Id == int.Parse(args[0]), cancellationToken);

        if (curEvent == null)
        {
            throw new ArgumentException($"Event not found: {args[0]}");
        }
        
        var role = await _dbContext.Roles
            .SingleOrDefaultAsync(r => r.Id == int.Parse(args[1]), cancellationToken);

        if (role == null)
        {
            throw new ArgumentException($"Role not found: {args[1]}");
        }

        var roleTaken = _dbContext.EventVolunteers
            .Count(e => e.EventId == curEvent.Id && e.RoleId == role.Id) > 0;
        if (roleTaken)
        {
            await botClient.SendMessage(
                chatId: callbackQuery.Message.Chat.Id,
                text: $"Роль {role.Name} для этого мероприятия уже занята",
                cancellationToken: cancellationToken);
            return;
        }

        var volunteer = new EventVolunteer
        {
            EventId = curEvent.Id,
            RoleId = role.Id,
            UserId = callbackQuery.From.Id,
        };
        
        _dbContext.EventVolunteers.Add(volunteer);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        await botClient.SendMessage(
            chatId: callbackQuery.Message!.Chat.Id,
            text: $"Вы успешно зарегистрированы на роль {role.Name}",
            cancellationToken: cancellationToken);
    }
}