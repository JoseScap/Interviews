# Hexagonal.Test

## Overview

`Hexagonal.Test` is the **test project** for the Hexagonal Architecture solution. It contains unit tests, integration tests, and test utilities to ensure the correctness and reliability of the application.

## Purpose

This project contains:
- **Core Tests**: Unit tests for use cases and business logic (e.g., `PersonUseCasesTests`)
- **Infrastructure Tests**: Integration tests for repositories and data access (e.g., `PersonRepositoryTests`)
- **Test Utilities**: Helpers, mocks, and test data builders

## Project Relationships

### Dependencies

This project depends on:

1. **Hexagonal.Core** - Tests use cases, domain entities, and business logic
2. **Hexagonal.Infrastructure** - Tests repository implementations and data access

### Dependents

This project has **NO dependents**. It is a test project and is not referenced by other projects.

### Architecture Position

In Hexagonal Architecture (Ports and Adapters):

```
    ┌──────────────┐
    │  Hexagonal.Api│  ← Primary Adapter
    └──────┬───────┘
           │
    ┌──────▼──────────────────┐
    │   Hexagonal.Core        │
    │  (Ports & Use Cases)    │
    └──────┬──────────────────┘
           │
    ┌──────▼──────────────┐
    │ Hexagonal.Infrastructure│
    │   (Driven Adapter)   │
    └─────────────────────┘
           ▲
           │
    ┌──────┴──────┐
    │Hexagonal.Test│  ← Test Project (This Project)
    └─────────────┘
```

- **Test Layer**: Tests all layers of the Hexagonal Architecture
- **Depends on**: Core (for use cases and domain) and Infrastructure (for repository implementations)
- **No dependents**: Not referenced by other projects

## Key Responsibilities

- Unit test use cases and business logic
- Integration test repository implementations
- Test domain entity behavior and validations
- Verify port implementations (driving and driven)
- Ensure code quality and maintainability through automated testing

## Testing Strategy

### Unit Tests

- **Use Case Tests**: Test business logic in isolation
  - Mock driven ports (repositories)
  - Verify use case behavior and business rules
  - Example: `PersonUseCasesTests`

### Integration Tests

- **Repository Tests**: Test data access operations
  - Use in-memory database for testing
  - Verify CRUD operations
  - Test persistence and retrieval
  - Example: `PersonRepositoryTests`

### Domain Tests

- Test entity behavior and validations
- Test business rules and domain logic
- Test extension methods and domain utilities

## Hexagonal Architecture Testing

- **Port Testing**: Verify that adapters correctly implement ports
- **Use Case Testing**: Test business logic independently of adapters
- **Adapter Testing**: Test that adapters correctly convert between external and internal representations

## Technology Stack

- .NET 8.0
- xUnit - Testing framework
- Microsoft.NET.Test.Sdk - Test SDK
- Entity Framework Core InMemory - For integration testing
- coverlet.collector - Code coverage collection

## Running Tests

```bash
dotnet test
```

## Test Organization

Tests are organized to mirror the structure of the projects they test:
- `Core/` - Tests for use cases and domain logic
- `Infrastructure/` - Tests for repositories and data access

## Testing Ports and Adapters

- **Driving Port Tests**: Verify use case interfaces work correctly
- **Driven Port Tests**: Verify repository interfaces are implemented correctly
- **Adapter Tests**: Verify adapters correctly implement their respective ports

