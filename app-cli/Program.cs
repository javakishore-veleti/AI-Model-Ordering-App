using System.Text.Json;
using AIModelOrderingApp.Core.Services;
using AIModelOrderingApp.Core.DTOs;
using AIModelOrderingApp.Models.Entities;
using AIModelOrderingApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // TODO: move connection string into config later

        string connectionString =
            "Server=localhost;Database=AIModelOrdering;Uid=root;";

        services.AddScoped<ICustomerService>(
            _ => new CustomerServiceImpl(connectionString)
        );
    });

var host = builder.Build();

// Read CLI arguments
string serviceName = GetArg("--service-name");
string inputFile = GetArg("--input-file");

// MAIN DISPATCH
if (serviceName.ToLower() == "customer")
{
    await HandleCustomerCrud(inputFile, host);
}
else
{
    Console.WriteLine("Unknown service. Use: --service-name customer");
}

// -----------------------------
// Helper Functions
// -----------------------------

static string GetArg(string name)
{
    var args = Environment.GetCommandLineArgs();
    int index = Array.IndexOf(args, name);
    return (index >= 0 && index < args.Length - 1) ? args[index + 1] : "";
}

static async Task HandleCustomerCrud(string file, IHost host)
{
    if (string.IsNullOrWhiteSpace(file))
    {
        Console.WriteLine("Please specify --input-file <path>");
        return;
    }

    if (!File.Exists(file))
    {
        Console.WriteLine($"Input file not found: {file}");
        return;
    }

    // Deserialize array of operations
    string json = await File.ReadAllTextAsync(file);
    var dtoList = JsonSerializer.Deserialize<List<CustomerCrudDTO>>(json);

    if (dtoList == null || dtoList.Count == 0)
    {
        Console.WriteLine("No operations found in JSON.");
        return;
    }

    using var scope = host.Services.CreateScope();
    var service = scope.ServiceProvider.GetRequiredService<ICustomerService>();

    foreach (var dto in dtoList)
    {
        string op = dto.CRUD_Operation.ToLower();

        switch (op)
        {
            case "create":
                var created = await service.CreateCustomerAsync(new Customer
                {
                    Name = dto.Name!,
                    Email = dto.Email!,
                    Phone = dto.Phone!
                });
                Console.WriteLine($"[CREATE] Customer Created. ID = {created.Id}");
                break;

            case "update":
                var updated = await service.UpdateCustomerAsync(new Customer
                {
                    Id = dto.Id!.Value,
                    Name = dto.Name!,
                    Email = dto.Email!,
                    Phone = dto.Phone!
                });
                Console.WriteLine($"[UPDATE] Customer Updated. ID = {updated.Id}");
                break;

            case "delete":
                bool deleted = await service.DeleteCustomerAsync(dto.Id!.Value);
                Console.WriteLine($"[DELETE] Customer ID {dto.Id} → {(deleted ? "OK" : "FAILED")}");
                break;

            case "get":
                var customer = await service.GetCustomerByIdAsync(dto.Id!.Value);
                Console.WriteLine("[GET] " + JsonSerializer.Serialize(
                    customer,
                    new JsonSerializerOptions { WriteIndented = true }
                ));
                break;

            case "get-all":
                var all = await service.GetAllCustomersAsync();
                Console.WriteLine("[GET-ALL] " + JsonSerializer.Serialize(
                    all,
                    new JsonSerializerOptions { WriteIndented = true }
                ));
                break;

            default:
                Console.WriteLine($"Unknown CRUD-Operation: {dto.CRUD_Operation}");
                break;
        }
    }
}
