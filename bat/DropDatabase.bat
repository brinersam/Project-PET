cd ../src
dotnet ef database update 0 --connection Host=localhost;Port=5432;Database=project_pet;Username=postgres;Password=postgres; -s ProjectPet.Api -p ProjectPet.Infrastructure