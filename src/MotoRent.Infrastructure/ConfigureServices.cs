using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
using MotoRent.Domain.Interfaces;
using MotoRent.Infrastructure.Common.Option;
using MotoRent.Infrastructure.Context;
using MotoRent.Infrastructure.Repositories;
using MotoRent.Infrastructure.Services;
using RabbitMQ.Client;
using System;

namespace MotoRent.Infrastructure
{
	public static class ConfigureServices
	{
		public static IServiceCollection AddInfraservices(this IServiceCollection services, IConfiguration configuration)
		{
			string cs = configuration.GetConnectionString("DefaultConnection")!;
			services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(cs,
			sqlOptions => sqlOptions.MigrationsHistoryTable(
					"__EFMigrationsHistory",
					"MotoRent"
				)));

			services.Configure<MinioOptions>(configuration.GetSection("Minio"));

			services.AddSingleton<IMinioClient>(sp =>
			{
				var opts = configuration.GetSection("Minio").Get<MinioOptions>()!;
				return new MinioClient()
					.WithEndpoint(opts.Endpoint)
					.WithCredentials(opts.AccessKey, opts.SecretKey)
					.Build();
			});


			services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
			services.AddScoped<ICourierRepository, CourierRepository>();
			services.AddScoped<ILeaseContractRepository, LeaseContractRepository>();
			services.AddScoped<IConsumedMessageRepository, ConsumedMessageRepository>();
			services.AddScoped<IStorageService, StorageService>();

			services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMQ"));

			services.AddSingleton(sp =>
			{
				var opts = sp.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
				return new ConnectionFactory
				{
					HostName = opts.Host,
					UserName = opts.User,
					Password = opts.Password,
					AutomaticRecoveryEnabled = true,
					NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
				};
			});

			services.AddScoped<IPublisherService, PublisherService>();

			return services;
		}
	}
}
