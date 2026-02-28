using System.Text.Json;

public class JsonScheduleRepository : IScheduleRepository
{
    private readonly string _path;
    private readonly JsonSerializerOptions _opts = new() { PropertyNameCaseInsensitive = true };

    public JsonScheduleRepository(string path)
    {
        _path = path;
        if (!File.Exists(_path))
        {
            var sample = new ScheduleFile
            {
                Groups = new List<GroupSchedule>
                {
                    new GroupSchedule
                {
                    Group = "9A",
                    Days = new List<DaySchedule>
                    {
                    new DaySchedule
                    {
                        Day = "Monday",

                        Lessons = new List<Lesson>
                        {

                            new Lesson ("09:00", "Math","Ivanov"),
                            new Lesson ("10:00", "English Language", "Gafarova"),
                            new Lesson ("11:00", "Chemistry", "Pankratova"),
                            new Lesson ("12:00", "Biology", "Plaxina"),
                            new Lesson ("13:00", "Social studies", "Uvarova"),

                        }
                    },
                    new DaySchedule
                    {
                        Day = "Tuesday", Lessons = new List<Lesson>
                        {
                            new Lesson ("09:00", "Physics","Petrova"),
                            new Lesson ("10:00", "Geography","Gadjiev"),
                            new Lesson ("11:00", "IT lesson", "Patokina"),
                            new Lesson ("12:00", "Biology", "Ponomarev"),
                            new Lesson ("13:00", "Social studies", "Shilova"),
                        }
                    },
                    new DaySchedule
                    {
                        Day = "Wednesday", Lessons = new List<Lesson>()
                        {
                            new Lesson ("09:00", "Russian language","Dianova"),
                            new Lesson ("10:00", "Biology","Taksheeva"),
                            new Lesson ("11:00", "IT lesson", "Pankova"),
                            new Lesson ("12:00", "Art lesson", "Drozdov"),
                            new Lesson ("13:00", "Sports", "Permilovsky"),
                        }
                    },
                    new DaySchedule
                    {
                        Day = "Thursday", Lessons = new List<Lesson>()
                        {
                            new Lesson ("09:00", "Music"," Baykalova"),
                            new Lesson ("10:00", "Astronomy","Tokman"),
                            new Lesson ("11:00", "Probability and statistics", "Remizov"),
                            new Lesson ("12:00", "Financial literacy", "Igumnov"),
                            new Lesson ("13:00", "Literature", "Lykinskiy"),
                        }
                    },
                    new DaySchedule
                    {
                        Day = "Friday", Lessons = new List<Lesson>()
                        {
                            new Lesson ("09:00", "History","Vasiluk"),
                            new Lesson ("10:00", "Sports","Smirnov"),
                            new Lesson ("11:00", "Physics", "Rezviy"),
                            new Lesson ("12:00", "Math", "Loxova"),
                            new Lesson ("13:00", "Astronomy", "Kikalo"),
                        }
                    }
                }
                }
                }
            };
            File.WriteAllText(_path, JsonSerializer.Serialize(sample, new JsonSerializerOptions { WriteIndented = true }));
        }
    }

    public ScheduleFile Load()
    {
        using var s = File.OpenRead(_path);
        return JsonSerializer.Deserialize<ScheduleFile>(s, _opts) ?? new ScheduleFile();
    }
}