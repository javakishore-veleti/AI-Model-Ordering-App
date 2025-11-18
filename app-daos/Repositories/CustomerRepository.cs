using AIModelOrderingApp.Core.Repositories;
using AIModelOrderingApp.Models.Entities;
using AIModelOrderingApp.Daos.Database;
using Microsoft.EntityFrameworkCore;

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
        customer.CreatedAt = DateTime.UtcNow;
        _db.Customers.Add(customer);
        await _db.SaveChangesAsync();
        return customer;
    }

    public async Task<Customer?> GetByIdAsync(long id)
    {
        return await _db.Customers.FindAsync(id);
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _db.Customers.ToListAsync();
    }

    public async Task<Customer> UpdateAsync(Customer customer)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        _db.Customers.Update(customer);
        await _db.SaveChangesAsync();
        return customer;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _db.Customers.FindAsync(id);
        if (entity == null) return false;

        _db.Customers.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}
