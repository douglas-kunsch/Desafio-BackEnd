using MediatR;
using MotoRent.Domain.Entities;
using System.Collections.Generic;

namespace MotoRent.Application.Queries.Motorcycles.GetMotorcycles
{
	public record GetMotorcyclesQuery(string? Plate) : IRequest<IEnumerable<Motorcycle>>;
}
