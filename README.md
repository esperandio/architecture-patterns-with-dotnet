# Architecture patterns with .NET

```sh
dotnet new xunit -o tests/UnitTesting
```

```sh
dotnet new classlib -o src/Domain
```

``` sh
dotnet add tests/UnitTesting/UnitTesting.csproj reference src/Domain/Domain.csproj
```

## References

- Architecture Patterns with Python (https://www.oreilly.com/library/view/architecture-patterns-with/9781492052197/)
- Getting Started with xUnit.net (https://xunit.net/docs/getting-started/netcore/cmdline)
- Implement value objects (https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/implement-value-objects)
- Tutorial: Get started with EF Core in an ASP.NET MVC web app (https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-6.0)
- Tutorial: Create a minimal web API with ASP.NET Core (https://docs.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio)
- Configuration in ASP.NET Core (https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-6.0)
- Safe storage of app secrets in development in ASP.NET Core (https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=linux)
- Loading Related Data (https://docs.microsoft.com/en-us/ef/core/querying/related-data/)
- How to preserve references and handle or ignore circular references in System.Text.Json (https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-preserve-references?pivots=dotnet-6-0#ignore-circular-references)