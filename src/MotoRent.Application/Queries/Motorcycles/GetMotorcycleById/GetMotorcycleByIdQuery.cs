using MediatR;
using MotoRent.Domain.Entities;
using System;

namespace MotoRent.Application.Queries.Motorcycles.GetMotorcycleById
{
	public record GetMotorcycleByIdQuery(Guid Id) : IRequest<Motorcycle?>;
}
