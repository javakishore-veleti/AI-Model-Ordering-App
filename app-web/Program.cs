using AIModelOrderingApp.Core.Services;
using AIModelOrderingApp.Services;
using AIModelOrderingApp.Core.Repositories;
using AIModelOrderingApp.Daos.Repositories;
using AIModelOrderingApp.Daos.Database;
using AIModelOrderingApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// Configure Services
// -----------------------------

string connectionString =
    "Server=localhost;Database=AIModelOrdering;Uid=root;";

// EF Core DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Repository + Service
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerServiceImplV2>();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AI Model Ordering API",
        Version = "v1"
    });
});

// Build the application
var app = builder.Build();

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI();

// -----------------------------
// Customer API Endpoints
// -----------------------------

app.MapGet("/", () => "AI Model Ordering Web API is running!");

// GET ALL --------------------------------------
app.MapGet("/api/customers", async (ICustomerService service) =>
{
    var customers = await service.GetAllCustomersAsync();
    return Results.Ok(customers);
});

// GET BY ID --------------------------------------
app.MapGet("/api/customers/{id:long}", async (long id, ICustomerService service) =>
{
    var customer = await service.GetCustomerByIdAsync(id);
    return customer is null ? Results.NotFound() : Results.Ok(customer);
});

// CREATE --------------------------------------
app.MapPost("/api/customers", async (Customer customer, ICustomerService service) =>
{
    var created = await service.CreateCustomerAsync(customer);
    return Results.Created($"/api/customers/{created.Id}", created);
});

// UPDATE --------------------------------------
app.MapPut("/api/customers/{id:long}", async (long id, Customer updatedCustomer, ICustomerService service) =>
{
    updatedCustomer.Id = id;
    var updated = await service.UpdateCustomerAsync(updatedCustomer);
    return Results.Ok(updated);
});

// DELETE --------------------------------------
app.MapDelete("/api/customers/{id:long}", async (long id, ICustomerService service) =>
{
    bool deleted = await service.DeleteCustomerAsync(id);
    return deleted ? Results.Ok() : Results.NotFound();
});

// -----------------------------
// Run App
// -----------------------------
app.Run();
