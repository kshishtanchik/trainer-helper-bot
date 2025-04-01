using DatabaseService;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

/// <summary>
/// Обработчик команда /start.
/// </summary>
public class StartCommandHandler : ICommandHandler
{
    private readonly ILogger<StartCommandHandler> logger;
    private readonly IDataService dataService;

    /// <summary>
    /// Базовый конструктор.
    /// </summary>
    public StartCommandHandler(ILoggerFactory loggerFactory, IDataService dataService)
    {
        logger = loggerFactory.CreateLogger<StartCommandHandler>();
        this.dataService = dataService;
    }

    /// <summary>
    /// Имя команды обработчика.
    /// </summary>
    public string CommandName => "/start";

    /// <summary>
    /// Метод обработчика команды.
    /// </summary>
    /// <param name="client">Клиент телеграмм.</param>
    /// <param name="message">Сообщение.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Задача.</returns>
    public async Task HandleCommandAsync(ITelegramBotClient client, Message message, CancellationToken cancellationToken)
    {
        /* https://t.me/event_oper_bot?start=event_1232 если получили event_{id} значит запускаем процесс записи
            1. найти мероприятие.
            2. сформировать сообщение с кнопками:
                - записаться - проверить свободные места, оставить данные пользователя.
                - предупредить об отсутствии - оставить данные с пометкой не придет
            3. сообщить о новом участнике

            https://t.me/event_oper_bot?start=facilitator если получили facilitator - помечаем чат организатором

            /start - просто команда старт - показываем доступные пользователю команды.
        */
        var userMsg = "Добро пожаловать! Мероприятие Выберите действие:\n/schedule - Расписание\n/book - Запись\n/profile - Профиль\n/pay - Оплата";
        var chatId = message.Chat.Id;
        var msgLink = message.Text.Split(" ")[1];
        if (msgLink.Contains("event"))
        {
            var aktivityId = Convert.ToInt32(msgLink.Split("_")[1]);
            try
            {
                var activity = await dataService.Reservation(chatId, aktivityId);
                userMsg = $"Вы успешно зарегистрировались на {activity.Name}";
            }
            catch (NotFoundActivityException notFoundActivityException)
            {
                logger.LogError(notFoundActivityException, notFoundActivityException.Message);
                userMsg = "К сожалению запрошенное мероприятие не найдено. Выберите доступное командой /activities";
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }

        await client.SendMessage(
        chatId: message.Chat.Id,
        text: userMsg,
        parseMode: ParseMode.Markdown,
        cancellationToken: cancellationToken);
    }

    public Task HandleResponseAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}