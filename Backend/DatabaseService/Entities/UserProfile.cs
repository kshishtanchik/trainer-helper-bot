using System.ComponentModel.DataAnnotations;

/// <summary>
/// Профиль пользователя.
/// </summary>
public class UserProfile
{
    public UserProfile()
    {
        
    }
    /// <summary>
    /// Создать профиль по данным чата.
    /// </summary>
    /// <param name="chatId">Идентификатор чата.</param>
    /// <param name="telegramAlias">Никнейм в телеграм для обращения через символ @.</param>
    public UserProfile(long chatId, string? telegramAlias=null)
    {
        ChatId = chatId;
        TelegramAlias = telegramAlias;
    }

    /// <summary>
    /// Идентификатор чата.
    /// </summary>
    [Key]
    public long ChatId { get; set; }

    /// <summary>
    /// Номер телефона для прямой связи.
    /// </summary>
    [FillableField("Телефон:")]
    public string? Phone { get; set; }

    /// <summary>
    /// Имя пользователя, как к нему обращаться
    /// </summary>
    [FillableField("Имя")]
    public string? Name { get; set; }

    /// <summary>
    /// Никнейм из телеграмм, если задан.
    /// </summary>
    public string? TelegramAlias { get; set; }
}