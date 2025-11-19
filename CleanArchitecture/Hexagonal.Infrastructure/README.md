# Hexagonal.Infrastructure

## Overview

`Hexagonal.Infrastructure` is the **secondary adapter** (driven port adapter) for the application. It implements the driven ports (repository interfaces) defined in the core domain, providing data persistence and external service integrations.

## Purpose

This project contains:
- **Persistence/Repository**: Repository implementations (e.g., `PersonRepository`)
- **Persistence/Database**: Database context and configuration (e.g., `HexagonalContext`)

## Project Relationships

### Dependencies

This project depends on:

1. **Hexagonal.Core** - Implements driven ports (repository interfaces) and uses domain entities

### Dependents

This project is used by:

1. **Hexagonal.Api** - Registers infrastructure services via dependency injection
2. **Hexagonal.Test** - Uses infrastructure implementations for integration testing

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
    │ Hexagonal.Infrastructure│  ← Secondary Adapter (This Project)
    │   (Driven Adapter)   │
    └─────────────────────┘
```

- **Secondary Adapter**: Implements the driven ports (repository interfaces) from the core
- **Depends on**: Core (implements its interfaces and uses entities)
- **Used by**: API (registers services) and Test (for integration tests)

## Key Responsibilities

- Implement driven ports (repository interfaces) from the core
- Provide data access operations (CRUD)
- Configure database context and Entity Framework
- Handle data persistence and retrieval
- Manage database migrations and schema
- Implement external service integrations (if needed)

## Hexagonal Architecture Concepts

- **Driven Port**: Implements repository interfaces defined in the core
- **Adapter**: Converts between domain entities and persistence technology
- **Dependency Direction**: Depends inward toward the core, not the other way around
- **Technology Encapsulation**: Hides framework-specific code (e.g., Entity Framework)

## Design Principles

- **Dependency Inversion**: Implements interfaces defined in the core domain
- **Persistence Implementation**: Handles all database and storage concerns
- **Technology Details**: Encapsulates framework-specific code (e.g., Entity Framework)
- **Testability**: Can be swapped with in-memory or mock implementations for testing
- **Adapter Pattern**: Adapts external systems to the core's interfaces

## Technology Stack

- .NET 8.0
- Entity Framework Core 8.0
- Entity Framework Core InMemory (for development/testing)

## Port Implementation

This project implements the **driven ports** defined in `Hexagonal.Core`:

- `IPersonRepository` → `PersonRepository`
  - Provides data access operations for Person entities
  - Uses Entity Framework Core for persistence
  - Maps between domain entities and database models

