using MediatR;
using MotoRent.Domain.Interfaces;
using MotoRent.Domain.Services;
using System.Threading;
using System.Threading.Tasks;

namespace MotoRent.Application.Commands.LeaseContracts.UpdateLeaseContractEndContract
{
	public class UpdateLeaseContractEndContractCommandHandler : IRequestHandler<UpdateLeaseContractEndContractCommand, bool>
	{
		private readonly ILeaseContractRepository leaseContractRepository;

		public UpdateLeaseContractEndContractCommandHandler(ILeaseContractRepository leaseContractRepository)
		{
			this.leaseContractRepository = leaseContractRepository;
		}

		public async Task<bool> Handle(UpdateLeaseContractEndContractCommand request, CancellationToken cancellationToken)
		{
			var leaseContract = await leaseContractRepository.FindByIdAsync(request.Id, cancellationToken);

			var (amount, fine) = LeaseBillingService.CloseContract(
				leaseContract!.Plan,
				leaseContract!.StartDate,
				leaseContract!.ExpectedEndDate,
				request.DeliveryDate
			);

			return await leaseContractRepository.UpdateEndOfContract(request.Id, request.DeliveryDate, amount, fine, cancellationToken);
		}
	}
}
