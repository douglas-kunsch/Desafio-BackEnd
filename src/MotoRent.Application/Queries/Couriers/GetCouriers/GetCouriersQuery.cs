using MediatR;
using MotoRent.Domain.Entities;
using System.Collections.Generic;

namespace MotoRent.Application.Queries.Couriers.GetCouriers
{
	public record GetCouriersQuery(string? DriverLicenseType) : IRequest<IEnumerable<Courier>>;
}
