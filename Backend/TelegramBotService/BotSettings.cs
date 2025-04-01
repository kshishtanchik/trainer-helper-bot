using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Настройки бота.
/// </summary>
public class BotSettings
{
    /// <summary>
    /// Токен бота.
    /// </summary>
    [NotNull]
    required public string Token { get; set; }
}