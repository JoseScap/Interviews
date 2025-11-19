# Onion.Api

## Overview

`Onion.Api` is the **presentation layer** of the application. It serves as the entry point for HTTP requests and is responsible for handling API endpoints, request/response serialization, and routing.

## Purpose

This project contains:
- **Controllers**: Handle HTTP requests and responses (e.g., `PeopleController`)
- **Extensions**: Configuration and middleware setup (e.g., `ApiExtensions`)
- **Program.cs**: Application entry point and service configuration

## Project Relationships

### Dependencies

This project depends on:

1. **Onion.Application** - References application services and interfaces to orchestrate business logic
2. **Onion.Domain** - Uses domain entities, requests, and responses for data transfer
3. **Onion.Infrastructure** - Registers infrastructure services (repositories, database context) via dependency injection

### Architecture Position

```
┌─────────────────┐
│   Onion.Api     │  ← Presentation Layer (This Project)
└────────┬────────┘
         │
    ┌────┴────┐
    │         │
┌───▼───┐ ┌──▼──────────┐
│ App   │ │Infrastructure│
└───┬───┘ └──────────────┘
    │
┌───▼────┐
│ Domain │
└────────┘
```

- **Top Layer**: This is the outermost layer that receives external requests
- **Depends on**: Application (business logic), Infrastructure (data access), and Domain (core entities)
- **No dependencies on it**: Other projects do not reference this layer

## Key Responsibilities

- Handle HTTP requests and responses
- Validate input data
- Map between HTTP concerns and application concerns
- Configure middleware and dependency injection
- Expose RESTful API endpoints

## Technology Stack

- .NET 8.0
- ASP.NET Core Web API
- Swashbuckle (Swagger/OpenAPI)

