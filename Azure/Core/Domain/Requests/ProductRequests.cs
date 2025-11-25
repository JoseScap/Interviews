using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Requests;

public class CreateProductRequest
{
    [Required]
    [MaxLength(32)]
    public string Category { get; set; } = string.Empty;
    [Required]
    [MaxLength(64)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [Range(0, double.MaxValue)]
    public double Price { get; set; }
    [Required]
    [MaxLength(1024)]
    public string Description { get; set; } = string.Empty;
}

public class UpdateProductRequest
{
    [MaxLength(32)]
    public string? Category { get; set; } = string.Empty;
    [MaxLength(64)]
    public string? Name { get; set; } = string.Empty;
    [Range(0, double.MaxValue)]
    public double? Price { get; set; }
    [MaxLength(1024)]
    public string? Description { get; set; } = string.Empty;
}
