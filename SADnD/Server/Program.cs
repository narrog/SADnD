using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using SADnD.Server.Data;
using SADnD.Shared.Models;
using Microsoft.AspNetCore.Identity;
using SADnD.Server.Areas.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
    .AddInMemoryIdentityResources(Config.IdentityResources) 
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>()
    .AddProfileService<CustomProfileService>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsDungeonMaster", policy => policy.RequireAssertion(context =>
    {
        var routeData = context.Resource as HttpContext;
        var campaignId = routeData.GetRouteValue("id")?.ToString();
        return context.User.Claims.Any(c => c.Type == "CampaignRole" && c.Value == $"{campaignId}:DungeonMaster");
    }));
    options.AddPolicy("IsPlayer", policy => policy.RequireAssertion(context =>
    {
        var routeData = context.Resource as HttpContext;
        var campaignId = routeData.GetRouteValue("id")?.ToString();
        return context.User.Claims.Any(c => c.Type == "CampaignRole" && c.Value == $"{campaignId}:Player");
    }));
});
builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

builder.Services.AddTransient<EFRepositoryGeneric<Campaign, ApplicationDbContext>>();
builder.Services.AddTransient<CustomClaimsService<ApplicationDbContext,UserManager<ApplicationUser>>>();

builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomUserClaimsPrincipalFactory>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Executes Database Migrations, tries 5 times if Database Container isn't running yet
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    int retries = 5;
    while (retries > 0)
    {
        try
        {
            context.Database.Migrate();
            break;
        }
        catch (Exception ex) 
        {
            retries--;
            // TODO: Logging
            await Task.Delay(5000);
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();