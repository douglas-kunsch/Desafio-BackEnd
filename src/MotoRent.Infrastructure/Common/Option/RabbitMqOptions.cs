namespace MotoRent.Infrastructure.Common.Option
{
	public class RabbitMqOptions
	{
		public string ConnectionString { get; set; } = string.Empty;
		public string Host { get; set; } = string.Empty;
		public string User { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public RabbitMqQueues Queues { get; set; } = new();
	}

	public class RabbitMqQueues
	{
		public string MotorcycleRegistered { get; set; } = string.Empty;
	}
}
