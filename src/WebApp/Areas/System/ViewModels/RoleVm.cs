namespace CleanArch.eCode.WebApp.Areas.System.ViewModels;

public class RoleVm
{
    public string Id { get; set; } = default!;

    public string? Name { get; set; }

    public string? Description { get; set; }

    public IEnumerable<string> Claims { get; set; } = new List<string>();
}