using Telegram.Bot;
using Telegram.Bot.Types;

namespace ScheduleBot.Commands;

public class TomorrowCommand : ICommand
{
    private readonly IScheduleRepository _scheduleRepository;

    public TomorrowCommand(IScheduleRepository scheduleRepository)
    {
        _scheduleRepository = scheduleRepository;
    }


    public async Task ExecuteAsync(Update update, ITelegramBotClient botClient, CancellationToken ct)
    {
        var chatId = update.Message!.Chat.Id;
        var text = update.Message!.Text ?? string.Empty;

        // ожидаем формат: /tomorrow 9A
        var parts = text.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
        {
            await botClient.SendTextMessageAsync(
            chatId,
            "Использование: /tomorrow [группа]\n Например: /tomorrow 9A",
            cancellationToken: ct);
            return;
        }

        var groupName = parts[1].Trim();

        var schedule = _scheduleRepository.Load();
        var group = schedule.Groups
        .FirstOrDefault(g => string.Equals(g.Group, groupName, StringComparison.OrdinalIgnoreCase));

        if (group == null)
        {
            await botClient.SendTextMessageAsync(
            chatId,
            $"Для группы {groupName} завтра  нет занятий .",
            cancellationToken: ct);
            return;
        }
        var tomorrow = DateTime.Now.AddDays(1).DayOfWeek.ToString();

        var daySchedule = group.Days
            .FirstOrDefault(d => string.Equals(d.Day,tomorrow,StringComparison.OrdinalIgnoreCase));
        
        if (daySchedule == null)
        {
            await botClient.SendTextMessageAsync(
                chatId,
                $"Завтра ({tomorrow}) занятий не будет. ",
                cancellationToken: ct);
            return;
        }            

        var lines = new List <string>
        {
            $"Расписание на завтра ({tomorrow}) для группы {groupName}:"
        };
        if (daySchedule.Lessons == null || daySchedule.Lessons.Count == 0 )
        {
            lines.Add("Завтра занятий не будет.");
        }
        else
        {
            lines.AddRange(
                daySchedule.Lessons.Select(
                    (l, i) => $"{i + 1}.{l.Time} - {l.Subject}" +
                        (string.IsNullOrEmpty(l.Teacher) ? "" : $"({l.Teacher})")
                )

             );
        }
        await botClient.SendTextMessageAsync(
            chatId,
            string.Join("\n", lines),
            cancellationToken: ct );

    }
}

