using AIModelOrderingApp.Core.Services;
using AIModelOrderingApp.Models.Entities;
using Dapper;
using MySql.Data.MySqlClient;

namespace AIModelOrderingApp.Services;

public class CustomerServiceImpl : ICustomerService
{
    private readonly string _connectionString;

    public CustomerServiceImpl(string connectionString)
    {
        _connectionString = connectionString;
    }

    private MySqlConnection GetConnection()
        => new MySqlConnection(_connectionString);

    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        using var conn = GetConnection();

        string sql = @"
            INSERT INTO Customers (Name, Email, Phone, CreatedAt)
            VALUES (@Name, @Email, @Phone, @CreatedAt);
            SELECT LAST_INSERT_ID();
        ";

        customer.CreatedAt = DateTime.UtcNow;
        customer.Id = await conn.ExecuteScalarAsync<long>(sql, customer);
        return customer;
    }

    public async Task<Customer?> GetCustomerByIdAsync(long id)
    {
        using var conn = GetConnection();

        string sql = @"SELECT * FROM Customers WHERE Id = @Id;";
        return await conn.QueryFirstOrDefaultAsync<Customer>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        using var conn = GetConnection();

        string sql = @"SELECT * FROM Customers;";
        return await conn.QueryAsync<Customer>(sql);
    }

    public async Task<Customer> UpdateCustomerAsync(Customer customer)
    {
        using var conn = GetConnection();

        customer.UpdatedAt = DateTime.UtcNow;

        string sql = @"
            UPDATE Customers
            SET Name = @Name,
                Email = @Email,
                Phone = @Phone,
                UpdatedAt = @UpdatedAt
            WHERE Id = @Id;
        ";

        await conn.ExecuteAsync(sql, customer);
        return customer;
    }

    public async Task<bool> DeleteCustomerAsync(long id)
    {
        using var conn = GetConnection();

        string sql = @"DELETE FROM Customers WHERE Id = @Id;";
        return await conn.ExecuteAsync(sql, new { Id = id }) > 0;
    }
}
