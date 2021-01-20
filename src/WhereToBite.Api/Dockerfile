FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY "WhereToBite.sln" "WhereToBite.sln"

COPY "src/WhereToBite.Api/WhereToBite.Api.csproj" "src/WhereToBite.Api/WhereToBite.Api.csproj"
COPY "src/WhereToBite.Core.DataExtractor.Abstraction/WhereToBite.Core.DataExtractor.Abstraction.csproj" "src/WhereToBite.Core.DataExtractor.Abstraction/WhereToBite.Core.DataExtractor.Abstraction.csproj"
COPY "src/WhereToBite.Core.DataExtractor.Concrete/WhereToBite.Core.DataExtractor.Concrete.csproj" "src/WhereToBite.Core.DataExtractor.Concrete/WhereToBite.Core.DataExtractor.Concrete.csproj"
COPY "src/WhereToBite.Domain/WhereToBite.Domain.csproj" "src/WhereToBite.Domain/WhereToBite.Domain.csproj"
COPY "src/WhereToBite.Infrastructure/WhereToBite.Infrastructure.csproj" "src/WhereToBite.Infrastructure/WhereToBite.Infrastructure.csproj"
COPY "tests/WhereToBite.Tests/WhereToBite.Tests.csproj" "tests/WhereToBite.Tests/WhereToBite.Tests.csproj"

COPY "NuGet.config" "NuGet.config"

RUN dotnet restore "WhereToBite.sln"

COPY . .

WORKDIR /src/src/WhereToBite.Api
RUN dotnet nuget locals all --list
RUN dotnet publish --no-restore -c Release -o /app --no-cache /restore

FROM build as unittest
WORKDIR /src/tests/WhereToBite.Tests

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WhereToBite.Api.dll"]