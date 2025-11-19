# Onion.Application

## Overview

`Onion.Application` is the **application/business logic layer**. It contains the core business logic and orchestrates operations between the domain entities and infrastructure services.

## Purpose

This project contains:
- **Services**: Business logic implementations (e.g., `PersonService`)
- **Interfaces**: Contracts for services and repositories (e.g., `IPersonService`, `IPersonRepository`)

## Project Relationships

### Dependencies

This project depends on:

1. **Onion.Domain** - Uses domain entities, requests, responses, and extension methods to implement business logic

### Dependents

This project is used by:

1. **Onion.Api** - Consumes application services through interfaces (e.g., `IPersonService`)
2. **Onion.Infrastructure** - Implements repository interfaces defined here (e.g., `IPersonRepository`)
3. **Onion.Test** - Unit tests for application services

### Architecture Position

```
┌─────────────┐
│   Onion.Api │
└──────┬──────┘
       │
┌──────▼──────────┐
│ Onion.Application│  ← Application Layer (This Project)
└──────┬──────────┘
       │
┌──────▼──────┐
│   Domain    │
└─────────────┘
```

- **Middle Layer**: Sits between the presentation layer (API) and domain layer
- **Depends on**: Domain (core entities and business rules)
- **Used by**: API (for business operations), Infrastructure (implements its interfaces), Test (for testing)

## Key Responsibilities

- Implement business logic and use cases
- Define service and repository interfaces (dependency inversion)
- Orchestrate operations between domain entities and data access
- Transform between domain entities and DTOs (requests/responses)
- Enforce business rules and validations

## Design Principles

- **Dependency Inversion**: Defines interfaces that infrastructure implements
- **Single Responsibility**: Each service handles a specific business concern
- **Separation of Concerns**: Business logic is independent of presentation and data access

