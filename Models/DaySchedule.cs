public class DaySchedule
{
    public string Day { get; set; } = string.Empty; // e.g. Monday
    public List<Lesson> Lessons { get; set; } = new();
}