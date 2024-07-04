using System.Text.Json.Serialization;

namespace CleanArch.eCode.WebApp.Core.ViewModels;

public enum Status
{
    success = 0,
    error = 1,
    warning = 2,
}

public class WebResult
{

    [JsonPropertyName("status")]
    public Status Status { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    public static WebResult Success(string message = "")
    {
        return new WebResult { Status = Status.success, Message = message };
    }

    public static WebResult Error(string message)
    {
        return new WebResult { Status = Status.error, Message = message };
    }
}