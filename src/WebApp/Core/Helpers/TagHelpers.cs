using CleanArch.eCode.WebApp.Core.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CleanArch.eCode.WebApp.Core.Helpers;

/// <summary>
/// Default modal header stype
/// </summary>
[HtmlTargetElement("s-modal-header")]
public class ModalHeader : TagHelper
{
    [ViewContext]
    public ViewContext ViewContext { get; set; } = default!;

    public string? Header { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        output.Attributes.SetAttribute("class", "modal-header");

        var viewData = ViewContext.ViewData["Title"];

        output.PreContent.AppendHtml($"<h5 class=\"modal-title\">{Header ?? viewData}</h5>");
        output.PreContent.AppendHtml($"<button type=\"button\" class=\"btn-close\" data-bs-dismiss=\"modal\" aria-label=\"Close\"></button>");
    }
}

/// <summary>
/// Default modal stype
/// </summary>
[HtmlTargetElement("s-modal")]
public class WebModal : TagHelper
{
    public string Id { get; set; } = WebUIConstants.DEFAULT_MODAL_ID;
    public string? Size { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        if (!string.IsNullOrWhiteSpace(Id))
            output.Attributes.SetAttribute("id", Id);

        // set class
        output.Attributes.SetAttribute("class", "modal fade");

        // set others attr
        output.Attributes.SetAttribute("tabindex", "-1");
        output.Attributes.SetAttribute("role", "dialog");
        output.Attributes.SetAttribute("aria-hidden", "true");
        output.Attributes.SetAttribute("data-backdrop", "static");
        output.Attributes.SetAttribute("data-keyboard", "false");

        output.Content.AppendHtml($"<div class=\"modal-dialog {Size}\" role=\"document\">");
        output.Content.AppendHtml("<div class=\"modal-content\">");
        output.Content.AppendHtml("</div>");
        output.Content.AppendHtml("</div>");
    }
}

/// <summary>
/// button for render modal
/// </summary>
[HtmlTargetElement("btn-modal")]
public class BtnModal : TagHelper
{
    public string Target { get; set; } = WebUIConstants.DEFAULT_MODAL_ID;
    public string Url { get; set; } = default!;
    public string? Class { get; set; } = "btn btn-primary my-1";

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "a";
        output.TagMode = TagMode.StartTagAndEndTag;

        if (!string.IsNullOrWhiteSpace(Class))
            output.Attributes.SetAttribute("class", Class);

        var jsOnClick = $"$.render_modal(\"#{Target}\", \"{Url}\")";

        output.Attributes.SetAttribute("onclick", jsOnClick);
        output.Attributes.SetAttribute("href", "javascript:void(0)");
    }
}

/// <summary>
/// Default Button Submit style
/// </summary>
[HtmlTargetElement("btn-submit")]
public class BtnSubmit : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "button";
        output.TagMode = TagMode.StartTagAndEndTag;

        output.Attributes.SetAttribute("class", "btn btn-primary");
        output.Attributes.SetAttribute("type", "submit");

        //output.PostContent.AppendHtml($"<i class=\"far fa-check-circle\"></i> ");
        output.PreContent.AppendHtml($"<i class=\"far fa-check-circle\"></i> ");
    }
}

[HtmlTargetElement("btn-search")]
public class BtnSearch : TagHelper
{
    public string Type { get; set; } = "submit";

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "button";
        output.TagMode = TagMode.StartTagAndEndTag;

        output.Attributes.SetAttribute("class", "btn btn-primary");
        output.Attributes.SetAttribute("type", Type);

        output.PreContent.AppendHtml($"<i class=\"fas fa-search\"></i> ");
    }
}

[HtmlTargetElement("form", Attributes = "ajax")]
public class AjaxForm : TagHelper
{
    public string? Id { get; set; }
    public string OnSuccess { get; set; } = "onAjaxSuccess";
    public string OnError { get; set; } = "onAjaxError";
    public string? UpdateTo { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "form";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("data-ajax", "true");
        output.Attributes.SetAttribute("data-ajax-method", "post");
        output.Attributes.SetAttribute("data-ajax-failure", OnError);

        if (!string.IsNullOrWhiteSpace(Id))
            output.Attributes.SetAttribute("id", Id);

        if (!string.IsNullOrWhiteSpace(UpdateTo))
            output.Attributes.SetAttribute("data-ajax-update", UpdateTo);
        else
            output.Attributes.SetAttribute("data-ajax-success", OnSuccess);
    }
}

/// <summary>
/// Default page header style
/// </summary>
[HtmlTargetElement("page-header")]
public class PageHeader : TagHelper
{
    [ViewContext]
    public ViewContext ViewContext { get; set; } = default!;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        output.Attributes.SetAttribute("class", "page-header");

        var viewData = ViewContext.ViewData["Title"];

        var html = "<div class=\"page-header\">";
        html += "<div class=\"page-block\">";
        html += "<div class=\"row align-items-center\">";
        html += "<div class=\"col-md-12\">";
        html += "<div class=\"page-header-title\">";
        html += $"<h3 class=\"mb-0\">{viewData}</h3>";
        html += "</div>";
        html += "</div>";
        html += "</div>";
        html += "</div>";
        html += "</div>";

        output.PreContent.AppendHtml(html);
    }
}
