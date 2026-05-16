# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ScanJobWorker.sln .
COPY ScanJobWorker.Domain/ScanJobWorker.Domain.csproj           ScanJobWorker.Domain/
COPY ScanJobWorker.Application/ScanJobWorker.Application.csproj ScanJobWorker.Application/
COPY ScanJobWorker.Infrastructure/ScanJobWorker.Infrastructure.csproj ScanJobWorker.Infrastructure/
COPY ScanJobWorker.Worker/ScanJobWorker.Worker.csproj           ScanJobWorker.Worker/

RUN dotnet restore

COPY . .
RUN dotnet publish ScanJobWorker.Worker/ScanJobWorker.Worker.csproj \
    -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ScanJobWorker.Worker.dll"]