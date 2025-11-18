using AIModelOrderingApp.Core.Services;
using AIModelOrderingApp.Core.Repositories;
using AIModelOrderingApp.Models.Entities;
using Serilog;

namespace AIModelOrderingApp.Services;

public class CustomerServiceImplV2 : ICustomerService
{
    private readonly ICustomerRepository _repo;

    public CustomerServiceImplV2(ICustomerRepository repo)
    {
        _repo = repo;
    }

    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        Log.Information("Creating customer: {@Customer}", customer);

        try
        {
            var created = await _repo.AddAsync(customer);
            Log.Information("Customer created with ID {Id}", created.Id);
            return created;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to create customer: {@Customer}", customer);
            throw;
        }
    }

    public async Task<Customer?> GetCustomerByIdAsync(long id)
    {
        Log.Information("Fetching customer with ID {Id}", id);

        try
        {
            var customer = await _repo.GetByIdAsync(id);

            if (customer == null)
                Log.Warning("Customer with ID {Id} not found", id);
            else
                Log.Information("Customer retrieved: {@Customer}", customer);

            return customer;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to fetch customer with ID {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        Log.Information("Fetching all customers");

        try
        {
            var list = await _repo.GetAllAsync();
            Log.Information("Retrieved {Count} customers", list.Count());
            return list;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to fetch customer list");
            throw;
        }
    }

    public async Task<Customer> UpdateCustomerAsync(Customer customer)
    {
        Log.Information("Updating customer: {@Customer}", customer);

        try
        {
            var updated = await _repo.UpdateAsync(customer);
            Log.Information("Customer updated: {@Updated}", updated);
            return updated;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to update customer: {@Customer}", customer);
            throw;
        }
    }

    public async Task<bool> DeleteCustomerAsync(long id)
    {
        Log.Information("Deleting customer with ID {Id}", id);

        try
        {
            var result = await _repo.DeleteAsync(id);

            if (result)
                Log.Information("Customer with ID {Id} successfully deleted", id);
            else
                Log.Warning("Customer with ID {Id} delete FAILED (not found?)", id);

            return result;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to delete customer with ID {Id}", id);
            throw;
        }
    }
}
