using MediatR;
using MotoRent.Domain.Entities;
using System;

namespace MotoRent.Application.Queries.LeaseContracts.GetLeaseContractById
{
	public record GetLeaseContractByIdQuery(Guid Id) : IRequest<LeaseContract?>;
}
