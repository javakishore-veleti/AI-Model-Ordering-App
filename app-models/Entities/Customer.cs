namespace AIModelOrderingApp.Models.Entities;

using System.ComponentModel.DataAnnotations;


public class Customer : BaseEntity {

    [Required, StringLength(80)]
    public string Name { get; set; } = "";

    [Required, EmailAddress, StringLength(120)]
    public string Email { get; set; } = "";

    [StringLength(20)]
    public string Phone { get; set; } = "";
}