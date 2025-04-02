/// <summary>
/// Менеджер для управления состоянием пользователей.
/// </summary>
public class BotStateManager
{
    private readonly Dictionary<long, UserState> _userStates = new();

    /// <summary>
    /// Получает состояние пользователя по его идентификатору.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Состояние пользователя.</returns>
    public UserState GetUserState(long userId)
    {
        if (!_userStates.ContainsKey(userId))
        {
            _userStates[userId] = new UserState(userId);
        }
        
        return _userStates[userId];
    }

    /// <summary>
    /// Очищает состояние пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    public void ClearUserState(long userId)
    {
        if (_userStates.ContainsKey(userId))
        {
            _userStates.Remove(userId);
        }
    }
}