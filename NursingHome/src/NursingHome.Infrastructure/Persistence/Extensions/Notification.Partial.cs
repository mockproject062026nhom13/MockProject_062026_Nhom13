using System;

namespace NursingHome.Infrastructure.Persistence.Generated;

public partial class Notification
{
    public Notification(string title, string type, long userId)
    {
        Title = title;
        Type = type;
        UserId = userId;
        IsRead = false;
        CreatedAt = DateTimeOffset.UtcNow;
    }
}
