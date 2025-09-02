using MediatR;
using MotoRent.Domain.Entities;
using MotoRent.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MotoRent.Application.Queries.Motorcycles.GetMotorcycles
{
	public class GetMotorcyclesQueryHandler : IRequestHandler<GetMotorcyclesQuery, IEnumerable<Motorcycle>>
	{
		private readonly IMotorcycleRepository motorcycleRepository;

		public GetMotorcyclesQueryHandler(IMotorcycleRepository motorcycleRepository)
		{
			this.motorcycleRepository = motorcycleRepository;
		}

		public async Task<IEnumerable<Motorcycle>> Handle(GetMotorcyclesQuery request, CancellationToken cancellationToken)
		{
			var motortcycle = await motorcycleRepository.FindAsync(request.Plate, cancellationToken);

			return motortcycle;
		}
	}
}
