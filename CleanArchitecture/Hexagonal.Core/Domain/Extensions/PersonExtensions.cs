using Hexagonal.Core.Domain.Entities;
using Hexagonal.Core.Domain.Requests;
using Hexagonal.Core.Domain.Responses;

namespace Hexagonal.Core.Domain.Extensions;

public static class PersonExtensions
{
    public static Person MapToEntity(this CreatePersonRequest request)
        => new(request.FirstName, request.LastName);

    public static BasePersonResponse MapToBaseResponse(this Person entity)
        => new(entity.Id, entity.FirstName, entity.LastName);
}
