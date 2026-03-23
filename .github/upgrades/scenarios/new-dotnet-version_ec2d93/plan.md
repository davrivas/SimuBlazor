# .NET 10 Upgrade Plan

## Table of Contents
- [1 Executive Summary](#1-executive-summary)
- [2 Migration Strategy](#2-migration-strategy)
- [3 Detailed Dependency Analysis](#3-detailed-dependency-analysis)
- [4 Project-by-Project Plans](#4-project-by-project-plans)
- [5 Package Update Reference](#5-package-update-reference)
- [6 Breaking Changes Catalog](#6-breaking-changes-catalog)
- [7 Testing & Validation Strategy](#7-testing--validation-strategy)
- [8 Risk Management](#8-risk-management)
- [9 Complexity & Effort Assessment](#9-complexity--effort-assessment)
- [10 Source Control Strategy](#10-source-control-strategy)
- [11 Success Criteria](#11-success-criteria)

---

## 1 Executive Summary

### Scenario
Upgrade all projects in the repository from `net8.0` to `net10.0` and update NuGet packages to assessment-suggested versions. The workspace contains a Blazor WebAssembly project (`Simu.Blazor.Web`) which requires special attention for runtime behavioral changes.

### Scope
- Projects: 5
  - `Simu.API\\Simu.API.csproj`
  - `Simu.Blazor.Web\\Simu.Blazor.Web.csproj`
  - `Simu.Business\\Simu.Business.csproj`
  - `Simu.Core\\Simu.Core.csproj`
  - `Simu.Data\\Simu.Data.csproj`
- All projects currently target `net8.0` and the assessment proposes `net10.0`.
- NuGet packages to update: 7 packages (see §5 Package Update Reference).

### Key Metrics (from assessment)
- Total Projects: 5
- Total NuGet Packages: 8 (7 require upgrade)
- Total Lines of Code: 1,512
- Files with incidents: 6
- API behavioral changes flagged: 3 (primarily `System.Uri` behavior affecting Blazor project)

### Selected Strategy
**All-At-Once Strategy** — All projects will be upgraded simultaneously in a single atomic operation.

Rationale:
- Solution size and complexity are small (5 projects, 1.5k LOC).
- All projects are SDK-style and homogeneous in structure.
- All suggested package updates have target versions available for .NET 10 (assessment shows 10.0.5 for framework packages).
- Single coordinated upgrade minimizes branching complexity and produces a unified repo state.

### High-level Deliverables
- Atomically-upgraded solution with all projects targeting `net10.0`.
- All assessment-suggested NuGet package updates applied.
- Solution builds with 0 errors; all automated tests (if present) pass.
- Validation notes for Blazor runtime behavioral changes.

---

## 2 Migration Strategy

### Approach
- Use the All-At-Once approach: change TargetFramework and package versions in all projects in one coordinated commit on branch `upgrade-to-NET10`.
- Preserve repository cleanliness: create and switch to `upgrade-to-NET10` branch before making any changes.
- Ensure prerequisites (developer SDK, global.json) are validated prior to changes.

### Atomic Upgrade Sequence (one bounded operation)
1. Ensure working tree is clean (no pending changes) or apply chosen pending-changes action.
2. Create and switch to branch `upgrade-to-NET10`.
3. Update TargetFramework properties for all projects to `net10.0` (or append target if multitargeted per-assessment).
4. Update NuGet package versions as detailed in §5 Package Update Reference.
5. Restore dependencies (`dotnet restore`).
6. Build solution and fix all compilation errors caused by the upgrade (single pass: build and address all compilation errors as part of the atomic change).
7. Verify solution builds with 0 errors.
8. Run all test projects and resolve test failures.
9. Create a single commit containing all project file and package updates; open a PR for code review.

Notes:
- The atomic operation encompasses both project file edits and package updates and compilation fixes (TASK-001 in executor terms).
- Testing and any further fixes occur after the atomic upgrade is successful.

---

## 3 Detailed Dependency Analysis

### Dependency Summary
Assessment dependency graph shows a shallow dependency tree with `Simu.Core` as the common library consumed by multiple projects. Project dependency relationships (downstream/upstream):
- `Simu.Core` — leaf library used by API, Blazor, Business, Data
- `Simu.Data` → depends on `Simu.Core`
- `Simu.Business` → depends on `Simu.Core`, `Simu.Data`
- `Simu.API` → depends on `Simu.Business`, `Simu.Core`, `Simu.Data`
- `Simu.Blazor.Web` → depends on `Simu.Core`

### Migration Ordering Considerations
- Although All-At-Once updates all projects simultaneously, the dependency graph must be respected during validation: build and testing should confirm downstream projects compile against upgraded dependencies.
- Test projects (none discovered explicitly in assessment) should run after the atomic upgrade.

### Circular dependencies
- No circular dependencies reported.

---

## 4 Project-by-Project Plans

### Common prerequisites for all projects
- Ensure .NET 10 SDK is installed on upgrade machine and CI agents. Validate using `dotnet --list-sdks` and `dotnet --version`.
- Verify `global.json` (if present) references a compatible SDK or update it to a version that supports .NET 10.
- Create and switch to branch `upgrade-to-NET10` from `master` before making changes.

---

### Project: Simu.Core (Class Library)
**Current State**: `net8.0` (SDK-style)
**Target State**: `net10.0`

Migration Steps:
1. Update `TargetFramework` in `Simu.Core.csproj` to `net10.0`.
2. Restore and build solution to discover compilation issues.
3. Update package references only if assessment lists packages for this project (none listed).
4. Verify public APIs compile; run unit tests if present.

Expected Breaking Changes / Notes:
- Assessment reports no API incompatibilities for `Simu.Core`.

Validation:
- Build succeeds with 0 errors.
- Dependant projects compile successfully against upgraded `Simu.Core`.

Risk: Low

---

### Project: Simu.Data (Class Library)
**Current State**: `net8.0` (SDK-style)
**Target State**: `net10.0`

Migration Steps:
1. Update `TargetFramework` in `Simu.Data.csproj` to `net10.0`.
2. Update EF Core packages to `10.0.5` (`Microsoft.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.SqlServer`, `Microsoft.EntityFrameworkCore.Tools`).
3. Restore and build solution; address compilation errors.
4. Verify EF Core tooling (`dotnet ef`) compatibility and update `dotnet-ef` global tool if used.
5. Run any data-layer unit tests and migration scripts.

Expected Breaking Changes / Notes:
- EF Core version change may introduce provider or migration behavior differences; plan to run database migrations in a test environment.

Validation:
- Build succeeds; EF Core migrations run in test DB.
- No runtime exceptions in data access during integration tests.

Risk: Medium (due to EF Core updates)

---

### Project: Simu.Business (Class Library)
**Current State**: `net8.0` (SDK-style)
**Target State**: `net10.0`

Migration Steps:
1. Update `TargetFramework` in `Simu.Business.csproj` to `net10.0`.
2. Update package references if assessment suggests (Microsoft.EntityFrameworkCore referenced - update to 10.0.5).
3. Restore and build solution; address compilation errors.
4. Run unit tests that cover business logic.

Expected Breaking Changes / Notes:
- No direct API incompatibilities detected. Verify integration with updated `Simu.Data`.

Validation:
- Build succeeds; unit tests pass.

Risk: Low

---

### Project: Simu.API (ASP.NET Core Web API)
**Current State**: `net8.0` (SDK-style)
**Target State**: `net10.0`

Migration Steps:
1. Update `TargetFramework` in `Simu.API.csproj` to `net10.0`.
2. Update ASP.NET Core/OpenAPI related packages (Microsoft.AspNetCore.OpenApi -> 10.0.5) and any other packages recommended.
3. Restore and build the solution; address compilation errors.
4. Validate Swagger/Swashbuckle integration (Swashbuckle.AspNetCore reported compatible).
5. Run integration tests and sample API calls.

Expected Breaking Changes / Notes:
- No major API incompatibilities reported; watch for configuration changes in minimal hosting or authentication behavior.

Validation:
- Build succeeds; API endpoints return expected responses in automated integration tests.

Risk: Low

---

### Project: Simu.Blazor.Web (Blazor WebAssembly)
**Current State**: `net8.0` (SDK-style)
**Target State**: `net10.0`

Migration Steps:
1. Update `TargetFramework` in `Simu.Blazor.Web.csproj` to `net10.0`.
2. Update Blazor WASM packages to `10.0.5` (`Microsoft.AspNetCore.Components.WebAssembly`, `Microsoft.AspNetCore.Components.WebAssembly.DevServer`).
3. Update `System.Net.Http.Json` to `10.0.5`.
4. Restore and build solution; address compilation or build-time warnings.
5. Pay attention to `System.Uri` behavioral changes — identify usages where URI constructors or parsing are used and add explicit parsing/UriKind handling if needed.
6. Run the Blazor application locally (in a test environment) to validate runtime behavior, navigation, and HTTP calls.

Expected Breaking Changes / Notes:
- Behavioral changes in `System.Uri` may alter relative/absolute URI parsing. Code that relies on previous parsing behavior should be adjusted to use `new Uri(baseUri, relativeUri)` or Uri.TryCreate with proper UriKind.

Validation:
- WASM app loads and runs; navigation and remote calls function as expected in browser tests.

Risk: Medium (runtime behavioral changes in Blazor)

---

## 5 Package Update Reference

### Common Package Updates (affecting multiple projects)
- `Microsoft.EntityFrameworkCore`: 8.0.0 -> 10.0.5 (affects `Simu.Business`, `Simu.Data`) — Required for compatibility with `net10.0` and to address assessment recommendation.
- `Microsoft.EntityFrameworkCore.SqlServer`: 8.0.0 -> 10.0.5 (affects `Simu.Data`) — Database provider update.
- `Microsoft.EntityFrameworkCore.Tools`: 8.0.0 -> 10.0.5 (affects `Simu.Data`) — Tooling support for migrations.

### Blazor / Client-side updates
- `Microsoft.AspNetCore.Components.WebAssembly`: 8.0.0 -> 10.0.5 (affects `Simu.Blazor.Web`) — Update for WASM runtime compatibility.
- `Microsoft.AspNetCore.Components.WebAssembly.DevServer`: 8.0.0 -> 10.0.5 (affects `Simu.Blazor.Web`) — Local dev server for WASM.
- `System.Net.Http.Json`: 8.0.0 -> 10.0.5 (affects `Simu.Blazor.Web`) — HTTP JSON APIs.

### API / Server updates
- `Microsoft.AspNetCore.OpenApi`: 8.0.0 -> 10.0.5 (affects `Simu.API`) — OpenAPI support aligned with server framework.
- `Swashbuckle.AspNetCore`: 6.4.6 -> (compatible) (affects `Simu.API`) — Assessment shows no required update; keep version unless tests reveal issues.

### Notes
- Apply all package updates as part of the atomic upgrade. Do not defer package updates.
- If any package in the repo uses Central Package Management (Directory.Packages.props), update versions there instead of per-project updates.

---

## 6 Breaking Changes Catalog

### Known / Expected Breaking Changes (based on assessment)
- `System.Uri` behavioral changes (affects `Simu.Blazor.Web`):
  - Some overloads and parsing behavior may treat certain strings differently (relative vs absolute). Update code to use `UriKind` explicitly or use `Uri.TryCreate`.
  - Use `new Uri(baseUri, relativeUri)` for combining URIs.

### Potential Areas to Watch
- EF Core 8 -> 10 changes: migrations, change tracking, query translation differences. Run migration tests.
- ASP.NET Core hosting/configuration differences: minimal hosting model is stable but verify DI, authentication, and middleware ordering.

### Discovery during build
- Expect to find source-level API changes during the single atomic build; catalog any method/namespace changes found and add them to this section during execution.

---

## 7 Testing & Validation Strategy

### Validation Checklist (post-upgrade)
For each project:
- [ ] Builds without errors
- [ ] No new compiler warnings that indicate deprecated APIs
- [ ] Unit tests pass (where present)
- [ ] Integration tests pass (where present)

### Blazor-specific validation
- Verify app bootstrapping and client-side routing
- Validate HTTP API calls using `System.Net.Http.Json` after upgrade
- Test URI parsing/navigation flows that previously relied on earlier `System.Uri` behavior

### Test Execution
- Run all discovered test projects via `dotnet test` after the atomic upgrade.
- For integration tests requiring external services (DB), run against test instances.

---

## 8 Risk Management

### Risk Summary
- Medium risk for `Simu.Blazor.Web` due to behavioral `System.Uri` changes.
- Medium risk for `Simu.Data` due to EF Core version bump.
- Low risk for other projects (build and package changes only).

### Mitigations
- Run Blazor runtime smoke tests in browser to detect URI and navigation issues.
- Run EF Core migrations in a disposable test database and validate data access integration tests.
- Keep a single atomic commit and PR for easy rollback if issues require reverting.

### Contingency
- If blocking runtime or EF Core issues are discovered, revert the upgrade branch and create an issue with reproduction steps; consider switching to an incremental approach for the problematic projects.

---

## 9 Complexity & Effort Assessment

| Project | LOC | Incident Files | Risk | Complexity |
| --- | ---: | ---: | --- | --- |
| Simu.Core | 319 | 1 | Low | Low |
| Simu.Data | 368 | 1 | Medium | Medium |
| Simu.Business | 395 | 1 | Low | Low |
| Simu.API | 418 | 1 | Low | Low |
| Simu.Blazor.Web | 12 | 2 | Medium | Medium |

Notes:
- Complexity is relative and expressed as Low/Medium/High. No time estimates are provided.

---

## 10 Source Control Strategy

- Branching: create `upgrade-to-NET10` from `master` and perform all changes there.
- Commit strategy: prefer a single atomic commit containing all project file and package updates. Keep additional commits for non-upgrade-related cleanup out of this branch.
- Pull Request: open PR from `upgrade-to-NET10` to `master`. Include the assessment and this plan as references.

---

## 11 Success Criteria

The upgrade is complete when all the following conditions are met:
1. All projects target `net10.0`.
2. All package updates listed in §5 are applied.
3. Solution builds successfully with 0 errors.
4. All unit and integration tests pass.
5. Blazor runtime behaviors (navigation, URI parsing) validated and any required code changes applied.
6. No remaining critical security vulnerabilities reported for updated packages.


---
