namespace LakeRun.Bot.Models;

public class Event
{
    public int Id { get; set; }

    public bool Completed { get; set; }

    public DateTime Date { get; set; }

    public Event()
    {
        var today = DateTime.Today;
        var daysTillSaturday = ((int)DayOfWeek.Saturday - (int)today.DayOfWeek + 7) % 7;
        var nextSaturday = today.AddDays(daysTillSaturday);
        Date = new DateTime(
            nextSaturday.Year,
            nextSaturday.Month,
            nextSaturday.Day,
            9, 0, 0);
    }
}
