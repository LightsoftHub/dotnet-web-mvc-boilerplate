using CleanArch.eCode.Shared.Notifications;
using Microsoft.AspNetCore.SignalR.Client;
using System.Text.Json.Serialization;

namespace CleanArch.eCode.WebBlazor.Infrastructure.Services;

public class SignalRClientService
{
    private HubConnection? _hubConnection;

    public event Action? OnMessageReceived;

    public async Task ConnectAsync(string hubUrl, string? accessToken)
    {
        if (_hubConnection != null) return;

        Task<string?> AccessTokenProvider() => Task.FromResult(accessToken);

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl, options =>
            {
                options.AccessTokenProvider = AccessTokenProvider; // Provide the access token
                options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.LongPolling;
            })
            .WithAutomaticReconnect()
            .ConfigureLogging(logging =>
            {
                logging.AddConsole(); // Add console logging
                logging.SetMinimumLevel(LogLevel.Information); // Set log level to debug
            })
            .Build();

        await _hubConnection.StartAsync();

        // Listen for incoming messages
        _hubConnection.On(NotificationConstants.SERVER_NOTIFICATION, () =>
        {
            OnMessageReceived?.Invoke();
        });
    }

    public async Task SendMessageAsync(string user, string message)
    {
        if (_hubConnection != null && _hubConnection.State == HubConnectionState.Connected)
        {
            await _hubConnection.SendAsync("SendMessage", user, message);
        }
    }

    public async Task DisconnectAsync()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.StopAsync();
            await _hubConnection.DisposeAsync();
            _hubConnection = null;
        }
    }
}
