using CleanArch.eCode.Shared.Common.Dtos;
using CleanArch.eCode.WebApp.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.eCode.WebApp.Core.Extensions;

public static class ResultExtensions
{
    private const string _break = "<br>";

    public static WebResult ToWebResult(this Light.Contracts.IResult result)
    {
        var webResult = new WebResult
        {
            Status = result.Succeeded ? Status.success : Status.error,
            Message = result.Message,
        };

        if (result.Errors != null && result.Errors.Any())
        {
            webResult.Message += _break + string.Join(_break, result.Errors);
        }

        return webResult;
    }

    public static JsonResult AsJson(this Light.Contracts.IResult result, bool successCode = false)
    {
        var webResult = result.ToWebResult();

        if (successCode is true)
        {
            return new JsonResult(webResult)
            {
                StatusCode = 200
            };
        }

        return new JsonResult(webResult)
        {
            StatusCode = (int)result.Code
        };
    }

    public static JsonResult AsJson<T>(this Light.Contracts.IResult<T> result, bool fullResult = false)
    {
        if (result.Succeeded && fullResult == false)
            return new JsonResult(result.Data);

        var webResult = result.ToWebResult();

        return new JsonResult(webResult)
        {
            StatusCode = (int)result.Code
        };
    }

    public static JsonResult AsJson(this WebResult result)
    {
        return new JsonResult(result)
        {
            StatusCode = result.Status == Status.success ? 200 : 500,
        };
    }

    public static PagedView<TResult> ToPagedVm<TResult>(this PagedResult<TResult> pagedData, PageLookup searchModel)
    {
        return new PagedView<TResult>(searchModel, pagedData);
    }
}