using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tech2023.DAL;

public class Subject
{
    [Key]
    [Required]
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}
