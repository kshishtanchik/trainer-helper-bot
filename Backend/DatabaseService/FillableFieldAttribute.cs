/// <summary>
/// Атрибут для пометки полей, которые нужно заполнить в диалоге.
/// </summary>
/// <remarks>
/// Инициализирует новый экземпляр класса <see cref="FillableFieldAttribute"/>.
/// </remarks>
/// <param name="prompt">Подсказка для пользователя.</param>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FillableFieldAttribute(string prompt) : Attribute
{
    /// <summary>
    /// Подсказка для пользователя, которая будет показана при запросе значения поля.
    /// </summary>
    public string Prompt { get; } = prompt;
}