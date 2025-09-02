using MediatR;
using System;

namespace MotoRent.Application.Commands.Motorcycles.UpdateMotorcycleLicensePlate
{
	public record UpdateMotorcycleLicensePlateCommand(Guid Id, string LicensePlate) : IRequest<bool>;
}
