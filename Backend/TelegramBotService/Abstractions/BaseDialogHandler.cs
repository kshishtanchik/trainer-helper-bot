using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

/// <summary>
/// Универсальный обработчик диалога для заполнения модели данных.
/// </summary>
/// <typeparam name="T">Тип модели данных.</typeparam>
public abstract class BaseDialogHandler<T> : ICommandHandler where T : class, new()
{
    private readonly BotStateManager _stateManager;
    private readonly Dictionary<long, DialogState<T>> _userStates = new();

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="BaseDialogHandler{T}"/>.
    /// </summary>
    /// <param name="botClient">Клиент Telegram Bot.</param>
    protected BaseDialogHandler(BotStateManager stateManager)
    {
        _stateManager = stateManager;
    }

    /// <summary>
    /// Название команды.
    /// </summary>
    public abstract string CommandName { get; }

    /// <summary>
    /// Обрабатывает команду.
    /// </summary>
    /// <param name="client">Экземпляр клиента.</param>
    /// <param name="message">Сообщение от пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    public async Task HandleCommandAsync(ITelegramBotClient client, Message message, CancellationToken cancellationToken)
    {
        var userId = message.From.Id;
        var chatId = message.Chat.Id;

        var userState = new DialogState<T>
        {
            Model = new T(),
            FieldsToFill = GetFillableFields(),
            userProfile =new UserProfile(userId){
                Name=$"{message.Chat.LastName} {message.Chat.FirstName} {message.Chat.Title}"
            }
        };

        _userStates[userId] = userState;
        _stateManager.GetUserState(userId).CurrentCommand = CommandName;

        // Отправляем сообщение с кнопками
        await BaseDialogHandler<T>.SendFieldsMenuAsync(chatId, userState, client, cancellationToken);
    }

    /// <summary>
    /// Обрабатывает ответ пользователя.
    /// </summary>
    /// <param name="client">Клиент телеграм.</param>
    /// <param name="message">Сообщение от пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    public async Task HandleResponseAsync(ITelegramBotClient client, Message message, CancellationToken cancellationToken)
    {
        var userId = message.From.Id;
        var chatId = message.Chat.Id;

        if (!_userStates.TryGetValue(userId, out var userState))
        {
            return;
        }

        if (userState.CurrentField == null)
        {
            // Обрабатываем нажатие кнопки
            var fieldName = message.Text;
            var field = userState.FieldsToFill.FirstOrDefault(f => f.Name == fieldName);

            if (field != null)
            {
                userState.CurrentField = field;
                await client.DeleteMessage(userState.LastMessage.Chat.Id, userState.LastMessage.Id, cancellationToken);
                userState.LastMessage = await client.SendMessage(
                    chatId: chatId,
                    text: $"{field.GetCustomAttribute<FillableFieldAttribute>()?.Prompt}:",
                    cancellationToken: cancellationToken);
            }
        }
        else
        {
            // Обрабатываем ввод значения
            SetPropertyValue(userState.Model, userState.CurrentField, message.Text);
            userState.FieldsToFill.Remove(userState.CurrentField);
            userState.CurrentField = null;

            if (userState.FieldsToFill.Any())
            {
                await BaseDialogHandler<T>.SendFieldsMenuAsync(chatId, userState, client, cancellationToken);
            }
            else
            {
                // сохранить модель в бд
                var modelKeyboardMarkup=SaveUserStateModel(userState);

                // Диалог завершен
                await client.SendMessage(
                    chatId: chatId,
                    text: "Спасибо! Все данные заполнены.\n" + FormatModel(userState.Model),
                    replyMarkup: modelKeyboardMarkup,
                    cancellationToken: cancellationToken);

                _userStates.Remove(userId);
            }
        }
    }

    protected abstract InlineKeyboardMarkup SaveUserStateModel(DialogState<T> userState);

    /// <summary>
    /// Отправляет меню с кнопками для выбора поля.
    /// </summary>
    /// <param name="chatId">Идентификатор чата.</param>
    /// <param name="userState">Состояние диалога.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    private static async Task SendFieldsMenuAsync(long chatId, DialogState<T> userState, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        var buttons = userState.FieldsToFill
            .Select(f => new[] { InlineKeyboardButton.WithCallbackData(f.GetCustomAttribute<FillableFieldAttribute>()?.Prompt, f.Name) })
            .ToArray();

        var keyboard = new InlineKeyboardMarkup(buttons);
        if (userState.LastMessage != null)
        {
            await botClient.DeleteMessage(userState.LastMessage.Chat.Id, userState.LastMessage.Id, cancellationToken);
        }

        userState.LastMessage = await botClient.SendMessage(
            chatId: chatId,
            text: "Выберите поле для заполнения:",
            replyMarkup: keyboard,
            cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Получает список полей, которые нужно заполнить.
    /// </summary>
    /// <returns>Список свойств, помеченных атрибутом <see cref="FillableFieldAttribute"/>.</returns>
    private List<PropertyInfo> GetFillableFields()
    {
        return typeof(T)
            .GetProperties()
            .Where(p => p.GetCustomAttribute<FillableFieldAttribute>() != null)
            .ToList();
    }

    /// <summary>
    /// Форматирует модель для отображения.
    /// </summary>
    /// <param name="model">Модель данных.</param>
    /// <returns>Форматированная строка.</returns>
    private string FormatModel(T model)
    {
        return string.Join("\n", typeof(T)
            .GetProperties()
            .Where(p => p.GetCustomAttribute<FillableFieldAttribute>() != null)
            .Select(p => $"{p.GetCustomAttribute<FillableFieldAttribute>()?.Prompt}: {p.GetValue(model)}"));
    }

    /// <summary>
    /// Устанавливает значение свойства.
    /// </summary>
    /// <param name="model">Модель данных.</param>
    /// <param name="property">Свойство, которое нужно заполнить.</param>
    /// <param name="value">Значение, введенное пользователем.</param>
    private void SetPropertyValue(T model, PropertyInfo property, string value)
    {
        if (property.PropertyType == typeof(string))
        {
            property.SetValue(model, value);
        }
        else if (property.PropertyType == typeof(long) && long.TryParse(value, out var longValue))
        {
            property.SetValue(model, longValue);
        }
        else if (property.PropertyType == typeof(int) && int.TryParse(value, out var intValue))
        {
            property.SetValue(model, intValue);
        }
        else if (property.PropertyType == typeof(double) && double.TryParse(value, out var doubleValue))
        {
            property.SetValue(model, doubleValue);
        }
        else if (property.PropertyType == typeof(DateTime) && ParseDateTime(value, out var dateValue))
        {
            property.SetValue(model, dateValue);
        }

        // Добавьте другие типы по необходимости
    }

    private bool ParseDateTime(string msg, out DateTime dateTime)
    {
        var datePattern = new Regex(@"^(?<day>\d{1,2})[\s\/\.]+(?<month>\d{1,2})[\s\/\\\.]+(?<year>\d{2,4})\s+(?<hours>\d{1,2})[\.:\s](?<minutes>\d{2})$");
        var datePaths = datePattern.Match(msg);
        var correctDate = $"{datePaths.Groups["day"]}.{datePaths.Groups["month"]}.{datePaths.Groups["year"]} {datePaths.Groups["hours"]}:{datePaths.Groups["minutes"]}";
        dateTime= new DateTime(Convert.ToInt32(datePaths.Groups["year"].ToString()),
        Convert.ToInt32(datePaths.Groups["month"].ToString()),
        Convert.ToInt32(datePaths.Groups["day"].ToString()),
        Convert.ToInt32(datePaths.Groups["hours"].ToString()),
        Convert.ToInt32(datePaths.Groups["minutes"].ToString()),
        0
        );
        return datePaths.Success;
        //return DateTime.TryParseExact(correctDate, "dd.MM.yyyy HH:mm", new CultureInfo("ru-RU"), DateTimeStyles.AllowWhiteSpaces, out dateTime);
    }
}