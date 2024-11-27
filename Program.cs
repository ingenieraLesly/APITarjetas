using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PrimoAPITarjetas.Data;
using PrimoAPITarjetas.Middleware;
using PrimoAPITarjetas.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

// Load JWT configuration
var jwtSection = builder.Configuration.GetSection("Jwt");
//var jwtKey = jwtSection["Key"]; --> PILAS CON ESTE ERROR
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
if (string.IsNullOrEmpty(jwtKey) || jwtKey.Length < 32)
{
    throw new ArgumentOutOfRangeException("JWT_KEY", "JWT key must be at least 32 characters long.");
}

builder.Services.AddDbContext<CardContext>(options =>
    options.UseSqlServer(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")));


if (string.IsNullOrEmpty(jwtKey) || jwtKey.Length < 32)
{
    throw new ArgumentOutOfRangeException("Jwt:Key", "JWT key must be at least 32 characters long.");
}

// Configure JWT authentication
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = signingKey
        };
    });

builder.Services.AddControllers();

// Configure database context
builder.Services.AddDbContext<CardContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
#pragma warning disable CA1416
builder.Logging.AddEventLog();
#pragma warning restore CA1416

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5000); // Puerto HTTP
    // serverOptions.ListenAnyIP(5001, listenOptions => listenOptions.UseHttps()); // Puerto HTTPS
});

// Registrar FeeService como singleton
builder.Services.AddSingleton<FeeService>();

// Registrar CardService como scoped
builder.Services.AddScoped<CardService>();

// Registrar FeeBackgroundService como Hosted Service
builder.Services.AddHostedService<FeeBackgroundService>();

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CargoPay API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CargoPay API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
