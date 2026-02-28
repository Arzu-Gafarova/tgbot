using ScheduleBot.Commands;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

class Program
{
    private const string ScheduleJson = "schedule.json";
    public static async Task Main()
    {
        Console.WriteLine("Запуск бота...");

        var token = "8045714215:AAG_jBEGD1vkEdlU1UpkSaX4tsg_MKYPosg";
        var botClient = new TelegramBotClient(token);

        var scheduleRepository = new JsonScheduleRepository(ScheduleJson);

        // Создаём диспетчер и регистрируем команды
        var dispatcher = new CommandDispatcher();
        dispatcher.Register("/start", new StartCommand());
        dispatcher.Register("/help", new HelpCommand());
        dispatcher.Register("/week", new WeekCommand(scheduleRepository));
        dispatcher.Register("/tomorrow", new TomorrowCommand(scheduleRepository));

        using var cts = new CancellationTokenSource();
        var receiverOptions = new ReceiverOptions { AllowedUpdates = Array.Empty<UpdateType>() };

        // Обработка обновлений
        botClient.StartReceiving(
        async (client, update, ct) => await dispatcher.DispatchAsync(update, client, ct),
        HandleErrorAsync,
        receiverOptions, 
        cts.Token);

        var me = await botClient.GetMeAsync();
        Console.WriteLine($"Бот запущен: @{me.Username}");
        Console.ReadLine();
        cts.Cancel();
    }

    static Task HandleErrorAsync(ITelegramBotClient bot, Exception ex, CancellationToken ct)
    {
        Console.WriteLine($"Ошибка: {ex.Message}");
        return Task.CompletedTask;
    }
}