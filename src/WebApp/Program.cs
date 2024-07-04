using CleanArch.eCode.WebApp;
using CleanArch.eCode.WebApp.Extensions;
using CleanArch.eCode.WebApp.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Cookie settings
        //options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(12);

        options.LoginPath = "/auth/login";
        options.AccessDeniedPath = "/access-denied";
        options.SlidingExpiration = true;
    });

builder.Services
    .AddRouting(options => options.LowercaseUrls = true)
    .Configure<RazorViewEngineOptions>(opts =>
            opts.ViewLocationExpanders.Add(new CustomViewLocationExpander()))
    .AddControllersWithViews();

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/");
});

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.ConfigurePipelines(builder.Configuration);

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapControllers().RequireAuthorization();

app.Run();
