using CleanArch.eCode.Infrastructure.Auth.Permissions;
using CleanArch.eCode.Shared.Authorization;
using CleanArch.eCode.Shared.Notifications;
using CleanArch.eCode.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.eCode.WebApp.Controllers;

[AutoValidateAntiforgeryToken]
public class NotificationsController(
    NotificationHttpService notificationService,
    UserHttpService userHttpService,
    ICurrentUser currentUser) : Controller
{

    #region[API]
    [DenyDirectAccess]
    public async Task<IActionResult> UserEntries()
    {
        var userId = currentUser.UserId;

        var request = new NotificationLookup { ToUser = userId };
        var response = await notificationService.GetAsync(request);

        var unreadCount = await notificationService.CountUnreadAsync(userId!);

        var vm = new UserNotificationsVm
        {
            Records = response.Data,
            Unread = unreadCount.Data
        };

        return Ok(vm);
    }
    #endregion

    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Index(NotificationLookup request)
    {
        if (Request.Method == "GET")
        {
            return View();
        }

        request.ToUser ??= currentUser.UserId;

        // for test paged
        request.PageSize = 3;

        var result = await notificationService.GetAsync(request);

        return PartialView("_EntriesPartial", result.ToPagedVm(request));
    }

    public async Task<IActionResult> Details(string id)
    {
        await notificationService.GetByIdAsync(id);

        return RedirectToAction("Index");
    }

    [DenyDirectAccess]
    [MustHavePermission(Permissions.System.Notification)]
    public IActionResult Create()
    {
        return PartialView("_NotifyToUserModal");
    }

    [HttpPost]
    [MustHavePermission(Permissions.System.Notification)]
    public async Task<IActionResult> CreateAsync(string? notifyFrom, string notifyTo, string title, string content, string? url)
    {
        notifyFrom ??= currentUser.UserId ?? "";

        var user = await userHttpService.GetByIdAsync(notifyFrom);

        var message = new SystemMessage
        {
            FromUserId = notifyFrom,
            FromName = user.Data.FirstName + " " + user.Data.LastName,
            ToUserId = notifyTo,
            Title = title,
            Message = content,
            Url = url,
        };

        var result = await notificationService.CreateAsync(message);

        return result.AsJson();
    }
}