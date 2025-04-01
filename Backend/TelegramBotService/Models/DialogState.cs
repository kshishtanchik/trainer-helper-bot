using System.Reflection;
using Telegram.Bot.Types;

/// <summary>
/// Состояние диалога для пользователя.
/// </summary>
public class DialogState<T> where T : class, new()
{
    /// <summary>
    /// Модель данных, которая заполняется в диалоге.
    /// </summary>
    public T Model { get; set; } =new T();

    /// <summary>
    /// Список полей, которые нужно заполнить.
    /// </summary>
    public List<PropertyInfo> FieldsToFill { get; set; } = new List<PropertyInfo>();

    /// <summary>
    /// Текущее поле, которое заполняется.
    /// </summary>
    public PropertyInfo? CurrentField { get; set; }

    /// <summary>
    /// Последнее отправленное ботом сообщение
    /// </summary>
    public Message? LastMessage { get; internal set; }

    /// <summary>
    /// Профиль пользователя
    /// </summary>
    public UserProfile userProfile { get; set; }
}