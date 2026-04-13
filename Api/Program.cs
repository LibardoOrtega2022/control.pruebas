using Core.Domains.Authors;
using Core.Domains.Books;
using Core.Domains.Loans;
using Core.Domains.Reports;
using Infrastructure;
using Api.Services;
using Microsoft.OpenApi;
using Dapper;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configurar Dapper para manejar DateTime nullable correctamente
SqlMapper.AddTypeHandler(new DapperDateTimeNullableHandler());

// Agregar CORS para permitir comunicación Blazor Frontend -> API
// Permitir dos puertos: 5088 (API) y 5089 (Frontend DevServer)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy.WithOrigins("http://localhost:5088", "http://localhost:5089")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Configurar System.Text.Json para manejar DateTime nulos correctamente
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        
        // ⭐ IMPORTANTE: Conversor personalizado para DateTime nulos
        options.JsonSerializerOptions.Converters.Add(new DateTimeNullableConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Biblioteca API",
        Version = "v1"
    });
});

builder.Services.AddInfrastructure(builder.Configuration);

// Registrar Domain Services - Authors
builder.Services.AddScoped<CreateAuthorDomain>();
builder.Services.AddScoped<AuthorListDomain>();
builder.Services.AddScoped<GetAuthorDomain>();
builder.Services.AddScoped<UpdateAuthorDomain>();
builder.Services.AddScoped<DeleteAuthorDomain>();

// Registrar Domain Services - Books
builder.Services.AddScoped<CreateBookDomain>();
builder.Services.AddScoped<GetBooksDomain>();
builder.Services.AddScoped<GetBookDetailDomain>();
builder.Services.AddScoped<UpdateBookDomain>();
builder.Services.AddScoped<DeleteBookDomain>();

// Registrar Domain Services - Loans
builder.Services.AddScoped<CreateLoanDomain>();
builder.Services.AddScoped<GetLoansDomain>();
builder.Services.AddScoped<GetLoanDetailDomain>();
builder.Services.AddScoped<ReturnLoanDomain>();

// Registrar Domain Services - Reports
builder.Services.AddScoped<GenerateLibrarySummaryReportDomain>();

// Registrar servicio de imágenes
builder.Services.AddScoped<IImageService, ImageService>();

var app = builder.Build();

// BUG FIX: Habilitar Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Biblioteca API v1");
        c.RoutePrefix = "swagger";
    });
}

// Habilitar acceso a archivos estáticos (para servir imágenes)
app.UseStaticFiles();

// Usar CORS policy para permitir comunicación con Blazor Frontend
app.UseCors("AllowBlazor");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();