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