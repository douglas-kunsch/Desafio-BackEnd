FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/MotoRent.Worker/MotoRent.Worker.csproj MotoRent.Worker/
COPY src/MotoRent.Application/MotoRent.Application.csproj MotoRent.Application/
COPY src/MotoRent.Infrastructure/MotoRent.Infrastructure.csproj MotoRent.Infrastructure/
COPY src/MotoRent.Domain/MotoRent.Domain.csproj MotoRent.Domain/
COPY src/MotoRent.Consumers/MotoRent.Consumers.csproj MotoRent.Consumers/

RUN dotnet restore MotoRent.Worker/MotoRent.Worker.csproj

COPY src/ .

WORKDIR /src/MotoRent.Worker
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MotoRent.Worker.dll"]
