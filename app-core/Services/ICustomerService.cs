using AIModelOrderingApp.Models.Entities;

namespace AIModelOrderingApp.Core.Services;

public interface ICustomerService
{
    Task<Customer> CreateCustomerAsync(Customer customer);
    Task<Customer?> GetCustomerByIdAsync(long id);
    Task<IEnumerable<Customer>> GetAllCustomersAsync();
    Task<Customer> UpdateCustomerAsync(Customer customer);
    Task<bool> DeleteCustomerAsync(long id);
}
