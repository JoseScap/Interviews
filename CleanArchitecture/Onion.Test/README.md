# Onion.Test

## Overview

`Onion.Test` is the **test project** for the solution. It contains unit tests, integration tests, and test utilities to ensure the correctness and reliability of the application.

## Purpose

This project contains:
- **Application Tests**: Unit tests for application services
- **Infrastructure Tests**: Integration tests for repositories and data access
- **Test Utilities**: Helpers, mocks, and test data builders

## Project Relationships

### Dependencies

This project depends on:

1. **Onion.Application** - Tests application services and business logic
2. **Onion.Domain** - Uses domain entities and DTOs for test data
3. **Onion.Infrastructure** - Tests repository implementations and data access

### Dependents

This project has **NO dependents**. It is a test project and is not referenced by other projects.

### Architecture Position

```
┌─────────────┐
│   Onion.Api │
└──────┬──────┘
       │
┌──────▼──────────┐
│  Application    │
└──────┬──────────┘
       │
┌──────▼──────────┐
│ Infrastructure  │
└──────┬──────────┘
       │
┌──────▼──────┐
│   Domain    │
└─────────────┘
       ▲
       │
┌──────┴──────┐
│ Onion.Test  │  ← Test Project (This Project)
└─────────────┘
```

- **Test Layer**: Tests all other layers of the application
- **Depends on**: Application, Domain, and Infrastructure (for testing)
- **No dependents**: Not referenced by other projects

## Key Responsibilities

- Unit test application services and business logic
- Integration test repository implementations
- Test domain entity behavior and validations
- Verify API endpoints (if integration tests are included)
- Ensure code quality and maintainability through automated testing

## Testing Strategy

- **Unit Tests**: Test individual components in isolation
- **Integration Tests**: Test interactions between layers
- **Domain Tests**: Test business rules and entity behavior
- **Repository Tests**: Test data access operations

## Technology Stack

- .NET 8.0
- xUnit - Testing framework
- Microsoft.NET.Test.Sdk - Test SDK
- coverlet.collector - Code coverage collection

## Running Tests

```bash
dotnet test
```

## Test Organization

Tests are typically organized to mirror the structure of the projects they test:
- `Application/` - Tests for application services
- `Infrastructure/` - Tests for repositories and data access

