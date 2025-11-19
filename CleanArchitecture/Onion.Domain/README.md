# Onion.Domain

## Overview

`Onion.Domain` is the **core domain layer** and the heart of the application. It contains the business entities, value objects, domain logic, and data transfer objects (DTOs) that represent the core business concepts.

## Purpose

This project contains:
- **Entities**: Core business entities (e.g., `Person`, `BaseEntity`)
- **Requests**: Input DTOs for operations (e.g., `CreatePersonRequest`)
- **Responses**: Output DTOs for operations (e.g., `BasePersonResponse`)
- **Extensions**: Domain-specific extension methods

## Project Relationships

### Dependencies

This project has **NO dependencies** on other projects. It is the innermost layer and represents pure business logic without any external concerns.

### Dependents

This project is used by **ALL other projects**:

1. **Onion.Application** - Uses entities, requests, responses, and extensions to implement business logic
2. **Onion.Infrastructure** - Uses entities for data persistence and mapping
3. **Onion.Api** - Uses requests and responses for API contracts
4. **Onion.Test** - Uses entities and DTOs for test data

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
│  Infrastructure │
└──────┬──────────┘
       │
┌──────▼──────┐
│   Domain    │  ← Core Layer (This Project)
└─────────────┘
```

- **Innermost Layer**: The core of the onion architecture
- **No Dependencies**: Completely independent of other layers
- **Used by All**: Every other layer depends on this project

## Key Responsibilities

- Define core business entities and their properties
- Represent business concepts and rules
- Provide data transfer objects (requests/responses)
- Contain domain-specific logic and extensions
- Serve as the single source of truth for business concepts

## Design Principles

- **Independence**: No dependencies on external libraries or other projects
- **Persistence Ignorance**: Entities are not tied to any specific persistence technology
- **Rich Domain Model**: Contains business logic and behavior, not just data
- **Single Source of Truth**: All layers reference the same domain entities

