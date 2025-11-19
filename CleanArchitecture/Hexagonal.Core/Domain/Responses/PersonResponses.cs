namespace Hexagonal.Core.Domain.Responses;

public class BasePersonResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public BasePersonResponse()
    {
    }

    public BasePersonResponse(int id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }
}
