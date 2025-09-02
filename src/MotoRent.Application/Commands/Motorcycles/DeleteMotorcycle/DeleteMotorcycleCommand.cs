using MediatR;
using System;

namespace MotoRent.Application.Commands.Motorcycles.DeleteMotorcycle
{
	public record DeleteMotorcycleCommand(Guid Id) : IRequest<bool>;
}
