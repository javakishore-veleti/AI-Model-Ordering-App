namespace AIModelOrderingApp.Models.Entities;

public class Customer : BaseEntity {
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
}