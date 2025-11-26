using Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder
    .AddPresentation()
    .AddInfrastructureServices()
    .AddApplicationUseCases();

var app = builder.Build();
app = await app.InitializeCosmosAsync();
app.UseConfiguredPipeline();
await app.RunAsync();
