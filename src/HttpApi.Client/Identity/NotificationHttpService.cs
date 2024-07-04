using CleanArch.eCode.Shared.Notifications;

namespace CleanArch.eCode.HttpApi.Client.Identity;

public class NotificationHttpService(IHttpClientFactory httpClientFactory) :
    TryHttpClient(httpClientFactory)
{
    private const string _path = "api/v1/signalr";

    public Task<PagedResult<NotificationDto>> GetAsync(NotificationLookup request)
    {
        var url = $"{_path}";
        url += "?" + request.BuildQuery();

        return TryGetPagedAsync<NotificationDto>(url);
    }

    public Task<Result<NotificationDto>> GetByIdAsync(string id)
    {
        var url = $"{_path}/{id}";

        return TryGetAsync<NotificationDto>(url);
    }

    public Task<Result<int>> CountUnreadAsync(string userId)
    {
        var url = $"{_path}/{userId}/count_unread";

        return TryGetAsync<int>(url);
    }

    public Task<Result> CreateAsync(SystemMessage message)
    {
        var url = $"{_path}";

        return TryPostAsync(url, message);
    }
}
