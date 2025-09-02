using MediatR;
using MotoRent.Domain.Entities;
using System.Collections.Generic;

namespace MotoRent.Application.Queries.LeaseContracts.GetLeaseContracts
{
	public record GetLeaseContractsQuery(int? PlanType) : IRequest<IEnumerable<LeaseContract>>;
}
