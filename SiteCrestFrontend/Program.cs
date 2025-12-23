using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SiteCrestFrontend;
using SiteCrestFrontend.API;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBase = builder.HostEnvironment.IsDevelopment() ? builder.Configuration["Api:DevUrl"] : builder.Configuration["Api:ProdUrl"];

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBase) });
builder.Services.AddScoped<IChemicalApi, ChemicalApi>();
builder.Services.AddScoped<IPathwayApi, PathwayApi>();
builder.Services.AddScoped<IValueApi, ValueApi>();

await builder.Build().RunAsync();
