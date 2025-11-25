# Azure Product Catalog Demo

This solution showcases a small but end-to-end product catalog built on top of .NET 8, Azure Cosmos DB, and a layered/hexagonal architecture. It is split into three projects:

- `Core`: Domain model plus application layer (ports, use cases, DTOs, and extensions).
- `Infrastructure`: Cosmos DB persistence and repository implementation.
- `Api`: ASP.NET Core Web API that wires everything together, exposes HTTP endpoints, and hosts Swagger UI.

## Domain Model

The domain centers around `Product`, which includes `Id`, `Category` (used as the Cosmos partition key), `Name`, `Price`, and `Description`. DTOs live under `Core/Domain/Requests` and `Core/Domain/Responses`, while `ProductExtensions` covers mapping logic and update merging.

## Application Layer

The application layer follows the ports-and-adapters style:

- Driving ports (`Core/Application/Ports/Driving`): use case contracts consumed by the API controller.
- Driven ports (`Core/Application/Ports/Driven`): repository contracts implemented by the infrastructure layer.
- Use case implementations (`Core/Application/UseCases/ProductUseCases.cs`) coordinate validation, mapping, repository calls, and partition-key-aware updates/deletes.

## Infrastructure Layer

`Infrastructure/Persistence/Db/DbContext.cs` encapsulates the Cosmos DB client, initializes the database plus the `Product` container (partitioned by `/Category`), and exposes the `CosmosClient`, `Database`, and `Container`.

`Infrastructure/Persistence/Repository/ProductRepository.cs` implements `IProductRepository` with Cosmos DB SDK queries for CRUD, including helper operations to list by id and handle partition-aware deletes.

## API Layer

`Api/Program.cs` configures:

- Standard ASP.NET Core services + Swagger.
- `DbContext` as a singleton (it internally initializes Cosmos on startup).
- Scoped registrations for repositories and each use case.

`Api/Controllers/ProductsController.cs` exposes the REST surface:

| HTTP Verb | Route            | Description                        |
|-----------|------------------|------------------------------------|
| POST      | `/api/products`  | Create product                     |
| GET       | `/api/products`  | List all products                  |
| GET       | `/api/products/{id}` | Retrieve a product by id     |
| PUT       | `/api/products/{id}` | Update product (handles category changes/partition moves) |
| DELETE    | `/api/products/{id}` | Delete product                |

All responses return the `BaseProductResponse` DTO; validation attributes on request models ensure inputs are sane before the use cases execute.

## Running Locally

1. **Prereqs**: .NET 8 SDK, access to Azure Cosmos DB or the local Cosmos DB Emulator.
2. **Configure Cosmos**: Update `Api/appsettings.json` (or `appsettings.Development.json`/user secrets) with a valid `ConnectionStrings:CosmosDb`. The provided value is an example and not a usable key.
3. **Restore & build**:
   ```powershell
   dotnet restore Azure.sln
   dotnet build Azure.sln
   ```
4. **Run the API**:
   ```powershell
   dotnet run --project Api/Api.csproj
   ```
5. **Explore REST surface**: Navigate to `https://localhost:{port}/swagger` to test each endpoint.

## Testing & Extensibility

- Cosmos DB access is funneled through `IProductRepository`, so you can swap the backing store (SQL, in-memory, etc.) without touching the domain/application layers.
- Adding new product-related use cases involves defining a driving port, implementing it in `ProductUseCases`, and wiring it up in `Program.cs` plus the controller.
- Because the domain and DTOs are in `Core`, you can reuse the same logic across other adapters (e.g., Azure Functions, message processors) without duplicating code.

