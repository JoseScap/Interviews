# Onion.Infrastructure

## Overview

`Onion.Infrastructure` is the **infrastructure/data access layer**. It provides implementations for data persistence, external services, and technical concerns that are not part of the core business logic.

## Purpose

This project contains:
- **Repositories**: Data access implementations (e.g., `PersonRepository`)
- **Database**: Database context and configuration (e.g., `OnionContext`)
- **External Service Implementations**: Integrations with third-party services

## Project Relationships

### Dependencies

This project depends on:

1. **Onion.Application** - Implements repository interfaces defined in the application layer (dependency inversion)
2. **Onion.Domain** - Uses domain entities for persistence and mapping

### Dependents

This project is used by:

1. **Onion.Api** - Registers infrastructure services via dependency injection
2. **Onion.Test** - Uses infrastructure implementations for integration testing

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
│ Infrastructure  │  ← Infrastructure Layer (This Project)
└──────┬──────────┘
       │
┌──────▼──────┐
│   Domain    │
└─────────────┘
```

- **Outer Layer**: Implements technical concerns and data access
- **Depends on**: Application (implements its interfaces) and Domain (uses entities)
- **Used by**: API (registers services) and Test (for integration tests)

## Key Responsibilities

- Implement data access operations (CRUD)
- Configure database context and Entity Framework
- Implement repository interfaces from the application layer
- Handle data persistence and retrieval
- Manage database migrations and schema
- Provide implementations for external service integrations

## Design Principles

- **Dependency Inversion**: Implements interfaces defined in the application layer
- **Persistence Implementation**: Handles all database and storage concerns
- **Technology Details**: Encapsulates framework-specific code (e.g., Entity Framework)
- **Testability**: Can be swapped with in-memory implementations for testing

## Technology Stack

- .NET 8.0
- Entity Framework Core 8.0
- Entity Framework Core InMemory (for development/testing)

