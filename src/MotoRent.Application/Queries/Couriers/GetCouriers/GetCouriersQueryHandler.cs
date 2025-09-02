using MediatR;
using MotoRent.Domain.Entities;
using MotoRent.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MotoRent.Application.Queries.Couriers.GetCouriers
{
	public class GetCouriersQueryHandler : IRequestHandler<GetCouriersQuery, IEnumerable<Courier>>
	{
		private readonly ICourierRepository courierRepository;

		public GetCouriersQueryHandler(ICourierRepository courierRepository)
		{
			this.courierRepository = courierRepository;
		}

		public async Task<IEnumerable<Courier>> Handle(GetCouriersQuery request, CancellationToken cancellationToken)
		{
			var couriers = await courierRepository.FindAsync(request.DriverLicenseType, cancellationToken);

			return couriers;
		}
	}
}
