using Onion.Domain.Entities;
using Onion.Domain.Requests;
using Onion.Domain.Responses;

namespace Onion.Domain.Extensions;

public static class PersonExtensions
{
    public static Person MapToEntity(this CreatePersonRequest request)
        => new(request.FirstName, request.LastName);

    public static BasePersonResponse MapToBaseResponse(this Person entity)
        => new(entity.Id, entity.FirstName, entity.LastName);
}
