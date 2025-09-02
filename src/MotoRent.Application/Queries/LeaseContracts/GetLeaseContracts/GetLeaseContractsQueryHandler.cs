using MediatR;
using MotoRent.Domain.Entities;
using MotoRent.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MotoRent.Application.Queries.LeaseContracts.GetLeaseContracts
{
	public class GetLeaseContractsQueryHandler : IRequestHandler<GetLeaseContractsQuery, IEnumerable<LeaseContract>>
	{
		private readonly ILeaseContractRepository leaseContractRepository;

		public GetLeaseContractsQueryHandler(ILeaseContractRepository leaseContractRepository)
		{
			this.leaseContractRepository = leaseContractRepository;
		}

		public async Task<IEnumerable<LeaseContract>> Handle(GetLeaseContractsQuery request, CancellationToken cancellationToken)
		{
			var leaseContracts = await leaseContractRepository.FindAsync(request.PlanType, cancellationToken);

			return leaseContracts;
		}
	}
}
