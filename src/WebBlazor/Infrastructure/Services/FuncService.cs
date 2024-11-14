using Light.Contracts;
using MudBlazor;

namespace CleanArch.eCode.WebBlazor.Infrastructure.Services;

public class FuncService(ISnackbar snackbar)
{
    public async Task<Result> CallGuardedAsync(Func<Task<Result>> call, string successMessage)
    {
        try
        {
            var result = await call();

            if (result.Succeeded)
            {
                snackbar.Add(successMessage, severity: Severity.Success);
            }
            else
            {
                snackbar.Add(result.Message, severity: Severity.Error);
            }

            return result;
        }
        catch (Exception ex)
        {
            snackbar.Add(ex.Message, severity: Severity.Error);
            return Result.Error(ex.Message);
        }
    }
}
