# Hexagonal.Api

## Overview

`Hexagonal.Api` is the **primary adapter** (driving port adapter) for the application. It serves as the HTTP entry point and adapts incoming HTTP requests to use cases defined in the core domain.

## Purpose

This project contains:
- **Controllers**: HTTP endpoints that adapt requests to use cases (e.g., `PeopleController`)
- **Extensions**: Configuration, dependency injection, and middleware setup (e.g., `ApiExtensions`)
- **Program.cs**: Application entry point and service registration

## Project Relationships

### Dependencies

This project depends on:

1. **Hexagonal.Core** - Uses driving ports (use case interfaces) and domain DTOs (requests/responses)
2. **Hexagonal.Infrastructure** - Registers infrastructure adapters (repository implementations) via dependency injection

### Dependents

This project has **NO dependents**. It is the entry point of the application.

### Architecture Position

In Hexagonal Architecture (Ports and Adapters):

```
        ┌──────────────┐
        │ Hexagonal.Api│  ← Primary Adapter (This Project)
        │  (Driving)   │
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
```

- **Primary Adapter**: Implements the driving ports (use cases) from the core
- **Depends on**: Core (for use case interfaces) and Infrastructure (for repository implementations)
- **No dependencies on it**: Other projects do not reference this layer

## Key Responsibilities

- Adapt HTTP requests to use case invocations
- Handle HTTP routing and request/response serialization
- Configure dependency injection for use cases and repositories
- Set up middleware pipeline (Swagger, HTTPS, authorization)
- Map between HTTP concerns and application use cases

## Hexagonal Architecture Concepts

- **Driving Port**: The API implements the driving ports (use case interfaces) from the core
- **Adapter**: Converts external HTTP protocol to internal use case calls
- **Dependency Direction**: Depends inward toward the core, not the other way around

## Technology Stack

- .NET 8.0
- ASP.NET Core Web API
- Swashbuckle (Swagger/OpenAPI)

