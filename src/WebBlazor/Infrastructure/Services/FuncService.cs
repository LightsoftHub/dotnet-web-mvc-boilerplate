using Light.Contracts;
using Microsoft.FluentUI.AspNetCore.Components;

namespace CleanArch.eCode.WebBlazor.Infrastructure.Services;

public class FuncService(IToastService toastService)
{
    public async Task<Result> CallGuardedAsync(Func<Task<Result>> call, string successMessage)
    {
        try
        {
            var result = await call();

            if (result.Succeeded)
            {
                toastService.ShowSuccess(successMessage);
            }
            else
            {
                toastService.ShowError(result.Message);
            }

            return result;
        }
        catch (Exception ex)
        {
            toastService.ShowError(ex.Message);

            return Result.Error(ex.Message);
        }
    }
}
