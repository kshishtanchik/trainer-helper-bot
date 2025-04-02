using Telegram.Bot.Types.ReplyMarkups;

public class UserProfileHandler : BaseDialogHandler<UserProfile>
{
    public UserProfileHandler(BotStateManager stateManager, IDataService dataService) : base(stateManager)
    {
    }

    public override string CommandName => "/profile";

    protected override InlineKeyboardMarkup SaveDialogStateModel(DialogState<UserProfile> userState)
    {
        return new InlineKeyboardMarkup();
    }
}