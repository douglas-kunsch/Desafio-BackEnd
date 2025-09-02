using Microsoft.Extensions.Options;
using Minio;
using Minio.AspNetCore.HealthChecks;
using MotoRent.Api.HealthCheck;
using MotoRent.Infrastructure.Common.Option;

namespace MotoRent.Api.Config
{
	public static class ConfigureHealthServices
	{
		public static IServiceCollection AddHealthChecksConfiguration(
			this IServiceCollection services, IConfiguration configuration)
		{
			string? connectionString = configuration.GetConnectionString("DefaultConnection");
			services.Configure<MinioOptions>(configuration.GetSection("Minio"));

			services.AddHealthChecks()
				.AddNpgSql(
					connectionString!,
					name: "postgres",
					tags: new[] { "db", "postgres" })
				.AddRabbitMQ(
					configuration["RabbitMQ:ConnectionString"]!,
					name: "rabbitmq",
					tags: new[] { "mq", "rabbitmq" })
				.AddMinio(sp =>
				{
					var settings = sp.GetRequiredService<IOptions<MinioOptions>>().Value;

					return new Minio.MinioClient()
						.WithEndpoint(settings.Endpoint)
						.WithCredentials(settings.AccessKey, settings.SecretKey)
						.WithSSL(settings.UseSSL)
						.Build();
				},
					name: "minio",
					tags: new[] { "storage", "minio" });
			services.AddSingleton<IMinioClient>(sp =>
			{
				var settings = sp.GetRequiredService<IOptions<MinioOptions>>().Value;
				return new MinioClient()
					.WithEndpoint(settings.Endpoint)
					.WithCredentials(settings.AccessKey, settings.SecretKey)
					.WithSSL(settings.UseSSL)
					.Build();
			});

			services.AddHealthChecks()
				.AddCheck<MinioBucketHealthCheck>("minio-bucket", tags: new[] { "storage", "minio", "bucket" });

			return services;
		}
	}
}
