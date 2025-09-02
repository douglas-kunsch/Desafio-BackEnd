using Serilog;

namespace MotoRent.Api.Config
{
	public static class ConfigureLoggingServices
	{
		public static WebApplicationBuilder AddLoggingConfiguration(this WebApplicationBuilder builder)
		{
			// Configura o Serilog a partir do appsettings.json e adiciona alguns enrichers úteis
			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(builder.Configuration)
				.Enrich.FromLogContext()
				.Enrich.WithMachineName()
				.Enrich.WithThreadId()
				.WriteTo.Console()
				.CreateLogger();

			builder.Host.UseSerilog();

			return builder;
		}
	}
}
