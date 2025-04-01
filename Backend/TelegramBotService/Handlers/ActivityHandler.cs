
using Telegram.Bot.Types.ReplyMarkups;

public class ActivityHandler : BaseDialogHandler<Activity>
{
    private readonly IDataService dataService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActivityHandler"/> class.
    /// </summary>
    /// <param name="stateManager"></param>
    /// <param name="dataService"></param>
    public ActivityHandler(BotStateManager stateManager, IDataService dataService) : base(stateManager)
    {
        this.dataService = dataService;
    }

    public override string CommandName => "/addActivity";

    protected override InlineKeyboardMarkup SaveUserStateModel(DialogState<Activity> userState)
    {
        dataService.CreateOrUpdateActivity(userState.Model, userState.userProfile);
        var buttons = new[]
        {
            InlineKeyboardButton.WithCallbackData("Отчет","report_"),
        InlineKeyboardButton.WithCallbackData("Изменить","report_"),
       InlineKeyboardButton.WithCallbackData("Отменит","report_")
       };
        return new InlineKeyboardMarkup(buttons);
    }
}