using Hexagonal.Core.Application.Ports.Driven;
using Hexagonal.Core.Application.Ports.Driving;
using Hexagonal.Core.Application.UseCases;
using Hexagonal.Infrastructure.Persistence.Database;
using Hexagonal.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Hexagonal.Api.Extensions;

public static class ApiExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<HexagonalContext>(options =>
        {
            options.UseInMemoryDatabase("MyInMemoryDb");
        });

        services.AddScoped<IPersonRepository, PersonRepository>();

        services.AddScoped<ICreatePersonUseCase, CreatePersonUseCase>();
        services.AddScoped<IGetAllPersonsUseCase, GetAllPersonsUseCase>();
        services.AddScoped<IGetPersonByIdUseCase, GetPersonByIdUseCase>();

        return services;
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}