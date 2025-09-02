using MediatR;
using MotoRent.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MotoRent.Application.Commands.Motorcycles.DeleteMotorcycle
{
	public class DeleteMotorcycleCommandHandler : IRequestHandler<DeleteMotorcycleCommand, bool>
	{
		private readonly IMotorcycleRepository _motorcycleRepository;

		public DeleteMotorcycleCommandHandler(IMotorcycleRepository motorcycleRepository)
		{
			_motorcycleRepository = motorcycleRepository;
		}

		public async Task<bool> Handle(DeleteMotorcycleCommand request, CancellationToken cancellationToken)
		{
			return await _motorcycleRepository.DeleteByIdAsync(request.Id, cancellationToken);
		}
	}
}
