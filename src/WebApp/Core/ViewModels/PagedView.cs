using Light.Contracts;

namespace CleanArch.eCode.WebApp.Core.ViewModels;

public interface IPagedView : IPagedInfo
{
    object? SearchModel { get; }

    string? Url { get; }

    string? ShowResultInId { get; }
}

public class PagedView<TModel> : PagedInfo, IPagedView
{
    public PagedView() { }

    public PagedView(object searchModel, PagedResult<TModel> pagedData)
    {
        SearchModel = searchModel;

        Page = pagedData.PagedInfo.Page;
        PageSize = pagedData.PagedInfo.PageSize;
        TotalRecords = pagedData.PagedInfo.TotalRecords;
        TotalPages = pagedData.PagedInfo.TotalPages;

        Records = pagedData.Data;
    }

    public object? SearchModel { get; set; }
    public string? Url { get; private set; }
    public string? ShowResultInId { get; private set; }

    public IEnumerable<TModel> Records { get; private set; } = [];

    public IPagedView SetSearchView(string? url, string showResultInId = "#content")
    {
        Url = url;
        ShowResultInId = showResultInId;

        return this;
    }
}
