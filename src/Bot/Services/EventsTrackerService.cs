using LakeRun.Bot.Data;
using LakeRun.Bot.Models;
using Microsoft.EntityFrameworkCore;

namespace LakeRun.Bot.Services;

public class EventsTrackerService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public EventsTrackerService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using  var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<LakeRunDbContext>();
        var curEvent = await dbContext.Events
            .OrderByDescending(e => e.Date)
            .FirstOrDefaultAsync(e => !e.Completed, stoppingToken);

        if (curEvent == null)
        {
            curEvent = new Event();
            dbContext.Events.Add(curEvent);
            await dbContext.SaveChangesAsync(stoppingToken);
        }
    }
}