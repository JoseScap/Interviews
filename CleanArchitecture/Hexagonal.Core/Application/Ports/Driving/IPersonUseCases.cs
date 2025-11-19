using Hexagonal.Core.Domain.Requests;
using Hexagonal.Core.Domain.Responses;

namespace Hexagonal.Core.Application.Ports.Driving;

public interface ICreatePersonUseCase
{
    Task<BasePersonResponse> ExecuteAsync(CreatePersonRequest request);
}

public interface IGetAllPersonsUseCase
{
    Task<List<BasePersonResponse>> ExecuteAsync();
}

public interface IGetPersonByIdUseCase
{
    Task<BasePersonResponse?> ExecuteAsync(int id);
}
