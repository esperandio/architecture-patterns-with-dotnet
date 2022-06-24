run:
	dotnet watch run --project src/Main/Main.csproj
update:
	dotnet ef database update --project src/Infrastructure/Persistence/Persistence.csproj