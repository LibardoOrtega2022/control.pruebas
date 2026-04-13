using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Frontend;
using Frontend.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Usar HttpClient sin BaseAddress para permitir URLs absolutas
builder.Services.AddScoped(sp => new HttpClient());

// Registrar servicios HTTP
builder.Services.AddScoped<AuthorService>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<LoanService>();
builder.Services.AddScoped<ReportService>();

await builder.Build().RunAsync();
