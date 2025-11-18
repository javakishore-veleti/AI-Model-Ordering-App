using AIModelOrderingApp.Models.Entities;

namespace AIModelOrderingApp.Core.Repositories;

public interface ICustomerRepository
{
    Task<Customer> AddAsync(Customer customer);
    Task<Customer?> GetByIdAsync(long id);
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer> UpdateAsync(Customer customer);
    Task<bool> DeleteAsync(long id);
}
