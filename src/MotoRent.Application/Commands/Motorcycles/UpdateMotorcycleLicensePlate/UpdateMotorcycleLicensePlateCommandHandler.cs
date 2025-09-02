using MediatR;
using MotoRent.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MotoRent.Application.Commands.Motorcycles.UpdateMotorcycleLicensePlate
{
	public class UpdateMotorcycleLicensePlateCommandHandler : IRequestHandler<UpdateMotorcycleLicensePlateCommand, bool>
	{
		private readonly IMotorcycleRepository _motorcycleRepository;

		public UpdateMotorcycleLicensePlateCommandHandler(IMotorcycleRepository motorcycleRepository)
		{
			_motorcycleRepository = motorcycleRepository;
		}

		public async Task<bool> Handle(UpdateMotorcycleLicensePlateCommand request, CancellationToken cancellationToken)
		{
			return await _motorcycleRepository.UpdateLicensePlate(request.Id, request.LicensePlate, cancellationToken);
		}
	}
}
