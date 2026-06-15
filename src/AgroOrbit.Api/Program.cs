using AgroOrbit.Api.Data;
using AgroOrbit.Api.Middlewares;
using AgroOrbit.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Host=localhost;Port=5432;Database=agroorbit;Username=postgres;Password=postgres";

builder.Services.AddDbContext<AgroOrbitDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<RiskAnalysisService>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Evita erro 500 por referência circular entre entidades do Entity Framework
        // Exemplo: User -> Farms -> User ou Farm -> CropAreas -> Farm.
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AgroOrbit API",
        Version = "v1",
        Description = "API .NET da plataforma AgroOrbit para monitoramento agrícola com dados de satélite, IoT e análise de risco climático."
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

app.UseMiddleware<ApiExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "AgroOrbit API v1");
    options.RoutePrefix = "swagger";
});

app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.Ok(new
{
    app = "AgroOrbit API",
    status = "online",
    swagger = "/swagger",
    health = "/health",
    description = "Monitoramento agrícola com dados orbitais, IoT, alertas e recomendações."
}));

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    service = "agroorbit-api",
    timestamp = DateTime.UtcNow
}));

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AgroOrbitDbContext>();
    db.Database.Migrate();
    SeedData.Insert(db);
}

app.Run();
