using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Telegram.Bot;

/// <summary>
/// Класс расширений добавления сервисов.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Расширение для добавления сервисов бота.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="settings">Настройки бота <see cref="BotSettings"/>.</param>
    /// <returns>Обновленная коллекция сервисов.</return>
    public static IServiceCollection AddTelegramBot(this IServiceCollection services, BotSettings settings)
    {
        services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(settings.Token));
        services.AddSingleton<BotStateManager>();
        services.AddTransient<ICommandHandler, StartCommandHandler>();
        services.AddTransient<ICommandHandler, UserProfileHandler>();
        services.AddTransient<ICommandHandler, ActivityHandler>();
        services.AddHostedService<TelegramBotService>();

        // Добавьте другие обработчики команд здесь
        return services;
    }
}