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
builder.Services.AddCascadingAuthenticationState();

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
            Name = "NoteStory",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = $"NoteStory{Globals.LocalTransactionsSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = $"NoteStory{Globals.KeysSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },new StoreSchema()
        {
            Name = "NotePerson",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = $"NotePerson{Globals.LocalTransactionsSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = $"NotePerson{Globals.KeysSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },new StoreSchema()
        {
            Name = "NoteLocation",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = $"NoteLocation{Globals.LocalTransactionsSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = $"NoteLocation{Globals.KeysSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },new StoreSchema()
        {
            Name = "NoteQuest",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = $"NoteQuest{Globals.LocalTransactionsSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = $"NoteQuest{Globals.KeysSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },new StoreSchema()
        {
            Name = "NoteHint",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = $"NoteHint{Globals.LocalTransactionsSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = $"NoteHint{Globals.KeysSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = "Appointment",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = $"Appointment{Globals.LocalTransactionsSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        },
        new StoreSchema()
        {
            Name = $"Appointment{Globals.KeysSuffix}",
            PrimaryKey = "Id",
            PrimaryKeyAuto = true,
            UniqueIndexes = new List<string> {"Id"}
        }
    };
});
builder.Services.AddScoped<APIRepository<Campaign>>();
builder.Services.AddScoped<IndexedDBRepository<Campaign>>();
builder.Services.AddScoped<SyncRepository<Campaign>>();

builder.Services.AddScoped<APIRepository<Character>>();
builder.Services.AddScoped<IndexedDBRepository<Character>>();
builder.Services.AddScoped<SyncRepository<Character>>();

builder.Services.AddScoped<APIRepository<Class>>();
builder.Services.AddScoped<IndexedDBRepository<Class>>();
builder.Services.AddScoped<SyncRepository<Class>>();

builder.Services.AddScoped<APIRepository<Race>>();
builder.Services.AddScoped<IndexedDBRepository<Race>>();
builder.Services.AddScoped<SyncRepository<Race>>();

builder.Services.AddScoped<APIRepository<JoinRequest>>();
builder.Services.AddScoped<IndexedDBRepository<JoinRequest>>();
builder.Services.AddScoped<SyncRepository<JoinRequest>>();

builder.Services.AddScoped<APIRepository<InventoryItem>>();
builder.Services.AddScoped<IndexedDBRepository<InventoryItem>>();
builder.Services.AddScoped<SyncRepository<InventoryItem>>();

builder.Services.AddScoped<IndexedDBRepository<NoteStory>>();
builder.Services.AddScoped<IndexedDBRepository<NotePerson>>();
builder.Services.AddScoped<IndexedDBRepository<NoteLocation>>();
builder.Services.AddScoped<IndexedDBRepository<NoteQuest>>();
builder.Services.AddScoped<IndexedDBRepository<NoteHint>>();
builder.Services.AddScoped<NoteApiManager>();
builder.Services.AddScoped<NoteSyncManager>();

builder.Services.AddScoped<APIRepository<Appointment>>();
builder.Services.AddScoped<IndexedDBRepository<Appointment>>();
builder.Services.AddScoped<SyncRepository<Appointment>>();

await builder.Build().RunAsync();
