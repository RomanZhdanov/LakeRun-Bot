namespace LakeRun.Bot.Models;

public class EventVolunteer
{
    public int EventId { get; set; }

    public int RoleId { get; set; }

    public long UserId { get; set; }

    public Event? Event { get; set; }

    public Role? Role { get; set; }
}
