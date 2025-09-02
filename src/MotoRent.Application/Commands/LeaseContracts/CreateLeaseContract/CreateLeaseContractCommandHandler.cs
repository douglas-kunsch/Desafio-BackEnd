using MediatR;
using MotoRent.Domain.Entities;
using MotoRent.Domain.Interfaces;
using MotoRent.Domain.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MotoRent.Application.Commands.LeaseContracts.CreateLeaseContract
{
	public class CreateLeaseContractCommandHandler : IRequestHandler<CreateLeaseContractCommand, Guid>
	{
		private readonly ILeaseContractRepository leaseContractRepository;

		public CreateLeaseContractCommandHandler(ILeaseContractRepository leaseContractRepository)
		{
			this.leaseContractRepository = leaseContractRepository;
		}

		public async Task<Guid> Handle(CreateLeaseContractCommand request, CancellationToken cancellationToken)
		{
			decimal expectedAmount = LeasePricingService.CalculateCost(request.Plan, request.StartDate, request.ExpectedEndDate);

			var leaseContract = new LeaseContract()
			{
				MotorcycleId = request.MotorcycleId,
				CourierId = request.CourierId,
				StartDate = request.StartDate,
				EndDate = request.EndDate,
				ExpectedEndDate = request.ExpectedEndDate,
				ExpectedAmount = expectedAmount,
				Plan = request.Plan,
				IsActive = true,
			};

			await leaseContractRepository.AddAsync(leaseContract, cancellationToken);

			return leaseContract.Id;
		}
	}
}
