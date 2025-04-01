// DatabaseService/Services/ITrainingService.cs

/// <summary>
/// Контракт сервиса доступа к данным.
/// </summary>
/// <remarks>
/// Определяет контракты методов манипуляции с данными.
/// </remarks>
public interface IDataService
{
    /// <summary>
    /// Забронировать место на активности.
    /// </summary>
    /// <param name="chatId">Идентификатор чата с пользователем откуда произошла запись.</param>
    /// <param name="activityId">Идентификатор мероприятия.</param>
    /// <returns><see cref="Task"/> Асинхронная операция резервирования места.</returns>
    Task<Activity> Reservation(long chatId, int activityId);

    /// <summary>
    /// Создать активность.
    /// </summary>
    /// <param name="activity">Активность.</param>
    /// <param name="supposedFacilitator">Предполагаемый.</param>
    /// <returns>Объект активности.</returns>
    Task<Activity> CreateOrUpdateActivity(Activity activity, UserProfile supposedFacilitator);

    /// <summary>
    /// Найти доступные активности по идентификатору чата.
    /// </summary>
    /// <param name="id">Идентификатор чата.</param>
    /// <returns>Список доступных активносте.</returns>
    Task<List<Activity>> FindAvaibleActivities(long id);
}