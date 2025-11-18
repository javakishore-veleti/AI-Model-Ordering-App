using AIModelOrderingApp.Core.Services;
using AIModelOrderingApp.Core.Repositories;
using AIModelOrderingApp.Models.Entities;

namespace AIModelOrderingApp.Services;

public class CustomerServiceImplV2 : ICustomerService
{
    private readonly ICustomerRepository _repo;

    public CustomerServiceImplV2(ICustomerRepository repo)
    {
        _repo = repo;
    }

    public Task<Customer> CreateCustomerAsync(Customer customer)
        => _repo.AddAsync(customer);

    public Task<Customer?> GetCustomerByIdAsync(long id)
        => _repo.GetByIdAsync(id);

    public Task<IEnumerable<Customer>> GetAllCustomersAsync()
        => _repo.GetAllAsync();

    public Task<Customer> UpdateCustomerAsync(Customer customer)
        => _repo.UpdateAsync(customer);

    public Task<bool> DeleteCustomerAsync(long id)
        => _repo.DeleteAsync(id);
}
