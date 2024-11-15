namespace CleanArch.eCode.WebBlazor.Components.Shared.Settings;

public interface IStaticAssetService
{
    public Task<string?> GetAsync(string assetUrl, bool useCache = true);
}
