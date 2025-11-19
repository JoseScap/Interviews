using System.ComponentModel.DataAnnotations;

namespace Hexagonal.Core.Domain.Requests;

public class CreatePersonRequest
{
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
}
