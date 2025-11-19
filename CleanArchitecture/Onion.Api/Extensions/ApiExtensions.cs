using Microsoft.EntityFrameworkCore;
using Onion.Application.Interfaces;
using Onion.Application.Services;
using Onion.Infrastructure.Database;
using Onion.Infrastructure.Repositories;

namespace Onion.Api.Extensions;

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
        services.AddDbContext<OnionContext>(options =>
        {
            options.UseInMemoryDatabase("MyInMemoryDb");
        });

        services.AddScoped<IPersonRepository, PersonRepository>();

        services.AddScoped<IPersonService, PersonService>();

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
