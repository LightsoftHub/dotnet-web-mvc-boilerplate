namespace CleanArch.eCode.Shared.Notifications;

public partial class NotificationDto
{
    public string Id { get; set; } = null!;

    public string FromUserId { get; set; } = null!;

    public string? FromName { get; set; }

    public string ToUserId { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Message { get; set; }

    public string? Url { get; set; }

    public bool MarkAsRead { get; set; }

    public DateTimeOffset CreatedOn { get; set; }
}