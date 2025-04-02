using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotService.Handlers;

/// <summary>
/// Обработчик отображения доступных событий.
/// </summary>
public class ActivitiesHandler : ICommandHandler
{
    private readonly IDataService _databaseService;
    
    /// <summary>
    /// Создать обработчик.
    /// </summary>
    /// <param name="databaseService">Сервис доступа к данным.</param>
    public ActivitiesHandler(IDataService databaseService)
    {
        _databaseService = databaseService;
    }

    /// <summary>
    /// Имя комманды.
    /// </summary>
    public string CommandName => "/activities";

    public async Task HandleCommandAsync(ITelegramBotClient client, Message message, CancellationToken cancellationToken)
    {
        // найти доступные активности у них и показатьв виде кнопок
        var avaibleActivities =await _databaseService.FindAvaibleActivities(message.Chat.Id);
        
        // сформировать кнопки для каждой активности
        var buttons =avaibleActivities
            .Select(f => new[] { 
                InlineKeyboardButton.WithCallbackData(f., f.Name) 
                })
            .ToArray();

        var keyboard = new InlineKeyboardMarkup(buttons);

        // todo: сохранить state
        await client.SendMessage(
            chatId: message.Chat.Id,
            text: "Выберите поле для заполнения:",
            replyMarkup: keyboard,
            cancellationToken: cancellationToken);
        throw new NotImplementedException();
    }

    public Task HandleResponseAsync(ITelegramBotClient client, Message message, CancellationToken cancellationToken)
    {
        /*
        todo: обработка запросов отображения активностей
        1. показать активность
        2. записаться на нее
        3. выйти из обработчика
        */
        throw new NotImplementedException();
    }
}
