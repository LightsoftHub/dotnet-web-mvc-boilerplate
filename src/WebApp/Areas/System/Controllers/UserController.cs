using CleanArch.eCode.Infrastructure.Auth.Permissions;
using CleanArch.eCode.Shared.Authorization;
using CleanArch.eCode.WebApp.Areas.System.ViewModels;
using CleanArch.eCode.WebApp.Core.ViewModels;
using Light.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.eCode.WebApp.Areas.System.Controllers;

[MustHavePermission(Permissions.Users.View)]
public class UserController(
    ICurrentUser currentUser,
    UserHttpService userService,
    RoleHttpService roleService) : SystemController
{
    #region[Api]

    [DenyDirectAccess]
    public async Task<IActionResult> Search(string? search)
    {
        var isMasterUser = currentUser.IsMasterUser;

        var getUsers = await userService.GetAsync();

        var users = getUsers.Data;

        if (!isMasterUser)
        {
            users = users.Where(x => !DefaultUser.MASTER_USERS.Contains(x.UserName!)).ToList();
        }

        if (!string.IsNullOrEmpty(search))
        {
            users = users.Where(x =>
                x.UserName.Contains(search)
                || x.FirstName.Contains(search)
                || x.LastName.Contains(search)
                ).ToList();
        }

        var response = users.Select(x => new SelectItem(x.Id, $"[{x.UserName}] - {x.LastName} {x.FirstName}")).ToList();
        response.Insert(0, new SelectItem("", "--"));

        return Ok(response);
    }

    [DenyDirectAccess]
    public async Task<IActionResult> GetDomainUser(string userName)
    {
        var user = await userService.GetDomainUserAsync(userName);

        return user.AsJson();
    }

    #endregion

    public async Task<IActionResult> Index()
    {
        var model = await userService.GetAsync();
        return View(model.Data);
    }

    [DenyDirectAccess]
    [MustHavePermission(Permissions.Users.Create)]
    public IActionResult Create()
    {
        return PartialView("_CreateUserModal");
    }

    [HttpPost]
    [MustHavePermission(Permissions.Users.Create)]
    public async Task<IActionResult> Create(CreateUserRequest request)
    {
        var res = await userService.CreateAsync(request);

        return res.AsJson(true);
    }

    [MustHavePermission(Permissions.Users.Update)]
    public async Task<IActionResult> Update(string id)
    {
        var user = await userService.GetByIdAsync(id);

        var currentUserDetails = await userService.GetByIdAsync(currentUser.UserId!);
        var currentUserRoles = currentUserDetails.Data.Roles;

        if (currentUser.IsMasterUser)
        {
            var allRoles = await roleService.GetAsync();
            currentUserRoles = allRoles.Data
                .Select(s => s.Name!)
                .ToList();
        }

        ViewBag.UserRoles = currentUserRoles
            .Select(role => new UserRoleVm
            {
                RoleName = role,
                IsOwned = user.Data.Roles.Contains(role),
            }).ToList();

        return View(user.Data);
    }

    [HttpPost]
    [MustHavePermission(Permissions.Users.Update)]
    public async Task<IActionResult> Update(UserDto request, List<UserRoleVm> userRoles)
    {
        request.Roles = userRoles.Where(x => x.IsOwned == true).Select(s => s.RoleName);

        var result = await userService.UpdateAsync(request);

        return result.AsJson();
    }

    [MustHavePermission(Permissions.Users.Delete)]
    public async Task<IActionResult> Delete(string id)
    {
        await userService.DeleteAsync(id);

        return RedirectToAction("Index");
    }

    [HttpPost]
    [MustHavePermission(Permissions.Users.Update)]
    public async Task<IActionResult> ForcePassword(string id, string password)
    {
        var result = await userService.ForcePasswordAsync(id, password);

        return result.AsJson();
    }
}