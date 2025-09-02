using MediatR;
using Microsoft.Extensions.Options;
using MotoRent.Domain.Entities;
using MotoRent.Domain.Events;
using MotoRent.Domain.Interfaces;
using MotoRent.Infrastructure.Common.Option;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MotoRent.Application.Commands.Motorcycles.CreateMotorcycle
{
	public class CreateMotortcycleCommandHandler : IRequestHandler<CreateMotortcycleCommand, Guid>
	{
		private readonly IMotorcycleRepository _motorcycleRepository;
		private readonly IPublisherService _publisher;
		private readonly RabbitMqOptions _rabbitOptions;

		public CreateMotortcycleCommandHandler(IMotorcycleRepository motorcycleRepository,
			IPublisherService publisher,
			IOptions<RabbitMqOptions> rabbitOptions)
		{
			_motorcycleRepository = motorcycleRepository;
			_publisher = publisher;
			_rabbitOptions = rabbitOptions.Value;
		}

		public async Task<Guid> Handle(CreateMotortcycleCommand request, CancellationToken cancellationToken)
		{
			var motorcycle = new Motorcycle()
			{
				Model = request.Model,
				LicensePlate = request.Plate?.Trim().ToUpperInvariant(),
				Year = request.Year,
				Identifier = request.Identifier,
			};

			await _motorcycleRepository.AddAsync(motorcycle, cancellationToken);

			var motorcycleRegisteredEvent = new MotorcycleRegisteredEvent(
				motorcycle.Id,
				motorcycle.Year,
				motorcycle.Model,
				motorcycle.LicensePlate,
				motorcycle.Identifier);

			await _publisher.PublishAsync(
				_rabbitOptions.Queues.MotorcycleRegistered,
				motorcycleRegisteredEvent,
				cancellationToken);

			return motorcycle.Id;
		}
	}
}
