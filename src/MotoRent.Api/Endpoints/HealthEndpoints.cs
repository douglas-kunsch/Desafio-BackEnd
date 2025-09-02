using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

public static class HealthEndpoints
{
	public static void MapHealthEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("/health")
					   .WithTags("health");


		group.MapGet("/", () => Results.Ok(new { status = "Healthy" }))
			 .WithName("HealthCheckSimple")
			 .Produces<object>(StatusCodes.Status200OK);

		group.MapGet("/details", async (HttpContext context, HealthCheckService healthChecks) =>
			{
				var report = await healthChecks.CheckHealthAsync();
				context.Response.ContentType = "application/json";
				await UIResponseWriter.WriteHealthCheckUIResponse(context, report);
			})
			.WithName("HealthCheckDetails")
			.WithTags("health");

		group.MapHealthChecks("/details", new HealthCheckOptions
		{
			ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
			ResultStatusCodes =
			{
				[HealthStatus.Healthy] = StatusCodes.Status200OK,
				[HealthStatus.Degraded] = StatusCodes.Status200OK,
				[HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
			}
		})
		.WithName("HealthCheckDetailed");
	}
}
