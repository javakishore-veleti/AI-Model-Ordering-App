using AIModelOrderingApp.Core.Repositories;
using AIModelOrderingApp.Models.Entities;
using AIModelOrderingApp.Daos.Database;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace AIModelOrderingApp.Daos.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _db;

    public CustomerRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Customer> AddAsync(Customer customer)
    {
        Log.Information("Repository: Adding customer {@Customer}", customer);

        try
        {
            await _db.Customers.AddAsync(customer);
            await _db.SaveChangesAsync();

            Log.Information("Repository: Customer added with ID {Id}", customer.Id);
            return customer;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Repository: Failed to add customer {@Customer}", customer);
            throw;
        }
    }

    public async Task<Customer?> GetByIdAsync(long id)
    {
        Log.Information("Repository: Fetching customer with ID {Id}", id);

        try
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
                Log.Warning("Repository: Customer with ID {Id} not found", id);
            else
                Log.Information("Repository: Retrieved customer {@Customer}", customer);

            return customer;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Repository: Failed to fetch customer ID {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        Log.Information("Repository: Fetching all customers");

        try
        {
            var list = await _db.Customers.ToListAsync();

            Log.Information("Repository: Retrieved {Count} customers", list.Count);
            return list;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Repository: Failed to retrieve all customers");
            throw;
        }
    }

    public async Task<Customer> UpdateAsync(Customer customer)
    {
        Log.Information("Repository: Updating customer {@Customer}", customer);

        try
        {
            _db.Customers.Update(customer);
            await _db.SaveChangesAsync();

            Log.Information("Repository: Customer updated successfully {@Customer}", customer);
            return customer;
        }
        catch (Exception ex)
        {
            Log.Error(ex, 
                "Repository: Failed to update customer ID {Id}. Data: {@Customer}", 
                customer.Id, customer);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(long id)
    {
        Log.Information("Repository: Deleting customer with ID {Id}", id);

        try
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
            {
                Log.Warning("Repository: Cannot delete, customer ID {Id} not found", id);
                return false;
            }

            _db.Customers.Remove(customer);
            await _db.SaveChangesAsync();

            Log.Information("Repository: Customer ID {Id} deleted", id);
            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Repository: Failed to delete customer ID {Id}", id);
            throw;
        }
    }
}
