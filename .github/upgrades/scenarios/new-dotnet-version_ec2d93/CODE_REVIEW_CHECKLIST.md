# Code Review Checklist — .NET 8 to .NET 10 Upgrade

## Overview
Use this checklist to conduct a thorough code review of the .NET 10 upgrade PR (`upgrade-to-NET10`). The PR updates all 5 projects from `.NET 8` to `.NET 10` with 7 NuGet package updates.

---

## ✅ Part 1: Project File Changes

### Target Framework Updates
- [ ] **Simu.Core** updated from `net8.0` to `net10.0` ✓
- [ ] **Simu.Data** updated from `net8.0` to `net10.0` ✓
- [ ] **Simu.Business** updated from `net8.0` to `net10.0` ✓
- [ ] **Simu.API** updated from `net8.0` to `net10.0` ✓
- [ ] **Simu.Blazor.Web** updated from `net8.0` to `net10.0` ✓
- [ ] No projects remain on `net8.0` (verify no partial upgrades)
- [ ] Multi-target framework configurations reviewed (if applicable)

### Project File Structure
- [ ] All `.csproj` files are valid XML and properly formatted
- [ ] No duplicate `<TargetFramework>` or `<TargetFrameworks>` entries
- [ ] PropertyGroup and ItemGroup structures are intact
- [ ] Project references are unchanged (no broken references)
- [ ] SDK declarations are appropriate for each project type:
  - [ ] `Simu.Core`: `Microsoft.NET.Sdk` ✓
  - [ ] `Simu.Data`: `Microsoft.NET.Sdk` ✓
  - [ ] `Simu.Business`: `Microsoft.NET.Sdk` ✓
  - [ ] `Simu.API`: `Microsoft.NET.Sdk.Web` ✓
  - [ ] `Simu.Blazor.Web`: `Microsoft.NET.Sdk.BlazorWebAssembly` ✓

---

## ✅ Part 2: NuGet Package Updates

### Package Versions
- [ ] `Microsoft.EntityFrameworkCore`: 8.0.0 → 10.0.5 in Simu.Business, Simu.Data ✓
- [ ] `Microsoft.EntityFrameworkCore.SqlServer`: 8.0.0 → 10.0.5 in Simu.Data ✓
- [ ] `Microsoft.EntityFrameworkCore.Tools`: 8.0.0 → 10.0.5 in Simu.Data ✓
- [ ] `Microsoft.AspNetCore.Components.WebAssembly`: 8.0.0 → 10.0.5 in Simu.Blazor.Web ✓
- [ ] `Microsoft.AspNetCore.Components.WebAssembly.DevServer`: 8.0.0 → 10.0.5 in Simu.Blazor.Web ✓
- [ ] `System.Net.Http.Json`: 8.0.0 → 10.0.5 in Simu.Blazor.Web ✓
- [ ] `Microsoft.AspNetCore.OpenApi`: 8.0.0 → 10.0.5 in Simu.API ✓
- [ ] `Swashbuckle.AspNetCore`: 6.4.6 (unchanged — verified as compatible) ✓

### Package Management
- [ ] All packages are from NuGet.org (no custom/internal feeds unless documented)
- [ ] No version wildcards or floating versions used
- [ ] No conflicting version constraints between projects
- [ ] `PrivateAssets` and `IncludeAssets` attributes preserved where needed
- [ ] No package reference duplication across projects

### Security Review
- [ ] Run `dotnet list package --vulnerable` to verify no CVEs in updated packages
- [ ] Check security advisories for each package:
  - [ ] EntityFrameworkCore 10.0.5
  - [ ] AspNetCore packages 10.0.5
  - [ ] System.Net.Http.Json 10.0.5
  - [ ] Swashbuckle.AspNetCore 6.5.0 (auto-resolved)
- [ ] No deprecated packages introduced

---

## ✅ Part 3: Build & Compilation

### Build Results
- [ ] ✅ Solution compiles with **0 errors**
- [ ] No new compiler warnings introduced (beyond expected NuGet warnings)
- [ ] Expected warnings reviewed:
  - [ ] `NU1510`: `System.Net.Http.Json` may be unnecessary in Simu.Blazor.Web
    - **Action**: Verify if actively used; consider removal if not needed
  - [ ] `NU1603`: `Swashbuckle.AspNetCore` 6.4.6 → 6.5.0 resolved
    - **Action**: Verify compatibility; can lock to 6.4.6 if needed

### Dependency Resolution
- [ ] All transitive dependencies resolved successfully
- [ ] No circular dependencies introduced
- [ ] Project dependency graph respected (see assessment dependency graph)
- [ ] Downstream projects compile against upgraded dependencies

### Architecture
- [ ] No changes to project architecture or structure
- [ ] Dependency injection configurations intact (if using DI)
- [ ] Middleware ordering unchanged (for Simu.API)
- [ ] Blazor component registration unchanged (for Simu.Blazor.Web)

---

## ✅ Part 4: Framework-Specific Changes

### .NET 10 Compatibility
- [ ] Code targets .NET 10 API surface only
- [ ] No obsolete APIs are used
- [ ] Nullable reference types enabled (enabled in all projects) ✓
- [ ] Implicit using statements enabled (enabled in all projects) ✓
- [ ] No platform-specific code concerns for target deployment platform

### ASP.NET Core (Simu.API)
- [ ] `Program.cs` uses modern minimal hosting (verify if present)
- [ ] OpenAPI/Swagger configuration compatible with 10.0.5
- [ ] Authentication/Authorization middleware updated if applicable
- [ ] CORS, SSL, and other middleware configurations reviewed
- [ ] Endpoint conventions preserved

### Blazor WebAssembly (Simu.Blazor.Web)
- [ ] **[HIGH PRIORITY]** Review for `System.Uri` behavioral changes (see Part 5)
- [ ] Blazor routing configuration compatible
- [ ] Component base classes and lifecycle hooks compatible
- [ ] JavaScript interop unchanged (if used)
- [ ] Service registration in `Program.cs` compatible
- [ ] Static assets and wwwroot structure unchanged

### Entity Framework Core (Simu.Data, Simu.Business)
- [ ] DbContext configurations compatible with EF Core 10
- [ ] Migration strategy documented and tested (see Part 5)
- [ ] Change tracking behavior reviewed
- [ ] Query translation changes assessed
- [ ] LINQ-to-SQL expressions reviewed for compatibility

---

## ✅ Part 5: Known Breaking Changes & Mitigations

### System.Uri Behavioral Changes (Blazor WASM Priority)
**Impact**: Code parsing/constructing relative URIs may behave differently in .NET 10.

**Review checklist**:
- [ ] Search `Simu.Blazor.Web` for all `new Uri(` constructions
- [ ] Identify any relative URI handling (e.g., `new Uri("pages/home")`)
- [ ] Review navigation code and HTTP client calls
- [ ] Check for implicit URI string conversions

**Code patterns to verify**:
```csharp
// ❌ Potentially problematic (verify behavior):
var uri = new Uri("relative/path");
var uri = new Uri(baseUri, "relative");

// ✅ Safe patterns:
var uri = new Uri(baseUri, new Uri("relative", UriKind.Relative));
Uri.TryCreate(baseUri, "relative", out var result);
```

**Action items**:
- [ ] Document any URI parsing changes found
- [ ] Create code fixes if needed (add to follow-up tasks)
- [ ] Plan runtime testing of navigation and HTTP calls

### Entity Framework Core 8 → 10 Changes
**Impact**: Migration behavior, change tracking, and query translation may differ.

**Review checklist**:
- [ ] Database schema migrations reviewed
- [ ] Migration scripts tested against test database (see Part 6)
- [ ] Change tracking behavior verified (if custom tracking used)
- [ ] LINQ query translations checked (complex queries)
- [ ] Lazy loading and explicit loading behavior verified

**Code patterns to verify**:
```csharp
// Migration-related
// ❌ Check if migrations use deprecated APIs
// ✅ Ensure fluent API configurations are compatible

// Queries
// Review for potential translation differences
context.Users.Where(u => u.IsActive).ToList();
```

**Action items**:
- [ ] Schedule migration testing against test database
- [ ] Document any schema changes
- [ ] Create performance baselines if applicable

### ASP.NET Core 8 → 10 Changes (Simu.API)
**Impact**: Hosting, configuration, and middleware behavior may change.

**Review checklist**:
- [ ] Minimal hosting model compatibility verified (if used)
- [ ] OpenAPI/Swagger generation tested
- [ ] Authentication token validation timing checked
- [ ] Dependency injection scope behavior verified
- [ ] Configuration providers and file loading unchanged

---

## ✅ Part 6: Testing Requirements

### Unit Tests (If Applicable)
- [ ] All unit tests compile with new framework
- [ ] Unit tests execute successfully
- [ ] No test framework version mismatches (xUnit, NUnit, MSTest)
- [ ] Test fixtures and mocks compatible

### Integration Tests (If Applicable)
- [ ] API integration tests execute successfully
- [ ] Database integration tests pass (EF Core-dependent tests)
- [ ] HTTP client tests compatible with updated packages
- [ ] Authentication/authorization tests pass

### Manual Runtime Testing (Required Before Merge)
#### For Simu.API
- [ ] API starts without errors
- [ ] OpenAPI/Swagger UI loads
- [ ] Sample API endpoints return correct responses
- [ ] Authentication/authorization flows work
- [ ] Database connectivity verified

#### For Simu.Blazor.Web (Priority)
- [ ] Application starts in browser without errors
- [ ] Home page loads and renders correctly
- [ ] Navigation between pages works
- [ ] HTTP calls to API succeed
- [ ] Forms and user interactions functional
- [ ] **[CRITICAL]** URI-based navigation tested (relative/absolute paths)
- [ ] Console for JavaScript errors checked

#### For Simu.Data / Simu.Business
- [ ] Database migrations run successfully on test environment
- [ ] Data access operations (CRUD) functional
- [ ] Complex queries execute without errors
- [ ] Transaction handling works correctly

### Test Coverage
- [ ] Existing test coverage maintained or improved
- [ ] No regression in test results
- [ ] Build pipeline tests (if applicable) pass

---

## ✅ Part 7: Performance & Diagnostics

### Performance Considerations
- [ ] No obvious performance regressions in code (no new nested loops, etc.)
- [ ] Async/await patterns preserved where needed
- [ ] No memory leaks introduced (if known memory-sensitive code exists)
- [ ] Entity Framework query performance verified (if complex queries updated)

### Diagnostic Tools
- [ ] Consider running `dotnet analyze` if enabled for codebase
- [ ] Review IDE warnings and suggestions
- [ ] Check for deprecated API usage warnings

### Known Performance Changes (Informational)
- [ ] Be aware: .NET 10 may have different GC behavior (document if observed)
- [ ] Be aware: Entity Framework may optimize queries differently (expected, acceptable)

---

## ✅ Part 8: Documentation & Communication

### Assessment & Plan Documents
- [ ] Reviewed `.github/upgrades/scenarios/new-dotnet-version_ec2d93/assessment.md`
  - [ ] Understood 5 projects scope
  - [ ] Noted 7 package updates
  - [ ] Reviewed API behavioral changes (3 System.Uri issues)
  - [ ] Understood risk assessment (Low-Medium)

- [ ] Reviewed `.github/upgrades/scenarios/new-dotnet-version_ec2d93/plan.md`
  - [ ] Understood All-At-Once strategy
  - [ ] Reviewed project-by-project plans
  - [ ] Read breaking changes catalog
  - [ ] Understood testing strategy and contingency plans

### PR Documentation
- [ ] PR description is clear and complete
- [ ] Commit message is descriptive
- [ ] No sensitive information in commit/PR

### Team Communication
- [ ] Team aware of upgrade timeline and merge plans
- [ ] Deployment team notified of framework change
- [ ] Documentation updated (if applicable)

---

## ✅ Part 9: Pre-Merge Sign-Off

### Approval Gates
- [ ] Code review completed and approved
- [ ] Build pipeline passes (CI/CD checks)
- [ ] Manual testing results documented
- [ ] Security review completed
- [ ] Architecture review completed (if required by team)

### Deployment Readiness
- [ ] Deployment strategy finalized (rolling, blue-green, etc.)
- [ ] Rollback plan documented
- [ ] Monitoring and alerting configured for new version
- [ ] Release notes prepared

### Outstanding Items
- [ ] List any follow-up tasks or issues discovered
- [ ] Create GitHub issues for post-merge work if needed:
  - [ ] Example: "Optimize System.Uri usage in Blazor navigation"
  - [ ] Example: "Verify EF Core 10 performance against production data"

---

## 🔍 Quick Reference: What Changed

| File | Change | Verified |
|------|--------|----------|
| `Simu.Core/Simu.Core.csproj` | `net8.0` → `net10.0` | ✓ |
| `Simu.Data/Simu.Data.csproj` | `net8.0` → `net10.0` + 3 EF packages | ✓ |
| `Simu.Business/Simu.Business.csproj` | `net8.0` → `net10.0` + 1 EF package | ✓ |
| `Simu.API/Simu.API.csproj` | `net8.0` → `net10.0` + OpenAPI package | ✓ |
| `Simu.Blazor.Web/Simu.Blazor.Web.csproj` | `net8.0` → `net10.0` + 3 Blazor packages | ✓ |

---

## 📋 Sign-Off

**Reviewer Name**: ________________________  
**Date**: ________________________  
**Status**: [ ] Approved  [ ] Approved with Comments  [ ] Request Changes  [ ] Blocked

**Comments/Issues Found**:
```
[Document any issues, concerns, or findings here]
```

---

## 🚀 Post-Merge Tasks (If Applicable)

Create follow-up GitHub issues for:
- [ ] Blazor WASM runtime testing in staging environment
- [ ] EF Core migration validation against production schema
- [ ] Performance baseline establishment for .NET 10
- [ ] Update CI/CD pipeline for .NET 10 SDK
- [ ] Update team documentation and coding guidelines
- [ ] Plan and schedule deployment to production

---

**References**:
- Assessment: `.github/upgrades/scenarios/new-dotnet-version_ec2d93/assessment.md`
- Plan: `.github/upgrades/scenarios/new-dotnet-version_ec2d93/plan.md`
- PR: `upgrade-to-NET10` branch
- .NET 10 Migration Guide: https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10
- EF Core 10 Guide: https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-10.0
