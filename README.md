# MotoRent - Skeleton for Mottu Backend Challenge

This repository is a skeleton showing a clean structure (Domain, Application, Infrastructure, Api, Consumers, Workers)
with both MassTransit-based consumer and low-level RabbitMQ BackgroundService workers. Tests are included with xUnit.

## How to run (dev)

1. Start dependencies:
   ```bash
   docker compose up -d
   ```
2. Apply migrations (if you implement them) or use `dotnet ef database update`.
3. Run the API:
   ```bash
   cd src/MotoRent.Api
   dotnet run
   ```
4. Access Swagger: http://localhost:8080/swagger
