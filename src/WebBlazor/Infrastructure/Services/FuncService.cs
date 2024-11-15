using CleanArch.eCode.WebBlazor.Components.Shared.Spinner;
using Light.Contracts;
using Microsoft.FluentUI.AspNetCore.Components;

namespace CleanArch.eCode.WebBlazor.Infrastructure.Services;

public class FuncService(
    IToastService toastService,
    SpinnerService spinnerService)
{
    public async Task<Result> CallGuardedAsync(Func<Task<Result>> call, string successMessage)
    {
        spinnerService.Show();

        Result result;

        try
        {
            result = await call();

            if (result.Succeeded)
            {
                toastService.ShowSuccess(successMessage);
            }
            else
            {
                toastService.ShowError(result.Message);
            }
        }
        catch (Exception ex)
        {
            toastService.ShowError(ex.Message);

            result = Result.Error(ex.Message);
        }

        spinnerService.Hide();

        return result;
    }

    public async Task<Result> CallGuardedAsync(Func<Task<Result>> call, string successMessage, Func<Task<Result>> runIfSuccess)
    {
        var result = await CallGuardedAsync(call, successMessage);

        if (result.Succeeded)
        {
            await runIfSuccess();
        }

        return result;
    }

    public async Task<Result> CallGuardedAsync(Func<Task<Result>> call, string successMessage, Func<Task> runIfSuccess)
    {
        var result = await CallGuardedAsync(call, successMessage);

        if (result.Succeeded)
        {
            await runIfSuccess();
        }

        return result;
    }

    public async Task CallGuardedAsync(Func<Task<Result>> call, string successMessage, FluentDialog fluentDialog)
    {
        var result = await CallGuardedAsync(call, successMessage);

        if (result.Succeeded)
        {
            await fluentDialog.CloseAsync(result);
        }
    }
}
