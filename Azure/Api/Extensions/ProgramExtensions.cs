using Core.Application.Ports.Driven;
using Core.Application.Ports.Driving;
using Core.Application.UseCases;
using Infrastructure.Configuration;
using Infrastructure.Persistence.Db;
using Infrastructure.Persistence.Repository;
using Infrastructure.Security;
using Infrastructure.Storage;

namespace Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        return builder;
    }

    public static WebApplicationBuilder AddInfrastructureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ConfigurationContext>();
        builder.Services.AddSingleton<KeyVaultContext>();
        builder.Services.AddSingleton<StorageContext>();
        builder.Services.AddSingleton<DbContext>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IStorageService, StorageService>();
        return builder;
    }

    public static WebApplicationBuilder AddApplicationUseCases(this WebApplicationBuilder builder)
    {
        // Product use cases
        builder.Services.AddScoped<ICreateProductUseCase, CreateProductUseCase>();
        builder.Services.AddScoped<IListAllProductsUseCase, ListAllProductsUseCase>();
        builder.Services.AddScoped<IListProductByIdUseCase, ListProductByIdUseCase>();
        builder.Services.AddScoped<IUpdateProductUseCase, UpdateProductUseCase>();
        builder.Services.AddScoped<IDeleteProductUseCase, DeleteProductUseCase>();
        // Catalog use cases
        builder.Services.AddScoped<IUploadCatalogBlobUseCase, UploadCatalogBlobUseCase>();
        builder.Services.AddScoped<IGetAllCatalogBlobsUseCase, GetAllCatalogBlobsUseCase>();
        builder.Services.AddScoped<IDeleteCatalogBlobUseCase, DeleteCatalogBlobUseCase>();
        return builder;
    }
}

public static class WebApplicationExtensions
{
    public static async Task<WebApplication> InitializeCosmosAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
        await dbContext.InitializeAsync();
        return app;
    }

    public static WebApplication UseConfiguredPipeline(this WebApplication app)
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
