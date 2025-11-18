namespace AIModelOrderingApp.Models.Entities;

public class CustomerModelMap : BaseEntity {

    public long CustomerId {get; set; }

    public long ModelId { get; set; }
}