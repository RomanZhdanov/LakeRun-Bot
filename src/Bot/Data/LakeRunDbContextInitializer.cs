using LakeRun.Bot.Models;
using Microsoft.EntityFrameworkCore;

namespace LakeRun.Bot.Data;

public class LakeRunDbContextInitializer
{
    private readonly ILogger<LakeRunDbContextInitializer> _logger;
    private readonly LakeRunDbContext _dbContext;

    public LakeRunDbContextInitializer(ILogger<LakeRunDbContextInitializer> logger, LakeRunDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task InitializeAsync()
    {
        try
        {
            await _dbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await SeedRolesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task SeedRolesAsync()
    {
        if (!_dbContext.Roles.Any())
        {
            var roles = new List<Role>
            {
                new Role { Name = "Run-директор", Description = "Руководитель команды" },
                new Role { Name = "Секундомер", Description = "Отвечает за измерение времени и отсекания финишировавших участников" },
                new Role { Name = "Сканер", Description = "Сканирует карточки участников после забега" },
                new Role { Name = "Замыкающий", Description = "Бежит позади всех участников" },
                new Role { Name = "Выдача карточек", Description = "Выдает участникам карточки на финише с порядковым номером" },
                new Role { Name = "Фотограф", Description = "Фотографирует участников" },
                new Role { Name = "Разминка", Description = "Проводит разминку перед забегом" },
            };

            await _dbContext.Roles.AddRangeAsync(roles);
            await _dbContext.SaveChangesAsync();
        }
    }
}
