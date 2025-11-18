using System.Text.Json;
using AIModelOrderingApp.Core.Services;
using AIModelOrderingApp.Core.DTOs;
using AIModelOrderingApp.Models.Entities;
using AIModelOrderingApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

public class Program
{
    public static async Task Main(string[] args)
    {
        // ---------------------------------------------------------
        // Setup Serilog (Console + Rolling File)
        // ---------------------------------------------------------
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(
                "logs/cli-log-.txt",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 10)
            .CreateLogger();

        Log.Information("CLI Application Starting...");

        try
        {
            var builder = Host.CreateDefaultBuilder(args)
                .UseSerilog()  // integrate Serilog with Host
                .ConfigureServices((context, services) =>
                {
                    string connectionString =
                        "Server=localhost;Database=AIModelOrdering;Uid=root;";

                    services.AddScoped<ICustomerService>(
                        _ => new CustomerServiceImpl(connectionString)
                    );
                });

            var host = builder.Build();

            // --------------------------------------------
            // Read CLI arguments
            // --------------------------------------------

            string serviceName = GetArg(args, "--service-name");
            string inputFile = GetArg(args, "--input-file");

            Log.Information("Args: service={Service} file={File}", serviceName, inputFile);

            // MAIN DISPATCH
            if (serviceName.ToLower() == "customer")
            {
                await HandleCustomerCrud(inputFile, host);
            }
            else
            {
                Log.Warning("Unknown service: {Service}", serviceName);
                Console.WriteLine("Unknown service. Use: --service-name customer");
            }

            Log.Information("CLI Application Finished.");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Fatal error occurred in CLI");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    // -----------------------------
    // Helper Functions
    // -----------------------------
    private static string GetArg(string[] args, string name)
    {
        int index = Array.IndexOf(args, name);
        return (index >= 0 && index < args.Length - 1)
            ? args[index + 1]
            : "";
    }

    // -----------------------------
    // CRUD Dispatcher
    // -----------------------------
    private static async Task HandleCustomerCrud(string file, IHost host)
    {
        Log.Information("Handling Customer CRUD with input file: {File}", file);

        if (string.IsNullOrWhiteSpace(file))
        {
            Log.Error("No --input-file argument provided.");
            Console.WriteLine("Please specify --input-file <path>");
            return;
        }

        if (!File.Exists(file))
        {
            Log.Error("Input file not found: {File}", file);
            Console.WriteLine($"Input file not found: {file}");
            return;
        }

        string json = await File.ReadAllTextAsync(file);
        var dtoList = JsonSerializer.Deserialize<List<CustomerCrudDTO>>(json);

        if (dtoList == null || dtoList.Count == 0)
        {
            Log.Warning("JSON file contained no valid operations.");
            Console.WriteLine("No operations found in JSON.");
            return;
        }

        using var scope = host.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ICustomerService>();

        // ----------------------------------------------------
        // Execute operations one by one
        // ----------------------------------------------------
        foreach (var dto in dtoList)
        {
            string op = dto.CRUD_Operation.ToLower();
            Log.Information("Executing operation: {Operation}", op);

            switch (op)
            {
                case "create":
                    var created = await service.CreateCustomerAsync(new Customer
                    {
                        Name = dto.Name!,
                        Email = dto.Email!,
                        Phone = dto.Phone!
                    });

                    Log.Information("Created customer {Id}", created.Id);
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

                    Log.Information("Updated customer {Id}", updated.Id);
                    Console.WriteLine($"[UPDATE] Customer Updated. ID = {updated.Id}");
                    break;

                case "delete":
                    bool deleted = await service.DeleteCustomerAsync(dto.Id!.Value);

                    Log.Information("Delete customer {Id} status={Status}", dto.Id, deleted);
                    Console.WriteLine($"[DELETE] Customer ID {dto.Id} → {(deleted ? "OK" : "FAILED")}");
                    break;

                case "get":
                    var customer = await service.GetCustomerByIdAsync(dto.Id!.Value);

                    Log.Information("Fetched customer: {Customer}", customer);
                    Console.WriteLine("[GET] " + JsonSerializer.Serialize(
                        customer,
                        new JsonSerializerOptions { WriteIndented = true }
                    ));
                    break;

                case "get-all":
                    var all = await service.GetAllCustomersAsync();

                    Log.Information("Fetched {Count} customers", all.Count());
                    Console.WriteLine("[GET-ALL] " + JsonSerializer.Serialize(
                        all,
                        new JsonSerializerOptions { WriteIndented = true }
                    ));
                    break;

                default:
                    Log.Warning("Unknown CRUD operation: {Op}", dto.CRUD_Operation);
                    Console.WriteLine($"Unknown CRUD-Operation: {dto.CRUD_Operation}");
                    break;
            }
        }
    }
}
