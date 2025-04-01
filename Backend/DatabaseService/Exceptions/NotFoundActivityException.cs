// DatabaseService/Services/NotFoundActivity.cs

namespace DatabaseService;

[Serializable]
public class NotFoundActivityException : Exception
{
    private int activityId;

    public NotFoundActivityException()
    {
    }

    public NotFoundActivityException(int activityId) : base($"Не найдена ктивность по идентификатору {activityId}")
    {
        this.activityId = activityId;
    }

    public NotFoundActivityException(string? message) : base(message)
    {
    }

    public NotFoundActivityException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}