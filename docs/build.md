## Build production image

```sh
docker build -t esperandio/architecture-patterns-with-dotnet -f Dockerfile.production .
```

## Run container

```sh
docker run -it --rm -p 8000:5000 \
    -e ASPNETCORE_URLS=http://+:5000 \
    -e DATABASE_CONNECTION_STRING="server=db;database=app;user=root;password=my-secret-pw" \
    esperandio/architecture-patterns-with-dotnet
```
