namespace AIModelOrderingApp.Models.Entities;

public class OrderLine : BaseEntity
{
    public long OrderHeaderId { get; set; }
    public long ModelId { get; set; }
    public int Quantity { get; set; }
}
