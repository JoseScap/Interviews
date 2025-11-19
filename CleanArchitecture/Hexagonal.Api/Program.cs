using Hexagonal.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline
app.ConfigurePipeline();

app.Run();
