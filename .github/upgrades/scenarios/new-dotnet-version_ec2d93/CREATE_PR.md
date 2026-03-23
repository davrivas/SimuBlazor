# PR Creation Guide — .NET 10 Upgrade

## Quick PR Details

**Branch to Create PR From**: `upgrade-to-NET10`  
**Target Branch**: `master`  
**Base Commit**: `cbda22d` (origin/master)  
**Head Commit**: `d90ce5e` (current upgrade-to-NET10)

### PR Title

```
Upgrade to .NET 10 — Framework and NuGet package updates
```

### PR Description

Copy the full content from: `.github/upgrades/scenarios/new-dotnet-version_ec2d93/PR_DESCRIPTION.md`

Or paste this summary:

---

## Upgrade Summary

- **Target**: Upgrade all 5 projects from `.NET 8` to `.NET 10`
- **Scope**: Framework version changes + 7 NuGet package updates
- **Status**: ✅ Solution builds successfully with 0 errors
- **Risk**: Low to Medium (medium for Blazor WASM due to System.Uri changes; medium for Data layer due to EF Core updates)

### Changes Made

**Project Files Updated**:
- `Simu.Core/Simu.Core.csproj`: `net8.0` → `net10.0`
- `Simu.Data/Simu.Data.csproj`: `net8.0` → `net10.0`
- `Simu.Business/Simu.Business.csproj`: `net8.0` → `net10.0`
- `Simu.API/Simu.API.csproj`: `net8.0` → `net10.0`
- `Simu.Blazor.Web/Simu.Blazor.Web.csproj`: `net8.0` → `net10.0`

**Package Updates**:
- Microsoft.EntityFrameworkCore: 8.0.0 → 10.0.5
- Microsoft.EntityFrameworkCore.SqlServer: 8.0.0 → 10.0.5
- Microsoft.EntityFrameworkCore.Tools: 8.0.0 → 10.0.5
- Microsoft.AspNetCore.Components.WebAssembly: 8.0.0 → 10.0.5
- Microsoft.AspNetCore.Components.WebAssembly.DevServer: 8.0.0 → 10.0.5
- System.Net.Http.Json: 8.0.0 → 10.0.5
- Microsoft.AspNetCore.OpenApi: 8.0.0 → 10.0.5

### Key Considerations

1. **Blazor Runtime Testing Required**: System.Uri behavioral changes may affect URI parsing in Blazor WASM app
2. **EF Core Testing Required**: Verify database migrations and data access after EF Core 8 → 10 update
3. **Build Status**: Solution builds with 0 errors; two non-blocking NuGet warnings noted
4. **Assessment & Plan Available**: Full technical documentation in `.github/upgrades/scenarios/new-dotnet-version_ec2d93/`

---

## Steps to Create PR (GitHub)

1. Go to your repository on GitHub
2. Click **Pull requests** tab
3. Click **New pull request**
4. Set:
   - **Compare**: `upgrade-to-NET10`
   - **Base**: `master`
5. Click **Create pull request**
6. **Title**: "Upgrade to .NET 10 — Framework and NuGet package updates"
7. **Description**: Paste the content from `PR_DESCRIPTION.md` (see file path above)
8. Click **Create pull request**

## Steps to Create PR (Azure DevOps)

1. Go to your project
2. Click **Repos** → **Pull requests**
3. Click **New pull request**
4. Set:
   - **Source branch**: `upgrade-to-NET10`
   - **Target branch**: `master`
5. Click **Create**
6. Fill in title and description as above
7. Click **Create**

## Commit Details

The upgrade was performed in a single atomic commit:

```
commit d90ce5e
Author: GitHub Copilot App Modernization Agent
Date: [execution timestamp]

    TASK-002: Atomic upgrade to net10.0 — project files and package updates

    - Updated TargetFramework to net10.0 in all 5 project files
    - Updated NuGet package versions per assessment recommendations
    - Solution builds with 0 errors
    - Ready for code review and runtime validation
```

## Pre-Merge Checklist

- [ ] PR reviewed and approved by team
- [ ] Blazor WASM application tested locally (navigation, HTTP calls)
- [ ] EF Core migrations validated against test database
- [ ] No new compiler warnings introduced (beyond NU1510/NU1603)
- [ ] Integration tests pass (if applicable)
- [ ] Security review of updated packages completed

## Post-Merge Recommended Actions

1. Monitor CI/CD pipeline for any build or deployment issues
2. Stage the upgraded application in a test environment
3. Run full integration and smoke tests
4. Coordinate with DevOps for production deployment planning

---

**Reference Documents**:
- Assessment: `.github/upgrades/scenarios/new-dotnet-version_ec2d93/assessment.md`
- Plan: `.github/upgrades/scenarios/new-dotnet-version_ec2d93/plan.md`
- Tasks: `.github/upgrades/scenarios/new-dotnet-version_ec2d93/tasks.md`
- PR Description: `.github/upgrades/scenarios/new-dotnet-version_ec2d93/PR_DESCRIPTION.md` (this file)
