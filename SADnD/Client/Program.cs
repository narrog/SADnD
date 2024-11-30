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
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = $"Character{Globals.LocalTransactionsSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = $"Character{Globals.KeysSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = "Campaign",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> { "Id" }
        },
        new StoreSchema()
        {
            Name = $"Campaign{Globals.LocalTransactionsSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> { "Id" }
        },
        new StoreSchema()
        {
            Name = $"Campaign{Globals.KeysSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> { "Id" }
        },
        new StoreSchema()
        {
            Name = "JoinRequest",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> { "Id" }
        },
        new StoreSchema()
        {
            Name = $"JoinRequest{Globals.LocalTransactionsSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> { "Id" }
        },
        new StoreSchema()
        {
            Name = $"JoinRequest{Globals.KeysSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> { "Id" }
        },
        new StoreSchema()
        {
            Name = "Class",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = $"Class{Globals.LocalTransactionsSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = $"Class{Globals.KeysSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = "Race",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = $"Race{Globals.LocalTransactionsSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = $"Race{Globals.KeysSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = "InventoryItem",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = $"InventoryItem{Globals.LocalTransactionsSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = $"InventoryItem{Globals.KeysSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = "Note",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = $"Note{Globals.LocalTransactionsSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = $"Note{Globals.KeysSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        }
    };
});
builder.Services.AddScoped<CampaignApiManager>();
//builder.Services.AddScoped<CampaignSyncManager>();
builder.Services.AddScoped<CharacterApiManager>();
//builder.Services.AddScoped<CharacterSyncManager>();
builder.Services.AddScoped<ClassApiManager>();
//builder.Services.AddScoped<ClassSyncManager>();
builder.Services.AddScoped<RaceApiManager>();
//builder.Services.AddScoped<RaceSyncManager>();
builder.Services.AddScoped<JoinRequestApiManager>();
//builder.Services.AddScoped<JoinRequestSyncManager>();
builder.Services.AddScoped<InventoryItemApiManager>();
//builder.Services.AddScoped<InventoryItemSyncManager>();
builder.Services.AddScoped<NoteApiManager>();
//builder.Services.AddScoped<NoteSyncManager>();

await builder.Build().RunAsync();
