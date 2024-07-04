using Microsoft.AspNetCore.Mvc.Rendering;

namespace CleanArch.eCode.WebApp.Core.Helpers;

public static class SideMenuHelpers
{
    public static string IsSelected(this IHtmlHelper html, string? controller = null, string? action = null, string? cssClass = null)
    {
        if (string.IsNullOrEmpty(cssClass))
            cssClass = "active";

        string? currentAction = html.ViewContext.RouteData.Values["action"]?.ToString();
        string? currentController = html.ViewContext.RouteData.Values["controller"]?.ToString();

        if (string.IsNullOrEmpty(controller))
            controller = currentController;

        if (string.IsNullOrEmpty(action))
            action = currentAction;

        return controller == currentController && action == currentAction ?
            cssClass : string.Empty;
    }

    public static string? PageClass(this IHtmlHelper htmlHelper)
    {
        return htmlHelper.ViewContext.RouteData.Values["action"]?.ToString();
    }

}
