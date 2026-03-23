# SimuBlazor .NET 10 Upgrade Tasks

## Overview

Upgrade all projects in the repository from `net8.0` to `net10.0` and apply the package updates listed in the plan. The approach uses a single atomic upgrade operation for project and package changes, preceded by prerequisite verification and followed by automated test execution and fixes.

**Progress**: 3/3 tasks complete (100%) ![0%](https://progress-bar.xyz/100)

---

## Tasks

### [✓] TASK-001: Verify prerequisites *(Completed: 2026-03-23 02:48)*
**References**: Plan §4 (Common prerequisites), Plan §2 (Migration Strategy)

- [✓] (1) Verify .NET 10 SDK is installed on the upgrade machine/CI agents (run `dotnet --list-sdks` / `dotnet --version`) per Plan §4
- [✓] (2) Runtime/SDK version meets minimum requirements (**Verify**)
- [✓] (3) If `global.json` exists, verify it references a compatible SDK or update it per Plan §4 (ensure compatibility with .NET 10)
- [✓] (4) `global.json` compatible with .NET 10 (**Verify**)
- [✓] (5) Verify required global tools referenced in the plan (e.g., `dotnet-ef` if used) are present or note update requirement per Plan §4 / Plan §Simu.Data

### [✓] TASK-002: Atomic framework and package upgrade with compilation fixes *(Completed: 2026-03-23 02:50)*
**References**: Plan §1 (Executive Summary), Plan §2 (Migration Strategy), Plan §4 (Project-by-Project Plans), Plan §5 (Package Update Reference), Plan §6 (Breaking Changes Catalog), Plan §10 (Source Control Strategy)

- [✓] (1) Update `TargetFramework` in all project files listed in Plan §1/§4 to `net10.0` per Plan §4
- [✓] (2) Update NuGet package versions across the solution per Plan §5 (or update `Directory.Packages.props` if central package management is used)
- [✓] (3) Restore dependencies (`dotnet restore`) for the solution
- [✓] (4) Build the solution and fix all compilation errors caused by framework and package updates (apply fixes guided by Plan §6 Breaking Changes Catalog)
- [✓] (5) Solution builds with 0 errors (**Verify**)
- [✓] (6) Commit changes with message: "TASK-002: Atomic upgrade to net10.0 — project files and package updates"

### [✓] TASK-003: Run test suite and validate upgrade *(Completed: 2026-03-23 02:50)*
**References**: Plan §7 (Testing & Validation Strategy), Plan §6 (Breaking Changes Catalog)

- [✓] (1) Run all discovered test projects via `dotnet test` per Plan §7
- [✓] (2) Fix any test failures (reference Plan §6 for common issues such as `System.Uri` and EF Core changes)
- [✓] (3) Re-run tests after fixes
- [✓] (4) All tests pass with 0 failures (**Verify**)
- [✓] (5) Commit test fixes with message: "TASK-003: Test fixes after .NET 10 upgrade"

---






