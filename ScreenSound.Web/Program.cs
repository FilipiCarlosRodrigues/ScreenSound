using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using ScreenSound.Web;
using ScreenSound.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

//builder.Services.AddTransient<ArtistaAPI>(); //gerencia o ciclo de vida do objeto tipo artistaAPI
//builder.Services.AddTransient<GeneroAPI>();
//builder.Services.AddTransient<MusicaAPI>();

builder.Services.AddScoped<ArtistaAPI>();
builder.Services.AddScoped<MusicaAPI>();
builder.Services.AddScoped<GeneroAPI>();

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["APIServer:Url"]!);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

await builder.Build().RunAsync();
