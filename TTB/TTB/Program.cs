using Microsoft.AspNetCore.Components.Authorization;
using TTB.Client.Pages;
using TTB.Components;
using TTB.Services;

var builder = WebApplication.CreateBuilder(args);

var apiUrl = builder.Configuration["ApiUrl"];
if (string.IsNullOrWhiteSpace(apiUrl))
{
    apiUrl = "https://localhost:7095";
}

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<TokenProcessing>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<HttpRequestSender>();
builder.Services.AddScoped<Logger>();


builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(apiUrl);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(TTB.Client._Imports).Assembly);


app.Run();
