namespace AIModelOrderingApp.Models.Entities;

public class OrderHeader : BaseEntity
{
    public long CustomerId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
}
