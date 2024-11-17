using CleanArch.eCode.Shared.Common.Dtos;
using CleanArch.eCode.WebApp.Core.ViewModels;
using Light.Contracts;
using Microsoft.AspNetCore.Mvc;
using IResult = Light.Contracts.IResult;

namespace CleanArch.eCode.WebApp.Core.Extensions;

public static class ResultExtensions
{
    private const string _break = "<br>";

    public static WebResult ToWebResult(this IResult result)
    {
        var webResult = new WebResult
        {
            Status = result.Succeeded ? Status.success : Status.error,
            Message = result.Message,
        };

        webResult.Message = result.Message.Replace("|", _break);

        return webResult;
    }

    public static JsonResult AsJson(this IResult result, bool successCode = false)
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
            StatusCode = (int)result.MapHttpStatusCode()
        };
    }

    public static JsonResult AsJson<T>(this Light.Contracts.IResult<T> result, bool fullResult = false)
    {
        if (result.Succeeded && fullResult == false)
            return new JsonResult(result.Data);

        var webResult = result.ToWebResult();

        return new JsonResult(webResult)
        {
            StatusCode = (int)result.MapHttpStatusCode()
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