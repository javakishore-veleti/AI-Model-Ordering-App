namespace AIModelOrderingApp.Models.Entities;

public class Order : BaseEntity
{
    public long CustomerId { get; set; }

    public string? OrderStatus { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    
}
