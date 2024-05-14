using ActionCommandGame.Sdk;
using ActionCommandGame.Security.Model.Abstractions;
using ActionCommandGame.Settings;
using ActionCommandGame.UI.Mvc.Stores;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var apiSettings = new ApiSettings();
builder.Configuration.GetSection(nameof(ApiSettings)).Bind(apiSettings);

builder.Services.AddHttpClient("ActionCommandApi", options =>
{
    if (!string.IsNullOrWhiteSpace(apiSettings.BaseAddress))
    {
        options.BaseAddress = new Uri(apiSettings.BaseAddress);
    }
});

//Register SDK
builder.Services.AddScoped<IdentitySdk>();
builder.Services.AddScoped<GameSdk>();
builder.Services.AddScoped<ItemSdk>();
builder.Services.AddScoped<PlayerSdk>();
builder.Services.AddScoped<PlayerItemSdk>();
builder.Services.AddScoped<PositiveGameEventSdk>();
builder.Services.AddScoped<NegativeGameEventSdk>();

builder.Services.AddScoped<ITokenStore, TokenStore>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Account/SignIn";
});

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
    