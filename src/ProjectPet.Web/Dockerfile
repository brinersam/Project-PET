# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ProjectPet.Web/ProjectPet.Web.csproj", "src/ProjectPet.Web/"]
COPY ["src/AccountsModule/ProjectPet.AccountsModule.Infrastructure/ProjectPet.AccountsModule.Infrastructure.csproj", "src/AccountsModule/ProjectPet.AccountsModule.Infrastructure/"]
COPY ["src/Core/ProjectPet.Core/ProjectPet.Core.csproj", "src/Core/ProjectPet.Core/"]
COPY ["src/Core/ProjectPet.SharedKernel/ProjectPet.SharedKernel.csproj", "src/Core/ProjectPet.SharedKernel/"]
COPY ["src/Core/ProjectPet.Framework/ProjectPet.Framework.csproj", "src/Core/ProjectPet.Framework/"]
COPY ["src/AccountsModule/ProjectPet.AccountsModule.Application/ProjectPet.AccountsModule.Application.csproj", "src/AccountsModule/ProjectPet.AccountsModule.Application/"]
COPY ["src/AccountsModule/ProjectPet.AccountsModule.Contracts/ProjectPet.AccountsModule.Contracts.csproj", "src/AccountsModule/ProjectPet.AccountsModule.Contracts/"]
COPY ["src/AccountsModule/ProjectPet.AccountsModule.Domain/ProjectPet.AccountsModule.Domain.csproj", "src/AccountsModule/ProjectPet.AccountsModule.Domain/"]
COPY ["src/AccountsModule/ProjectPet.AccountsModule.Presentation/ProjectPet.AccountsModule.Presentation.csproj", "src/AccountsModule/ProjectPet.AccountsModule.Presentation/"]
COPY ["src/DiscussionsModule/ProjectPet.DiscussionsModule.Infrastructure/ProjectPet.DiscussionsModule.Infrastructure.csproj", "src/DiscussionsModule/ProjectPet.DiscussionsModule.Infrastructure/"]
COPY ["src/DiscussionsModule/ProjectPet.DiscussionsModule.Application/ProjectPet.DiscussionsModule.Application.csproj", "src/DiscussionsModule/ProjectPet.DiscussionsModule.Application/"]
COPY ["src/DiscussionsModule/ProjectPet.DiscussionsModule.Contracts/ProjectPet.DiscussionsModule.Contracts.csproj", "src/DiscussionsModule/ProjectPet.DiscussionsModule.Contracts/"]
COPY ["src/DiscussionsModule/ProjectPet.DiscussionsModule.Domain/ProjectPet.DiscussionsModule.Domain.csproj", "src/DiscussionsModule/ProjectPet.DiscussionsModule.Domain/"]
COPY ["src/DiscussionsModule/ProjectPet.DiscussionsModule.Presentation/ProjectPet.DiscussionsModule.Presentation.csproj", "src/DiscussionsModule/ProjectPet.DiscussionsModule.Presentation/"]
COPY ["src/FileManagement/ProjectPet.FileManagement.Infrastructure/ProjectPet.FileManagement.Infrastructure.csproj", "src/FileManagement/ProjectPet.FileManagement.Infrastructure/"]
COPY ["src/FileManagement/ProjectPet.FileManagement.Application/ProjectPet.FileManagement.Application.csproj", "src/FileManagement/ProjectPet.FileManagement.Application/"]
COPY ["src/Species/ProjectPet.Species.Contracts/ProjectPet.SpeciesModule.Contracts.csproj", "src/Species/ProjectPet.Species.Contracts/"]
COPY ["src/Species/ProjectPet.Species.Domain/ProjectPet.SpeciesModule.Domain.csproj", "src/Species/ProjectPet.Species.Domain/"]
COPY ["src/FileManagement/ProjectPet.FileManagement.Presentation/ProjectPet.FileManagement.Presentation.csproj", "src/FileManagement/ProjectPet.FileManagement.Presentation/"]
COPY ["src/Species/ProjectPet.Species.Infrastructure/ProjectPet.SpeciesModule.Infrastructure.csproj", "src/Species/ProjectPet.Species.Infrastructure/"]
COPY ["src/Species/ProjectPet.Species.Application/ProjectPet.SpeciesModule.Application.csproj", "src/Species/ProjectPet.Species.Application/"]
COPY ["src/Volunteer/ProjectPet.Volunteer.Contracts/ProjectPet.VolunteerModule.Contracts.csproj", "src/Volunteer/ProjectPet.Volunteer.Contracts/"]
COPY ["src/Species/ProjectPet.Species.Presentation/ProjectPet.SpeciesModule.Presentation.csproj", "src/Species/ProjectPet.Species.Presentation/"]
COPY ["src/VolunteerRequest/ProjectPet.VolunteerRequest.Infrastructure/ProjectPet.VolunteerRequests.Infrastructure.csproj", "src/VolunteerRequest/ProjectPet.VolunteerRequest.Infrastructure/"]
COPY ["src/VolunteerRequest/ProjectPet.VolunteerRequest.Application/ProjectPet.VolunteerRequests.Application.csproj", "src/VolunteerRequest/ProjectPet.VolunteerRequest.Application/"]
COPY ["src/VolunteerRequest/ProjectPet.VolunteerRequest.Contracts/ProjectPet.VolunteerRequests.Contracts.csproj", "src/VolunteerRequest/ProjectPet.VolunteerRequest.Contracts/"]
COPY ["src/VolunteerRequest/ProjectPet.VolunteerRequest.Domain/ProjectPet.VolunteerRequests.Domain.csproj", "src/VolunteerRequest/ProjectPet.VolunteerRequest.Domain/"]
COPY ["src/VolunteerRequest/ProjectPet.VolunteerRequest.Presentation/ProjectPet.VolunteerRequests.Presentation.csproj", "src/VolunteerRequest/ProjectPet.VolunteerRequest.Presentation/"]
COPY ["src/Volunteer/ProjectPet.Volunteer.Infrastructure/ProjectPet.VolunteerModule.Infrastructure.csproj", "src/Volunteer/ProjectPet.Volunteer.Infrastructure/"]
COPY ["src/Volunteer/ProjectPet.Volunteer.Application/ProjectPet.VolunteerModule.Application.csproj", "src/Volunteer/ProjectPet.Volunteer.Application/"]
COPY ["src/Volunteer/ProjectPet.Volunteer.Domain/ProjectPet.VolunteerModule.Domain.csproj", "src/Volunteer/ProjectPet.Volunteer.Domain/"]
COPY ["src/Volunteer/ProjectPet.Volunteer.Presentation/ProjectPet.VolunteerModule.Presentation.csproj", "src/Volunteer/ProjectPet.Volunteer.Presentation/"]
RUN dotnet restore "./src/ProjectPet.Web/ProjectPet.Web.csproj"
COPY . .
WORKDIR "/src/src/ProjectPet.Web"
RUN dotnet build "./ProjectPet.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ProjectPet.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectPet.Web.dll"]