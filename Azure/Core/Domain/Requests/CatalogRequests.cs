using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Requests;

public class UploadCatalogImageRequest
{
    [Required]
    public IFormFile File { get; set; } = null!;
}

