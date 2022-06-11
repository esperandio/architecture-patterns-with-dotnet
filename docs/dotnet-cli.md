# Dotnet CLI

## Add project reference

```
dotnet add [ csproj target ] reference [ csproj reference ]
```

## Add project to solution

```
dotnet sln add [ csproj target ] 
```

## Setting connection string as an environment variable

```
export ConnectionStrings__ApplicationDbContext=server=127.0.0.1\;port=3306\;database=container\;user=root\;password=my-secret-pw
```