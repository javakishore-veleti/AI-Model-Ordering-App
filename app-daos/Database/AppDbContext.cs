using Microsoft.EntityFrameworkCore;
using AIModelOrderingApp.Models.Entities;

namespace AIModelOrderingApp.Daos.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
}
