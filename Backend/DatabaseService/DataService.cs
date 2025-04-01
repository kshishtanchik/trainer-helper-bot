// DatabaseService/Services/DataService.cs

using Microsoft.Extensions.DependencyInjection;

namespace DatabaseService;

/// <summary>
/// Сервис управления данными.
/// </summary>
/// <param name="dbContext">Контекст базы данных.</param>
public class DataService : IDataService
{
    private readonly BotDbContext dbContext;

    /// <summary>
    /// Создать сервис.
    /// </summary>
    /// <param name="serviceProvider">Контекст базыданных.</param>
    public DataService(IServiceProvider serviceProvider)
    {
        dbContext = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<BotDbContext>();
    }

    /// <summary>
    /// Создать или обновить существующую активность.
    /// </summary>
    /// <remarks>Обновляемая активность определяется по id.</remarks>
    /// <param name="activity">Объект активности.</param>
    /// <param name="supposedFacilitator">Предполагаемый фасилитатор.</param>
    /// <returns>Созданная или обновленная активность.</returns>
    public async Task<Activity> CreateOrUpdateActivity(Activity activity, UserProfile supposedFacilitator)
    {
        var facilitator = dbContext.UserProfiles.FirstOrDefault(p => p.ChatId == supposedFacilitator.ChatId);
        
        facilitator ??= dbContext.UserProfiles.Add(supposedFacilitator).Entity;

        activity.Facilitator = facilitator;
        await dbContext.Activities.AddAsync(activity);
        await dbContext.SaveChangesAsync();
        return activity;
    }

    public Task<List<Activity>> FindAvaibleActivities(long id)
    {
         //todo: 
        // по ид чата или пользователя найти  активности где он учавствовал -> 
        // достать организаторов и тренеров-> 
        var user=dbContext.UserProfiles.FirstOrDefault(x=>x.ChatId==id);
        
        throw new NotImplementedException();
    }

    /// <summary>
    /// Зарезарвировать место на мероприятии.
    /// </summary>
    /// <param name="chatId">Идентификатор чата с пользователем.</param>
    /// <param name="activityId">Идентификатор мероприятия.</param>
    /// <returns>Описание мероприятия.</returns>
    public async Task<Activity> Reservation(long chatId, int activityId)
    {
        var activity = dbContext.Activities.FirstOrDefault(x => x.Id == activityId) ?? throw new NotFoundActivityException(activityId);
        var user = dbContext.UserProfiles.FirstOrDefault(p => p.ChatId == chatId);
        if (user == null)
        {
            user = new UserProfile(chatId);
            dbContext.UserProfiles.Add(user);
        }

        activity.Bookings.Add(new Booking()
        {
            Activity = activity,
            User = user
        });
        await dbContext.SaveChangesAsync();
        return activity;
    }
}