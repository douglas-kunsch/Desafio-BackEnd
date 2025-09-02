using MediatR;
using MotoRent.Domain.Entities;
using MotoRent.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MotoRent.Application.Queries.LeaseContracts.GetLeaseContractById
{
	public class GetLeaseContractByIdQueryHandler : IRequestHandler<GetLeaseContractByIdQuery, LeaseContract?>
	{
		private readonly ILeaseContractRepository leaseContracRepository;

		public GetLeaseContractByIdQueryHandler(ILeaseContractRepository leaseContracRepository)
		{
			this.leaseContracRepository = leaseContracRepository;
		}

		public async Task<LeaseContract?> Handle(GetLeaseContractByIdQuery request, CancellationToken cancellationToken)
		{
			var leaseContracts = await leaseContracRepository.FindByIdAsync(request.Id, cancellationToken);

			return leaseContracts;
		}
	}
}
