using Light.Identity;
using System.Net.Http.Json;

namespace CleanArch.eCode.HttpApi.Client.Identity;

public class TokenHttpService(IHttpClientFactory httpClientFactory) :
    HttpClientBase(httpClientFactory)
{
    public async Task<Result<TokenDto>> GetTokenAsync(string userName, string password)
    {
        var url = "api/v1/oauth/get_token";

        var request = new { userName, password };

        var res = await HttpClient.PostAsJsonAsync(url, request);

        return await res.ReadResultAsync<TokenDto>();
    }
}