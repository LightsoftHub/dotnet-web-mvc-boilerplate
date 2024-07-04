using Microsoft.AspNetCore.Mvc.Razor;

namespace CleanArch.eCode.WebApp.Extensions
{
    public class CustomViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            return new[]
            {
                "~/Areas/{2}/Views/{1}/{0}.cshtml", // for areas
                "~/Views/{1}/{0}.cshtml", // default MVC
                "~/Views/Shared/{0}.cshtml", // default MVC

                "~/{1}/Views/{0}.cshtml", // custom views at root/[features]/views

                "~/Modules/{1}/Views/{0}.cshtml", // custom views at root/Modules/[module-names]/[features]/views
            };
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            // Nothing needs to be done here
        }
    }
}