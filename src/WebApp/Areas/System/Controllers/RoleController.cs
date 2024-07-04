using CleanArch.eCode.Infrastructure.Auth.Permissions;
using CleanArch.eCode.Shared.Authorization;
using CleanArch.eCode.WebApp.Areas.System.ViewModels;
using Light.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.eCode.WebApp.Areas.System.Controllers;

[AutoValidateAntiforgeryToken]
[MustHavePermission(Permissions.Roles.View)]
public class RoleController(RoleHttpService roleService) : SystemController
{
    public async Task<IActionResult> Index()
    {
        var model = await roleService.GetAsync();
        return View(model.Data);
    }

    [MustHavePermission(Permissions.Roles.Create)]
    public IActionResult Create()
    {
        return PartialView("_CreateRoleModal");
    }

    [HttpPost]
    [MustHavePermission(Permissions.Roles.Create)]
    public async Task<IActionResult> Create(CreateRoleRequest request)
    {
        var res = await roleService.CreateAsync(request);

        return res.AsJson(true);
    }

    [DenyDirectAccess]
    public async Task<IActionResult> Update(string id)
    {
        var role = await roleService.GetByIdAsync(id);

        if (role.Succeeded)
        {
            var vm = new RoleVm
            {
                Id = role.Data.Id,
                Name = role.Data.Name,
                Description = role.Data.Description,
                Claims = role.Data.Claims.Select(s => s.Value).ToList()
            };

            return View(vm);
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    [MustHavePermission(Permissions.Roles.Update)]
    public async Task<IActionResult> Update(RoleVm request)
    {
        var dto = new RoleDto
        {
            Id = request.Id,
            Name = request.Name,
            Description = request.Description,
            Claims = request.Claims.Select(s => new ClaimDto
            {
                Type = ClaimTypes.Permission,
                Value = s
            })
        };

        var result = await roleService.UpdateAsync(dto);

        return result.AsJson();
    }

    [MustHavePermission(Permissions.Roles.Delete)]
    public async Task<IActionResult> Delete(string id)
    {
        await roleService.DeleteAsync(id);

        return RedirectToAction("Index");
    }
}