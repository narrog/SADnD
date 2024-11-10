using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SADnD.Client;
using SADnD.Client.Services;
using SADnD.Shared.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("SADnD.ServerAPI", client => client.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}api/"))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("SADnD.ServerAPI"));

builder.Services.AddApiAuthorization();

builder.Services.AddScoped<CampaignManager>();
builder.Services.AddScoped<CharacterManager>();
builder.Services.AddScoped<RaceManager>();
builder.Services.AddScoped<ClassManager>();
builder.Services.AddScoped<JoinRequestManager>();
builder.Services.AddScoped<InventoryItemManager>();

await builder.Build().RunAsync();
