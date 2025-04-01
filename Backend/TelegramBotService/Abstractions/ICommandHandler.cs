using Telegram.Bot;
using Telegram.Bot.Types;

/// <summary>
/// Интерфейс для обработчиков команд.
/// </summary>
public interface ICommandHandler
{
    /// <summary>
    /// Название команды.
    /// </summary>
    string CommandName { get; }

    /// <summary>
    /// Обрабатывает команду.
    /// </summary>
    /// <param name="message">Сообщение от пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    Task HandleCommandAsync(ITelegramBotClient client, Message message, CancellationToken cancellationToken);

    /// <summary>
    /// Обрабатывает ответ пользователя.
    /// </summary>
    /// <param name="message">Сообщение от пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    Task HandleResponseAsync(ITelegramBotClient client, Message message, CancellationToken cancellationToken);
}