# Hexagonal.Core

## Overview

`Hexagonal.Core` is the **core domain** of the application following the Hexagonal Architecture (Ports and Adapters) pattern. It contains the business logic, domain entities, use cases, and defines the ports (interfaces) that adapters must implement.

## Purpose

This project contains:
- **Domain/Entities**: Core business entities (e.g., `Person`, `BaseEntity`)
- **Domain/Requests**: Input DTOs for use cases (e.g., `CreatePersonRequest`)
- **Domain/Responses**: Output DTOs for use cases (e.g., `BasePersonResponse`)
- **Domain/Extensions**: Domain-specific extension methods
- **Application/UseCases**: Business logic implementations (e.g., `CreatePersonUseCase`, `GetPersonByIdUseCase`)
- **Application/Ports/Driving**: Interfaces for use cases (what the application can do)
- **Application/Ports/Driven**: Interfaces for repositories (what the application needs)

## Project Relationships

### Dependencies

This project has **NO dependencies** on other projects. It is the core of the Hexagonal Architecture and represents pure business logic without any external concerns.

### Dependents

This project is used by **ALL other projects**:

1. **Hexagonal.Api** - Implements driving ports (use case interfaces) and uses domain DTOs
2. **Hexagonal.Infrastructure** - Implements driven ports (repository interfaces) and uses domain entities
3. **Hexagonal.Test** - Tests use cases and domain logic

### Architecture Position

In Hexagonal Architecture (Ports and Adapters):

```
    ┌──────────────┐
    │  Hexagonal.Api│  ← Primary Adapter
    └──────┬───────┘
           │
    ┌──────▼──────────────────┐
    │   Hexagonal.Core        │  ← Core (This Project)
    │  (Ports & Use Cases)    │
    └──────┬──────────────────┘
           │
    ┌──────▼──────────────┐
    │ Hexagonal.Infrastructure│
    │   (Driven Adapter)   │
    └─────────────────────┘
```

- **Core Layer**: The center of the hexagon, completely independent
- **No Dependencies**: Completely independent of external libraries or other projects
- **Defines Ports**: Both driving ports (use cases) and driven ports (repositories)
- **Used by All**: All adapters depend on this core

## Key Responsibilities

- Define core business entities and domain logic
- Implement use cases (application business logic)
- Define driving ports (use case interfaces) for primary adapters
- Define driven ports (repository interfaces) for secondary adapters
- Provide data transfer objects (requests/responses)
- Enforce business rules and validations

## Hexagonal Architecture Concepts

### Ports

1. **Driving Ports** (`Application/Ports/Driving`):
   - Interfaces that define what the application can do
   - Implemented by primary adapters (e.g., API, CLI, gRPC)
   - Example: `ICreatePersonUseCase`, `IGetPersonByIdUseCase`

2. **Driven Ports** (`Application/Ports/Driven`):
   - Interfaces that define what the application needs
   - Implemented by secondary adapters (e.g., Database, File System, External APIs)
   - Example: `IPersonRepository`

### Use Cases

- Encapsulate business logic and orchestrate operations
- Depend on driven ports (repositories) via dependency injection
- Are invoked by primary adapters through driving ports

## Design Principles

- **Independence**: No dependencies on external libraries or other projects
- **Ports and Adapters**: Defines contracts (ports) that adapters implement
- **Dependency Inversion**: Core defines interfaces, adapters implement them
- **Persistence Ignorance**: Entities are not tied to any specific persistence technology
- **Rich Domain Model**: Contains business logic and behavior, not just data

