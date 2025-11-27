# Azure Product Catalog Demo

This solution showcases a small but end-to-end product catalog built on top of .NET 8, Azure Cosmos DB, and a layered/hexagonal architecture. It is split into three projects:

- `Core`: Domain model plus application layer (ports, use cases, DTOs, and extensions).
- `Infrastructure`: Azure integrations (Cosmos DB persistence, Key Vault access, and Blob Storage services plus configuration wrappers).
- `Api`: ASP.NET Core Web API that wires everything together, exposes HTTP endpoints, and hosts Swagger UI.

## Domain Model

The domain centers around `Product`, which includes `Id`, `Category` (used as the Cosmos partition key), `Name`, `Price`, and `Description`. DTOs live under `Core/Domain/Requests` and `Core/Domain/Responses`, while `ProductExtensions` covers mapping logic and update merging.

Catalog assets are modeled via `CatalogImage`, which stores the generated blob name, the public blob URI, and the product category partition key. The associated DTOs (`CatalogImageRequests`, `CatalogImageResponses`) and mapping helpers (`CatalogImageExtensions`) keep file metadata inside the core layer so that controllers only orchestrate the HTTP surface.

## Application Layer

The application layer follows the ports-and-adapters style:

- Driving ports (`Core/Application/Ports/Driving`): use case contracts consumed by the API controllers (products and catalog/storage).
- Driven ports (`Core/Application/Ports/Driven`): abstractions for adapters (Cosmos DB repositories, storage services, etc.) implemented in the infrastructure layer.
- Use case implementations (`Core/Application/UseCases/*UseCases.cs`) coordinate validation, mapping, repository calls, blob uploads, and partition-key-aware updates/deletes. File handling now lives entirely inside `CreateCatalogImageUseCase`, which validates uploaded `IFormFile`s (non-empty payloads, inferred content type) and streams them to the storage adapter before persisting metadata.

## Infrastructure Layer

- `Infrastructure/Persistence/Db/DbContext.cs` encapsulates the Cosmos DB client, initializes the database plus the `Product` container (partitioned by `/Category`), and exposes the `CosmosClient`, `Database`, and `Container`.
- `Infrastructure/Persistence/Repository` hosts the Cosmos-backed repositories (e.g., products, catalog images), each implementing its respective port and handling partition-aware CRUD via the container clients surfaced by `DbContext`.
- `Infrastructure/Configuration/ConfigurationContext.cs` centralizes strongly typed access to the `Azure` section in configuration (Key Vault URI, Storage account URI, container names, and max sizes).
- `Infrastructure/Security/KeyVaultContext.cs` instantiates a `SecretClient` using `DefaultAzureCredential` to retrieve secrets (e.g., the Cosmos connection string) at runtime.
- `Infrastructure/Storage/StorageContext.cs` materializes Azure Blob Storage containers (Catalog and Invoices) using the configuration above, enforces max size metadata, and exposes typed clients to the rest of the app.
- `Infrastructure/Storage/CatalogImageStorageService.cs` implements `ICatalogImageStorageService`, checking max payload size, normalizing HTTP headers, and uploading/deleting blobs through the strongly typed container clients.

## API Layer

`Api/Program.cs` configures:

- Standard ASP.NET Core services + Swagger.
- Singleton infrastructure services: `ConfigurationContext`, `KeyVaultContext`, `StorageContext`, and `DbContext` (the latter bootstraps Cosmos on startup).
- Scoped registrations for repositories, product use cases, and the catalog-image use cases plus storage service adapter.

`Api/Controllers/ProductsController.cs` handles the product CRUD surface, returning `BaseProductResponse` DTOs and relying on request validation to guard inputs.

`Api/Controllers/CatalogImageController.cs` is the multipart/form-data entry point for catalog media. It delegates to the dedicated use cases (`Create`, `ListAll`, `ListById`, `Delete`) so that all blob validation, uploads, and metadata persistence stay inside the application layer.

## Running Locally

1. **Prereqs**: .NET 8 SDK, access to Azure Cosmos DB or the local Cosmos DB Emulator.
2. **Configure Azure services**:
   - `Api/appsettings.json` (or environment-specific overrides) must include an `Azure` section with `KeyVaultUri`, `StorageAccountUri`, and nested `StorageContainers` (`Catalog`, `CatalogMaxKbSize`, `Invoices`, `InvoicesMaxKbSize`). Key Vault secrets (e.g., `CosmosSecret`, `StorageSecret`) need to exist in the configured vault.
3. **Restore & build**:
   ```powershell
   dotnet restore Azure.sln
   dotnet build Azure.sln
   ```
4. **Run the API**:
   ```powershell
   dotnet run --project Api/Api.csproj
   ```
5. **Explore REST surface**: Navigate to `https://localhost:{port}/swagger` to test each endpoint (including the catalog-image upload, which accepts `multipart/form-data` with `file` + `productCategory` form fields).

## Testing & Extensibility

- Cosmos DB access is funneled through `IProductRepository`, so you can swap the backing store (SQL, in-memory, etc.) without touching the domain/application layers.
- New product or storage use cases follow the same pattern: define the driving port, implement it in `ProductUseCases`/`StorageUseCases`, and wire it up in `Program.cs` plus the relevant controller.
- Because the domain and DTOs are in `Core`, you can reuse the same logic across other adapters (e.g., Azure Functions, message processors) without duplicating code.
- Storage uploads and reads are abstracted behind `IStorageService`, so changing the underlying blob provider only requires a new infrastructure adapter.
