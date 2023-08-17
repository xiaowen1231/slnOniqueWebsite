using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Infra;
using System.Configuration;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<OniqueContext>(
    options=>options.UseSqlServer(builder.Configuration.GetConnectionString("OniqueConnection"))
    );
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<UserInfoService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.LoginPath = new PathString("/Home/Login");

    option.AccessDeniedPath = new PathString("/Home/NoRole");

    option.ExpireTimeSpan = TimeSpan.FromDays(1);
});

builder.Services.AddSession();

builder.Services.AddSingleton(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));
var memoryCache = new MemoryCache(new MemoryCacheOptions());

builder.Services.AddSingleton<IMemoryCache>(memoryCache);

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

app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
