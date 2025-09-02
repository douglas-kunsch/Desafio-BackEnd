using MediatR;
using MotoRent.Domain.Entities;
using MotoRent.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MotoRent.Application.Queries.Couriers.GetCourierById
{
	public class GetCourierByIdQueryHandler : IRequestHandler<GetCourierByIdQuery, Courier?>
	{
		private readonly ICourierRepository courierRepository;

		public GetCourierByIdQueryHandler(ICourierRepository courierRepository)
		{
			this.courierRepository = courierRepository;
		}

		public async Task<Courier?> Handle(GetCourierByIdQuery request, CancellationToken cancellationToken)
		{
			var courier = await courierRepository.FindByIdAsync(request.Id, cancellationToken);

			return courier;
		}
	}
}
