using MediatR;
using System;

namespace MotoRent.Application.Commands.Motorcycles.CreateMotorcycle
{
	public record CreateMotortcycleCommand(int Year, string Model, string Plate, string Identifier) : IRequest<Guid>;
}
