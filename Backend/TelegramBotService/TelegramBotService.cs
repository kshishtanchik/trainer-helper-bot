using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

/// <summary>
/// Сервис для обработки обновлений Telegram-бота.
/// </summary>
public class TelegramBotService : IHostedService
{
    private readonly ILogger<TelegramBotService> _logger;
    private readonly ITelegramBotClient _botClient;
    private readonly BotStateManager _stateManager;
    private readonly Dictionary<string, ICommandHandler> _commandHandlers;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="TelegramBotService"/>.
    /// </summary>
    /// <param name="botClient">Клиент Telegram Bot.</param>
    /// <param name="stateManager">Менеджер состояния пользователей.</param>
    /// <param name="commandHandlers">Список обработчиков команд.</param>
    public TelegramBotService(ILoggerFactory loggerFactory, ITelegramBotClient botClient, BotStateManager stateManager, IEnumerable<ICommandHandler> commandHandlers)
    {
        _logger = loggerFactory.CreateLogger<TelegramBotService>();
        _botClient = botClient;
        _stateManager = stateManager;
        _commandHandlers = commandHandlers.ToDictionary(h => h.CommandName);
    }

    /// <summary>
    /// Запускает сервис.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Начинаем получать обновления
        _botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, null, cancellationToken);

        Console.WriteLine("Бот запущен и ожидает обновлений...");
    }

    private async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.CallbackQuery)
        {
            await HandleCallbackQuery(update, cancellationToken);
        }

        if (update.Type != UpdateType.Message || update.Message!.Type != MessageType.Text)
        {
            return;
        }

        var message = update.Message;
        var userId = message.From.Id;
        var userState = _stateManager.GetUserState(userId);

        if (_commandHandlers.TryGetValue(message.Text.Split(' ')[0], out var handler))
        {
            await handler.HandleCommandAsync(client, message, cancellationToken);
        }
        else if (!string.IsNullOrEmpty(userState.CurrentCommand))
        {
            // Если есть активная команда, передаем сообщение в обработчик
            if (_commandHandlers.TryGetValue(userState.CurrentCommand, out var activeHandler))
            {
                await activeHandler.HandleResponseAsync(client, message, cancellationToken);
            }
        }
        else
        {
            await _botClient.SendMessage(
                chatId: message.Chat.Id,
                text: "Неизвестная команда. Используйте /start для просмотра доступных команд.",
                cancellationToken: cancellationToken);
        }
    }

    private async Task HandleCallbackQuery(Update update, CancellationToken cancellationToken)
    {
        // Обрабатываем нажатие кнопки
        var callbackQuery = update.CallbackQuery;
        var chatId = callbackQuery.Message.Chat.Id;
        var data = callbackQuery.Data;
        var userState = _stateManager.GetUserState(chatId);

        var command = userState.CurrentCommand;
        var handler =  _commandHandlers[command];

        if (handler != null)
        {
            await handler.HandleResponseAsync(_botClient, new Message
            {
                From = callbackQuery.From,
                Chat = callbackQuery.Message.Chat,
                Text = data
            }, cancellationToken);
        }
    }

    private async Task HandleErrorAsync(ITelegramBotClient client, Exception exception, HandleErrorSource source, CancellationToken token)
    {
        _logger.LogError(exception, "Ошибка в работе бота");
    }

    /// <summary>
    /// Останавливает сервис.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Бот остановлен.");
    }
}