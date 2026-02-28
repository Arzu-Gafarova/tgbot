public class GroupSchedule
{
    public string Group { get; set; } = string.Empty; // e.g. "9A"
    public List<DaySchedule> Days { get; set; } = new();
}