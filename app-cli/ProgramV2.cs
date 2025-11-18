using System.Text.Json;
using AIModelOrderingApp.Core.DTOs;
using AIModelOrderingApp.Core.Services;
using AIModelOrderingApp.Models.Entities;
using AIModelOrderingApp.Services;               // CustomerServiceImplV2
using AIModelOrderingApp.Core.Repositories;      // ICustomerRepository
using AIModelOrderingApp.Daos.Repositories;      // CustomerRepository
using AIModelOrderingApp.Daos.Database;          // AppDbContext
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

public class ProgramV2
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Your MySQL Connection String
                string connectionString =
                    "Server=localhost;Database=AIModelOrdering;Uid=root;Pwd=yourpassword;";

                // Register EF Core DbContext
                services.AddDbContext<AppDbContext>(options =>
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

                // Register repository
                services.AddScoped<ICustomerRepository, CustomerRepository>();

                // Register EF Core service layer (V2)
                services.AddScoped<ICustomerService, CustomerServiceImplV2>();
            });

        var host = builder.Build();

        string serviceName = GetArg(args, "--service-name");
        string inputFile   = GetArg(args, "--input-file");

        if (serviceName.ToLower() == "customer")
        {
            await HandleCustomerCrud(inputFile, host);
        }
        else
        {
            Console.WriteLine("Unknown service. Use: --service-name customer");
        }
    }

    // -----------------------------
    // Utility Functions
    // -----------------------------

    private static string GetArg(string[] args, string name)
    {
        int index = Array.IndexOf(args, name);
        return (index >= 0 && index < args.Length - 1)
            ? args[index + 1]
            : "";
    }

    private static async Task HandleCustomerCrud(string file, IHost host)
    {
        if (string.IsNullOrWhiteSpace(file))
        {
            Console.WriteLine("Please specify --input-file <file.json>");
            return;
        }

        if (!File.Exists(file))
        {
            Console.WriteLine($"File not found: {file}");
            return;
        }

        var json = await File.ReadAllTextAsync(file);

        // Deserialize array of CRUD operations
        var dtoList = JsonSerializer.Deserialize<List<CustomerCrudDTO>>(json);

        if (dtoList == null || dtoList.Count == 0)
        {
            Console.WriteLine("No valid JSON operations found.");
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
                    Console.WriteLine($"[CREATE] ID: {created.Id}");
                    break;

                case "update":
                    var updated = await service.UpdateCustomerAsync(new Customer
                    {
                        Id = dto.Id!.Value,
                        Name = dto.Name!,
                        Email = dto.Email!,
                        Phone = dto.Phone!
                    });
                    Console.WriteLine($"[UPDATE] ID: {updated.Id}");
                    break;

                case "delete":
                    bool deleted = await service.DeleteCustomerAsync(dto.Id!.Value);
                    Console.WriteLine($"[DELETE] ID {dto.Id} → {(deleted ? "OK" : "FAILED")}");
                    break;

                case "get":
                    var customer = await service.GetCustomerByIdAsync(dto.Id!.Value);
                    Console.WriteLine("[GET] " + JsonSerializer.Serialize(
                        customer,
                        new JsonSerializerOptions { WriteIndented = true }));
                    break;

                case "get-all":
                    var all = await service.GetAllCustomersAsync();
                    Console.WriteLine("[GET-ALL] " + JsonSerializer.Serialize(
                        all,
                        new JsonSerializerOptions { WriteIndented = true }));
                    break;

                default:
                    Console.WriteLine($"Unknown CRUD-Operation: {dto.CRUD_Operation}");
                    break;
            }
        }
    }
}
