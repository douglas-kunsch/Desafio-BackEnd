using Microsoft.Extensions.Options;
using MotoRent.Consumers;
using MotoRent.Infrastructure;
using MotoRent.Infrastructure.Common.Option;
using RabbitMQ.Client;

namespace MotoRent.Worker
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var host = Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((ctx, config) =>
				{
					var env = ctx.HostingEnvironment;

					config.SetBasePath(Directory.GetCurrentDirectory())
						  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
						  .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
						  .AddEnvironmentVariables()
						  .AddCommandLine(args);
				})
				.ConfigureServices((ctx, services) =>
				{
					var configuration = ctx.Configuration;
					services.AddInfraservices(configuration);

					services.AddSingleton(sp =>
					{
						var options = sp.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
						return new ConnectionFactory
						{
							Uri = new(configuration["RabbitMQ:ConnectionString"])
						};
					});

					services.AddHostedService<MotorcycleRegisteredConsumer>();
				})
				.ConfigureLogging(logging =>
				{
					logging.ClearProviders();
					logging.AddConsole();
				})
				.Build();

			await host.RunAsync();
		}
	}
}
