using Microsoft.Extensions.Options;
using MotoRent.Domain.Interfaces;
using MotoRent.Infrastructure.Common.Option;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MotoRent.Infrastructure.Services
{
	public class PublisherService : IPublisherService
	{
		private readonly ConnectionFactory _factory;
		private readonly RabbitMqOptions _options;

		public PublisherService(ConnectionFactory factory, IOptions<RabbitMqOptions> options)
		{
			_factory = factory;
			_options = options.Value;
		}

		public Task PublishAsync<T>(string queueName, T message, CancellationToken cancellationToken = default)
		{
			using var connection = _factory.CreateConnection();
			using var channel = connection.CreateModel();

			channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);

			byte[] body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

			channel.BasicPublish(exchange: "",
								 routingKey: queueName,
								 basicProperties: null,
								 body: body);

			return Task.CompletedTask;
		}
	}
}
