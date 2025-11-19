using Onion.Domain.Entities;
using Onion.Domain.Requests;
using Onion.Domain.Responses;

namespace Onion.Application.Interfaces;

public interface IPersonService
{
    Task<BasePersonResponse> CreateAsync(CreatePersonRequest request);
    Task<BasePersonResponse?> ListOneByIdAsync(int id);
    Task<List<BasePersonResponse>> ListAllAsync();
}
