using Microsoft.AspNetCore.Mvc;

namespace CleanArch.eCode.WebApp.Areas.System.Controllers;

[Area("System")]
public class SystemController : Controller
{
}

public class SystemController<T> : SystemController
{
    /// <summary>
    /// Abstract BaseApi Controller Class
    /// </summary>
    private ILogger<T> _logger = null!;

    protected ILogger<T> Logger =>
        _logger ??= HttpContext.RequestServices.GetRequiredService<ILogger<T>>();
}