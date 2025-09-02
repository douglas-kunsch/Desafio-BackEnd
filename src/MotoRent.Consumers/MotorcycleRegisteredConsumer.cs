using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MotoRent.Domain.Entities;
using MotoRent.Domain.Events;
using MotoRent.Domain.Interfaces;
using MotoRent.Infrastructure.Common.Option;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MotoRent.Consumers
{
	public class MotorcycleRegisteredConsumer : BackgroundService
	{
		private readonly ILogger<MotorcycleRegisteredConsumer> _logger;
		private readonly RabbitMqOptions _options;
		private readonly ConnectionFactory _factory;
		private readonly IServiceScopeFactory _scopeFactory;
		private IConnection? _connection;
		private IModel? _channel;

		public MotorcycleRegisteredConsumer(
			ILogger<MotorcycleRegisteredConsumer> logger,
			IOptions<RabbitMqOptions> options,
			ConnectionFactory factory,
			IServiceScopeFactory scopeFactory)
		{
			_logger = logger;
			_options = options.Value;
			_factory = factory;
			_scopeFactory = scopeFactory;
		}

		public override Task StartAsync(CancellationToken cancellationToken)
		{
			_connection = _factory.CreateConnection();
			_channel = _connection.CreateModel();

			_channel.QueueDeclare(
				queue: _options.Queues.MotorcycleRegistered,
				durable: true,
				exclusive: false,
				autoDelete: false,
				arguments: null);

			_logger.LogInformation("Consumer connected to RabbitMQ, queue: {Queue}", _options.Queues.MotorcycleRegistered);

			return base.StartAsync(cancellationToken);
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var consumer = new EventingBasicConsumer(_channel!);
			consumer.Received += async (model, ea) =>
			{
				try
				{
					string json = Encoding.UTF8.GetString(ea.Body.ToArray());
					var @event = JsonSerializer.Deserialize<MotorcycleRegisteredEvent>(json);

					if (@event != null)
					{
						_logger.LogInformation("Received event: {Id} {Year}", @event.MotorcycleId, @event.Year);

						if (@event.Year == 2024)
						{
							using var scope = _scopeFactory.CreateScope();
							var repo = scope.ServiceProvider.GetRequiredService<IConsumedMessageRepository>();

							var message = new ConsumedMessage
							{
								EventType = nameof(MotorcycleRegisteredEvent),
								Payload = json
							};

							await repo.AddAsync(message, stoppingToken);
							_logger.LogInformation("Stored event for Motorcycle {Id}", @event.MotorcycleId);
						}
					}

					_channel!.BasicAck(ea.DeliveryTag, false);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Error processing message");
				}
			};

			_channel!.BasicConsume(
				queue: _options.Queues.MotorcycleRegistered,
				autoAck: false,
				consumer: consumer);

			return Task.CompletedTask;
		}

		public override void Dispose()
		{
			_channel?.Close();
			_connection?.Close();
			base.Dispose();
		}
	}
}
