## Interviews Monorepo

This repository holds several reference applications grouped by architectural style. Each solution lives in its own folder so you can open and build them independently while still sharing a single Git history.

### CleanArchitecture
- Location: `CleanArchitecture/`
- Solutions: `CleanArchitecture.sln` exposing `Hexagonal.*` and `Onion.*` projects.
- Purpose: demonstrates Hexagonal and Onion architecture variants, including API, Application, Domain, Infrastructure, and Test layers for each approach.

### Azure
- Location: `Azure/`
- Solution: `Azure.sln` with `Api`, `Core`, and `Infrastructure` projects.
- Purpose: minimal ASP.NET Core API backed by Cosmos DB, focused on showcasing infrastructure patterns (Cosmos DbContext, repositories) and CI/CD via the GitHub workflow in `.github/workflows/main_azureexample.yml`.

### Getting Started
1. Pick the solution you want to explore (`CleanArchitecture.sln` or `Azure/Azure.sln`).
2. Restore and build with `dotnet restore` / `dotnet build`.
3. Follow the per-solution README inside each folder for detailed instructions.

> Tip: Because this is a monorepo, workflows and shared configuration live at the root (e.g., `.github/workflows`). Keep an eye on relative paths when updating build or deploy scripts.
