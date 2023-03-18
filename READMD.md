```sh
docker build -t architecture-patterns-dotnet-dev -f Dockerfile.development .
```

``` sh
docker run -tti --rm --volume "$(pwd)/src":/app -w /app architecture-patterns-dotnet-dev bash
```

## References

- Official images for .NET and ASP.NET Core (https://hub.docker.com/_/microsoft-dotnet/)
- Example application code for the python architecture book (https://github.com/cosmicpython/code)
- Unit testing C# in .NET Core using dotnet test and xUnit (https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test)
- How To Persist DDD Value Objects? (https://www.youtube.com/watch?v=2XsdAaWi7ZQ)