using System.Net.Http.Json;

namespace CleanArch.eCode.HttpApi.Client;

public class TryHttpClient(IHttpClientFactory httpClientFactory) :
    HttpClientBase(httpClientFactory)
{
    protected virtual async Task<Result> TryGetAsync(string url)
    {
        try
        {
            var response = await HttpClient.GetAsync(url);

            return await response.ReadResultAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }

    protected virtual async Task<Result<T>> TryGetAsync<T>(string url)
    {
        try
        {
            var response = await HttpClient.GetAsync(url);

            return await response.ReadResultAsync<T>();
        }
        catch (Exception ex)
        {
            return Result<T>.Error(ex.Message);
        }
    }

    protected virtual async Task<PagedResult<T>> TryGetPagedAsync<T>(string url)
    {
        try
        {
            var response = await HttpClient.GetAsync(url);

            return await response.ReadPagedResultAsync<T>();
        }
        catch (Exception ex)
        {
            return new PagedResult<T>
            {
                Code = ResultCode.Error,
                Message = ex.Message
            };
        }
    }

    protected virtual async Task<Result> TryPostAsync<T>(string url, T value)
    {
        try
        {
            var response = await HttpClient.PostAsJsonAsync(url, value);

            return await response.ReadResultAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }

    protected virtual async Task<Result> TryPutAsync<T>(string url, T value)
    {
        try
        {
            var response = await HttpClient.PutAsJsonAsync(url, value);

            return await response.ReadResultAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }

    protected virtual async Task<Result> TryDeleteAsync(string url)
    {
        try
        {
            var response = await HttpClient.DeleteAsync(url);

            return await response.ReadResultAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
