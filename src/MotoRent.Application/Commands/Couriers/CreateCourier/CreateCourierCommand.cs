using MediatR;
using System;
using System.IO;

namespace MotoRent.Application.Commands.Couriers.CreateCourier
{
	public record CreateCourierCommand(
		string Identifier,
		string Name,
		string Cnpj,
		DateTime BirthDate,
		string DriverLicenseNumber,
		string DriverLicenseType,
		Stream FileStream,
		string FileName,
		string ContentType
	) : IRequest<Guid>;
}
