namespace CleanArch.eCode.WebBlazor.Infrastructure.Services;

public class WebConfiguration(IConfiguration configuration) : ISettings
{
    public string GetSignalRUrl() =>
        configuration.GetValue<string>("BackendUrls:SignalR_Hub") ?? "";
}
