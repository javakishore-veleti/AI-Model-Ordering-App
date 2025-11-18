using AIModelOrderingApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIModelOrderingApp.Daos.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
}
