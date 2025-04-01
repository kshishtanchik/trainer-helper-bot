/// <summary>
/// Состояние пользователя в диалоге с ботом.
/// </summary>
public class UserState
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Текущая команда, выполняемая пользователем.
    /// </summary>
    public string CurrentCommand { get; set; }

    /// <summary>
    /// Текущий шаг в диалоге.
    /// </summary>
    public int CurrentStep { get; set; }

    /// <summary>
    /// Текущее поле в диалоге.
    /// </summary>
    public int CurrentField { get; set; }

    /// <summary>
    /// Данные, введенные пользователем.
    /// </summary>
    public Dictionary<string, string> Data { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="UserState"/>.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    public UserState(long userId)
    {
        UserId = userId;
        Data = new Dictionary<string, string>();
    }
}