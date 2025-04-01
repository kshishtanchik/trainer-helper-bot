using System;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotService.Handlers;

/// <summary>
/// Обработчик отображения доступных событий.
/// </summary>
public class ActivitiesHandler : ICommandHandler
{
    private readonly IDataService _databaseService;

    /// <summary>
    /// Имя комманды.
    /// </summary>
    public string CommandName => "/activities";

    /// <summary>
    /// Создать обработчик.
    /// </summary>
    /// <param name="databaseService">Сервис доступа к данным.</param>
    public ActivitiesHandler(IDataService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task HandleCommandAsync(ITelegramBotClient client, Message message, CancellationToken cancellationToken)
    {
        // найти доступные активности у них и показатьв виде кнопок
        var avaibleActivities =await _databaseService.FindAvaibleActivities(message.Chat.Id);
        throw new NotImplementedException();
    }

    public Task HandleResponseAsync(ITelegramBotClient client, Message message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
