using MediatR;
using MotoRent.Domain.Entities;
using System;

namespace MotoRent.Application.Queries.Couriers.GetCourierById
{
	public record GetCourierByIdQuery(Guid Id) : IRequest<Courier?>;
}
