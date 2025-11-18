using System.Text.Json.Serialization;

namespace AIModelOrderingApp.Core.DTOs;

public class CustomerCrudDTO
{
    public long? Id { get; set; }
    public string? Name { get; set; } 
    public string? Email { get; set; } 
    public string? Phone { get; set; }

    [JsonPropertyName("CRUD-Operation")]
    public string CRUD_Operation { get; set; } = "";
}
