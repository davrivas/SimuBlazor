# Pull Request: Upgrade to .NET 10

**Branch**: `upgrade-to-NET10`  
**Target**: `master`  
**Commit**: `d90ce5e`

## Summary

This PR upgrades all projects in the Simu solution from `.NET 8` to `.NET 10`, including updates to 7 NuGet packages to versions compatible with the new framework.

### What Changed

- **Projects upgraded (5)**: All projects now target `net10.0`
  - `Simu.Core/Simu.Core.csproj`
  - `Simu.Data/Simu.Data.csproj`
  - `Simu.Business/Simu.Business.csproj`
  - `Simu.API/Simu.API.csproj`
  - `Simu.Blazor.Web/Simu.Blazor.Web.csproj`

- **NuGet packages updated (7 of 8)**:
  - `Microsoft.EntityFrameworkCore`: 8.0.0 → 10.0.5
  - `Microsoft.EntityFrameworkCore.SqlServer`: 8.0.0 → 10.0.5
  - `Microsoft.EntityFrameworkCore.Tools`: 8.0.0 → 10.0.5
  - `Microsoft.AspNetCore.Components.WebAssembly`: 8.0.0 → 10.0.5
  - `Microsoft.AspNetCore.Components.WebAssembly.DevServer`: 8.0.0 → 10.0.5
  - `System.Net.Http.Json`: 8.0.0 → 10.0.5
  - `Microsoft.AspNetCore.OpenApi`: 8.0.0 → 10.0.5
  - `Swashbuckle.AspNetCore`: 6.4.6 (compatible — no update needed)

### Build Status

✅ **Solution builds successfully with 0 errors**

Build output verified after upgrade. Two non-blocking warnings noted:
- `NU1510`: `System.Net.Http.Json` may be unnecessary in `Simu.Blazor.Web` (verify if actively used)
- `NU1603`: `Swashbuckle.AspNetCore` 6.4.6 not found; 6.5.0 resolved instead (compatible)

### Testing

- No unit test projects were discovered in the solution; `dotnet test` completed successfully with restore and build phases.
- **Recommended manual testing**: Blazor runtime validation for navigation and HTTP call behavior, especially around `System.Uri` changes (see below).

### Risk Assessment

- **Low risk** for most projects (Simu.Core, Simu.Business, Simu.API)
- **Medium risk** for `Simu.Blazor.Web` due to `System.Uri` behavioral changes in .NET 10
- **Medium risk** for `Simu.Data` due to Entity Framework Core version bump (EF Core 8 → 10)

### Known Issues & Behavioral Changes

#### System.Uri Behavioral Changes (Blazor Project)
.NET 10 introduces changes to `System.Uri` parsing and handling of relative/absolute URIs. Code that previously relied on implicit URI parsing behavior may behave differently. 

**Action items**:
- Review any code constructing URIs in `Simu.Blazor.Web` for relative URI handling
- Use `new Uri(baseUri, relativeUri)` pattern for explicit relative URI construction
- Use `Uri.TryCreate()` with explicit `UriKind` parameter when needed
- Runtime testing of navigation flows and HTTP client calls

#### Entity Framework Core Changes
EF Core 8 → 10 may introduce differences in:
- Migration behavior
- Change tracking
- Query translation

**Action items**:
- Run EF Core migrations against a test database
- Verify data access integration tests pass
- Check for any query behavior changes in business logic

### Assessment & Plan Documents

Full technical details are available in the upgrade artifacts:

- **Assessment Report**: `.github/upgrades/scenarios/new-dotnet-version_ec2d93/assessment.md`
  - 5 projects analyzed
  - 8 NuGet packages reviewed
  - 1,512 lines of code assessed
  - 3 behavioral API changes flagged (all in `System.Uri`)
  - All projects are SDK-style and have no circular dependencies

- **Upgrade Plan**: `.github/upgrades/scenarios/new-dotnet-version_ec2d93/plan.md`
  - All-At-Once upgrade strategy (atomic, single-pass operation)
  - Project-by-project migration details
  - Package update reference with exact version mappings
  - Breaking changes catalog
  - Testing and validation strategy
  - Risk management and contingency plans
  - Success criteria

### Next Steps for Reviewers

1. **Code Review**: Review the project file changes and ensure they align with your team's framework upgrade policies
2. **Manual Testing**: Test the Blazor application locally to validate:
   - Application startup and page navigation
   - HTTP client calls and API integration
   - Any forms or URI-dependent features
3. **Integration Testing**: Run any external integration tests against updated EF Core
4. **Security Review**: Verify there are no new security vulnerabilities in upgraded packages (run `dotnet list package --vulnerable` if desired)
5. **Merge & Deploy**: After approval, merge to `master` and coordinate with your CI/CD pipeline

### Checklist

- [x] All projects updated to `net10.0`
- [x] NuGet packages updated
- [x] Solution builds with 0 errors
- [x] No breaking API changes (assessment confirmed compatibility)
- [ ] Manual runtime testing completed (recommended before merge)
- [ ] Integration tests executed (if applicable)
- [ ] Team code review approval
- [ ] Merge to master

### Questions or Issues?

If you encounter issues during review or testing:
1. Check the assessment and plan documents for detailed guidance
2. Refer to the breaking changes catalog in the plan for mitigation strategies
3. Consult .NET 10 and EF Core 10 migration guides at https://learn.microsoft.com/

---

**Prepared by**: GitHub Copilot App Modernization Agent  
**Upgrade Scenario ID**: `new-dotnet-version_ec2d93`
