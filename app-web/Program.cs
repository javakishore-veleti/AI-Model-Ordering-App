using AIModelOrderingApp.Core.Services;
using AIModelOrderingApp.Services;
using AIModelOrderingApp.Core.Repositories;
using AIModelOrderingApp.Daos.Repositories;
using AIModelOrderingApp.Daos.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

string connectionString =
    "Server=localhost;Database=AIModelOrdering;Uid=root;Pwd=yourpassword;";

// EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Repository + Service
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerServiceImplV2>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "AI Model Ordering API", 
        Version = "v1" 
    });
});

var app = builder.Build();

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AI Model Ordering API V1");
});

app.MapGet("/", () => "AI Model Ordering Web API is running!");

app.Run();
