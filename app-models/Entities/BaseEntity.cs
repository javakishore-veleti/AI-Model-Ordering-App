namespace AIModelOrderingApp.Models.Entities;

public abstract class BaseEntity {

    // MySQL auto-increment
    public long Id {get; set; }

    public DateTime CreatedAt { get; set;} = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

}