using Microsoft.EntityFrameworkCore;
using MotoRent.Api.Config;
using MotoRent.Api.Endpoints;
using MotoRent.Api.Middleware;
using MotoRent.Application;
using MotoRent.Infrastructure;
using MotoRent.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;

builder.Configuration
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
	.AddEnvironmentVariables();

builder.AddLoggingConfiguration();

builder.Services.AddApplication();
builder.Services.AddInfraservices(builder.Configuration);
builder.Services.AddHealthChecksConfiguration(builder.Configuration);

builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

	if (env.IsDevelopment())
	{
		try
		{
			db.Database.Migrate();
			logger.LogInformation("Migrations aplicadas automaticamente no ambiente de desenvolvimento.");
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Erro ao aplicar migrations automaticamente.");
			throw;
		}
	}
	else
	{
		logger.LogInformation("Ambiente {Environment} detectado, migrations devem ser aplicadas manualmente.", env.EnvironmentName);
	}
}

app.UseMiddleware<ValidationExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
	app.UseSwaggerDocumentation();
}

app.MapRootEndpoints();
app.MapMotorcycleEndpoints();
app.MapCourierEndpoints();
app.MapLeaseContractEndpoints();
app.MapHealthEndpoints();

app.Run();
