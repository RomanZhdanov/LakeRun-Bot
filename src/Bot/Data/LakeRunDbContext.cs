using LakeRun.Bot.Models;
using Microsoft.EntityFrameworkCore;

namespace LakeRun.Bot.Data;

public class LakeRunDbContext : DbContext
{
    public LakeRunDbContext(DbContextOptions<LakeRunDbContext> options) : base(options)
    {
    }

    public DbSet<Event> Events => Set<Event>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<EventVolunteer> EventVolunteers => Set<EventVolunteer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventVolunteer>()
            .HasKey(ev => new { ev.EventId, ev.RoleId });
    }
}
