using MediatR;
using System;

namespace MotoRent.Application.Commands.LeaseContracts.UpdateLeaseContractEndContract
{
	public record UpdateLeaseContractEndContractCommand(Guid Id, DateTime DeliveryDate) : IRequest<bool>;
}
