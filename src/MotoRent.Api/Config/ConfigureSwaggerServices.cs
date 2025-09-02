namespace MotoRent.Api.Config
{
	public static class ConfigureSwaggerServices
	{
		public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
		{
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
				{
					Title = "Sistema de Manutenção de Motos",
					Version = "v1",
					Description = "API para gerenciamento de motos, entregadores e locações"
				});
			});

			return services;
		}

		public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
		{
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "MotoRent API v1");
				c.RoutePrefix = "swagger";
			});

			return app;
		}
	}
}
