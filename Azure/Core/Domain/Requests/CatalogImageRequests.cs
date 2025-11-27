using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Requests;

public class CreateCatalogImageRequest
{
    [Required]
    public IFormFile File { get; set; } = null!;
    [Required]
    [MaxLength(32)]
    public string ProductCategory { get; set; } = string.Empty;
}

