using MediatR;
using System;

namespace MotoRent.Application.Commands.LeaseContracts.CreateLeaseContract
{
	public record CreateLeaseContractCommand(Guid CourierId, Guid MotorcycleId, DateTime StartDate, DateTime EndDate, DateTime ExpectedEndDate, int Plan) : IRequest<Guid>;
}
