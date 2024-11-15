using BlazorDB;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SADnD.Client;
using SADnD.Client.Services;
using SADnD.Client.Shared;
using SADnD.Shared.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("SADnD.ServerAPI", client => client.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}api/"))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("SADnD.ServerAPI"));

builder.Services.AddApiAuthorization();

builder.Services.AddBlazorDB(options =>
{
    options.Name = "SADnD.IndexedDB";
    options.Version = 1;

    options.StoreSchemas = new List<StoreSchema>()
    {
        new StoreSchema()
        {
            Name = "Character",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            Indexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = $"Character{Globals.LocalTransactionsSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            Indexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = $"Character{Globals.KeysSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            Indexes = new List<string> {"Id"}
        }
    };
});
builder.Services.AddScoped<CampaignManager>();
builder.Services.AddScoped<CharacterManager>();
builder.Services.AddScoped<CharacterSyncManager>();
builder.Services.AddScoped<RaceManager>();
builder.Services.AddScoped<ClassManager>();
builder.Services.AddScoped<JoinRequestManager>();

await builder.Build().RunAsync();
