using CleanArch.eCode.Shared.Notifications;

namespace CleanArch.eCode.WebApp.Models;

public class UserNotificationsVm
{
    public IEnumerable<NotificationDto> Records { get; set; } = null!;

    public int Unread { get; set; }
}
