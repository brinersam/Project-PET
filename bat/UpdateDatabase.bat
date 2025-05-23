cd ../
dotnet ef database update --connection Host=localhost;Port=5432;Database=project_pet;Username=postgres;Password=postgres; -s src/ProjectPet.Web/ProjectPet.Web.csproj -p src/AccountsModule/ProjectPet.AccountsModule.Infrastructure --context AuthDbContext
dotnet ef database update --connection Host=localhost;Port=5432;Database=project_pet;Username=postgres;Password=postgres; -s src/ProjectPet.Web/ProjectPet.Web.csproj -p src/Species/ProjectPet.Species.Infrastructure --context WriteDbContext
dotnet ef database update --connection Host=localhost;Port=5432;Database=project_pet;Username=postgres;Password=postgres; -s src/ProjectPet.Web/ProjectPet.Web.csproj -p src/Volunteer/ProjectPet.Volunteer.Infrastructure --context WriteDbContext
dotnet ef database update --connection Host=localhost;Port=5432;Database=project_pet;Username=postgres;Password=postgres; -s src/ProjectPet.Web/ProjectPet.Web.csproj -p src/VolunteerRequest/ProjectPet.VolunteerRequest.Infrastructure --context WriteDbContext
dotnet ef database update --connection Host=localhost;Port=5432;Database=project_pet;Username=postgres;Password=postgres; -s src/ProjectPet.Web/ProjectPet.Web.csproj -p src/DiscussionsModule/ProjectPet.DiscussionsModule.Infrastructure --context WriteDbContext
PAUSE
