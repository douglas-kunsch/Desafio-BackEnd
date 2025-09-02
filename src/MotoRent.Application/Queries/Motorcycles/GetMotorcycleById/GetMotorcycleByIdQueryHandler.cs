using MediatR;
using MotoRent.Domain.Entities;
using MotoRent.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MotoRent.Application.Queries.Motorcycles.GetMotorcycleById
{
	public class GetMotorcycleByIdQueryHandler : IRequestHandler<GetMotorcycleByIdQuery, Motorcycle?>
	{
		private readonly IMotorcycleRepository motorcycleRepository;

		public GetMotorcycleByIdQueryHandler(IMotorcycleRepository motorcycleRepository)
		{
			this.motorcycleRepository = motorcycleRepository;
		}

		public async Task<Motorcycle?> Handle(GetMotorcycleByIdQuery request, CancellationToken cancellationToken)
		{
			var motortcycle = await motorcycleRepository.FindByIdAsync(request.Id, cancellationToken);

			return motortcycle;
		}
	}
}
