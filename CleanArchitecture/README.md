# Interviews Solution

This solution demonstrates two popular Clean Architecture patterns implemented in .NET 8.0:
- **Onion Architecture** (Layered Architecture)
- **Hexagonal Architecture** (Ports and Adapters)

Both architectures follow the same core principles of Clean Architecture: separation of concerns, dependency inversion, and independence of business logic from external frameworks.

---

## 📚 Table of Contents

- [Onion Architecture](#onion-architecture)
- [Hexagonal Architecture](#hexagonal-architecture)
- [Comparison](#comparison)
- [Solution Structure](#solution-structure)
- [Getting Started](#getting-started)

---

## 🧅 Onion Architecture

### Overview

**Onion Architecture** (also known as **Layered Architecture** or **Clean Architecture**) organizes code into concentric layers, with the domain at the center and dependencies pointing inward. Each layer can only depend on layers closer to the center.

### Architecture Layers

```
┌─────────────────┐
│   Onion.Api     │  ← Presentation Layer
└────────┬────────┘
         │
    ┌────┴────┐
    │         │
┌───▼───┐ ┌──▼──────────┐
│ App   │ │Infrastructure│
└───┬───┘ └──────────────┘
    │
┌───▼────┐
│ Domain │  ← Core (No Dependencies)
└────────┘
```

### Project Structure

1. **Onion.Domain** - Core domain layer
   - Entities, DTOs (Requests/Responses)
   - Domain logic and extensions
   - **No dependencies** on other projects

2. **Onion.Application** - Application/Business logic layer
   - Services and business logic
   - Service and repository interfaces
   - **Depends on**: Domain

3. **Onion.Infrastructure** - Infrastructure/Data access layer
   - Repository implementations
   - Database context (Entity Framework)
   - **Depends on**: Application, Domain

4. **Onion.Api** - Presentation layer
   - Controllers and HTTP handling
   - API configuration
   - **Depends on**: Application, Domain, Infrastructure

5. **Onion.Test** - Test project
   - Unit and integration tests
   - **Depends on**: Application, Domain, Infrastructure

### Key Characteristics

- **Layered Structure**: Clear separation into distinct layers
- **Dependency Direction**: All dependencies point inward toward the domain
- **Domain Independence**: Domain layer has no external dependencies
- **Interface-Based**: Application layer defines interfaces, Infrastructure implements them

### When to Use

- Traditional enterprise applications
- Teams familiar with layered architectures
- Applications with clear separation between business logic and infrastructure
- Projects requiring strict layer boundaries

---

## 🔷 Hexagonal Architecture

### Overview

**Hexagonal Architecture** (also known as **Ports and Adapters**) organizes code around a core domain with ports (interfaces) that adapters implement. The core is isolated from external concerns through well-defined ports.

### Architecture Structure

```
        ┌──────────────┐
        │ Hexagonal.Api│  ← Primary Adapter (Driving Port)
        │  (Driving)   │
        └──────┬───────┘
               │
        ┌──────▼──────────────────┐
        │   Hexagonal.Core        │
        │  (Ports & Use Cases)    │  ← Core (No Dependencies)
        └──────┬──────────────────┘
               │
        ┌──────▼──────────────┐
        │ Hexagonal.Infrastructure│
        │   (Driven Adapter)   │
        └─────────────────────┘
```

### Project Structure

1. **Hexagonal.Core** - Core domain layer
   - Domain entities and DTOs
   - Use cases (business logic)
   - **Ports**:
     - **Driving Ports**: Use case interfaces (what the application can do)
     - **Driven Ports**: Repository interfaces (what the application needs)
   - **No dependencies** on other projects

2. **Hexagonal.Infrastructure** - Secondary adapter
   - Implements driven ports (repository interfaces)
   - Database context and persistence
   - **Depends on**: Core

3. **Hexagonal.Api** - Primary adapter
   - Implements driving ports (use case interfaces)
   - HTTP controllers and API configuration
   - **Depends on**: Core, Infrastructure

4. **Hexagonal.Test** - Test project
   - Unit and integration tests
   - **Depends on**: Core, Infrastructure

### Key Concepts

#### Ports

- **Driving Ports** (Primary/Inbound): Interfaces that define what the application can do
  - Example: `ICreatePersonUseCase`, `IGetPersonByIdUseCase`
  - Implemented by primary adapters (API, CLI, gRPC, etc.)

- **Driven Ports** (Secondary/Outbound): Interfaces that define what the application needs
  - Example: `IPersonRepository`
  - Implemented by secondary adapters (Database, File System, External APIs, etc.)

#### Adapters

- **Primary Adapters**: Implement driving ports, convert external requests to use cases
- **Secondary Adapters**: Implement driven ports, convert domain operations to external systems

### Key Characteristics

- **Port-Based**: Core defines ports (interfaces), adapters implement them
- **Symmetric**: Both input and output are treated as adapters
- **Technology Agnostic**: Core is completely independent of frameworks
- **Flexible**: Easy to swap adapters (e.g., REST API → gRPC, SQL → NoSQL)

### When to Use

- Applications with multiple entry points (API, CLI, gRPC, etc.)
- Systems requiring easy swapping of external dependencies
- Microservices architectures
- Projects where the core business logic must be completely framework-agnostic

---

## ⚖️ Comparison

### Similarities

| Aspect | Both Architectures |
|--------|-------------------|
| **Core Principle** | Business logic is independent of external frameworks |
| **Dependency Direction** | Dependencies point inward toward the core |
| **Domain Independence** | Core domain has no external dependencies |
| **Testability** | Easy to test business logic in isolation |
| **Separation of Concerns** | Clear separation between business and infrastructure |

### Differences

| Aspect | Onion Architecture | Hexagonal Architecture |
|--------|-------------------|----------------------|
| **Structure** | Concentric layers | Core with ports and adapters |
| **Organization** | Layer-based (Domain → Application → Infrastructure → API) | Port-based (Core defines ports, adapters implement) |
| **Interfaces** | Application layer defines interfaces | Core defines both driving and driven ports |
| **Flexibility** | Good for traditional layered apps | Better for multiple entry points and swapping adapters |
| **Complexity** | Simpler, more familiar | More flexible, slightly more complex |
| **Use Cases** | Encapsulated in services | Explicit use case classes with interfaces |

### Dependency Flow

**Onion Architecture:**
```
API → Application → Domain
API → Infrastructure → Application → Domain
```

**Hexagonal Architecture:**
```
API → Core (implements driving ports)
Infrastructure → Core (implements driven ports)
```

---

## 📁 Solution Structure

```
Interviews/
├── Onion.Api/              # Onion Architecture - Presentation Layer
├── Onion.Application/      # Onion Architecture - Business Logic
├── Onion.Domain/           # Onion Architecture - Core Domain
├── Onion.Infrastructure/   # Onion Architecture - Data Access
├── Onion.Test/             # Onion Architecture - Tests
│
├── Hexagonal.Api/          # Hexagonal Architecture - Primary Adapter
├── Hexagonal.Core/         # Hexagonal Architecture - Core with Ports
├── Hexagonal.Infrastructure/ # Hexagonal Architecture - Secondary Adapter
├── Hexagonal.Test/         # Hexagonal Architecture - Tests
│
└── Azure.Api/              # Azure-specific API (not documented)
```

### Project Dependencies

**Onion Architecture:**
- `Onion.Api` → `Onion.Application`, `Onion.Domain`, `Onion.Infrastructure`
- `Onion.Application` → `Onion.Domain`
- `Onion.Infrastructure` → `Onion.Application`, `Onion.Domain`
- `Onion.Test` → `Onion.Application`, `Onion.Domain`, `Onion.Infrastructure`

**Hexagonal Architecture:**
- `Hexagonal.Api` → `Hexagonal.Core`, `Hexagonal.Infrastructure`
- `Hexagonal.Infrastructure` → `Hexagonal.Core`
- `Hexagonal.Test` → `Hexagonal.Core`, `Hexagonal.Infrastructure`

---

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or VS Code / Rider

### Running the Projects

#### Onion Architecture

```bash
cd Onion.Api
dotnet run
```

The API will be available at `https://localhost:5001` (or the port configured in `launchSettings.json`)

#### Hexagonal Architecture

```bash
cd Hexagonal.Api
dotnet run
```

The API will be available at `https://localhost:5001` (or the port configured in `launchSettings.json`)

### Running Tests

```bash
# Onion Architecture Tests
dotnet test Onion.Test

# Hexagonal Architecture Tests
dotnet test Hexagonal.Test
```

### API Endpoints

Both architectures implement the same Person management API:

- `POST /api/people` - Create a new person
- `GET /api/people` - Get all people
- `GET /api/people/{id}` - Get a person by ID

### Swagger Documentation

Both APIs include Swagger/OpenAPI documentation available at:
- `https://localhost:5001/swagger`

---

## 📖 Additional Documentation

Each project contains its own README with detailed information:

### Onion Architecture
- [Onion.Api/README.md](Onion.Api/README.md)
- [Onion.Application/README.md](Onion.Application/README.md)
- [Onion.Domain/README.md](Onion.Domain/README.md)
- [Onion.Infrastructure/README.md](Onion.Infrastructure/README.md)
- [Onion.Test/README.md](Onion.Test/README.md)

### Hexagonal Architecture
- [Hexagonal.Api/README.md](Hexagonal.Api/README.md)
- [Hexagonal.Core/README.md](Hexagonal.Core/README.md)
- [Hexagonal.Infrastructure/README.md](Hexagonal.Infrastructure/README.md)
- [Hexagonal.Test/README.md](Hexagonal.Test/README.md)

---

## 🎯 Choosing Between Architectures

### Choose Onion Architecture When:
- Building traditional enterprise applications
- Your team is familiar with layered architectures
- You have a single primary entry point (e.g., REST API)
- You want a simpler, more straightforward structure
- You prefer explicit layer boundaries

### Choose Hexagonal Architecture When:
- You need multiple entry points (API, CLI, gRPC, etc.)
- You want to easily swap external dependencies
- Building microservices
- You need maximum flexibility for adapters
- The core business logic must be completely framework-agnostic

---

## 📝 Notes

- Both implementations use **Entity Framework Core InMemory** for simplicity
- Both architectures implement the same domain model (Person entity)
- The codebase demonstrates the same functionality using different architectural approaches
- All projects target **.NET 8.0**

---

## 🤝 Contributing

This is a demonstration project showcasing different architectural patterns. Feel free to explore, learn, and adapt these patterns to your own projects.

