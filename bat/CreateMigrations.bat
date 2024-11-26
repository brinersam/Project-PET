cd ../
dotnet ef migrations add InitialSpecies -s src/ProjectPet.Web/ProjectPet.Web.csproj -p src/Species/ProjectPet.Species.Infrastructure --context WriteDbContext
dotnet ef migrations add InitialVolunteer -s src/ProjectPet.Web/ProjectPet.Web.csproj -p src/Volunteer/ProjectPet.Volunteer.Infrastructure --context WriteDbContext
dotnet ef migrations add InitialAccounts -s src/ProjectPet.Web/ProjectPet.Web.csproj -p src/AccountsModule/ProjectPet.AccountsModule.Infrastructure --context AuthDbContext