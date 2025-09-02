using MediatR;
using MotoRent.Domain.Entities;
using MotoRent.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MotoRent.Application.Commands.Couriers.CreateCourier
{
	public class CreateCourierCommandHandler : IRequestHandler<CreateCourierCommand, Guid>
	{
		private readonly ICourierRepository courierRepository;
		private readonly IStorageService storageService;

		public CreateCourierCommandHandler(ICourierRepository courierRepository, IStorageService storageService)
		{
			this.courierRepository = courierRepository;
			this.storageService = storageService;
		}

		public async Task<Guid> Handle(CreateCourierCommand request, CancellationToken cancellationToken)
		{
			string? path = null;

			try
			{
				path = await storageService.UploadAsync(
					request.FileStream,
					request.FileName,
					request.ContentType,
					cancellationToken);

				var courier = new Courier()
				{
					Identifier = request.Identifier,
					Name = request.Name,
					Cnpj = request.Cnpj,
					BirthDate = request.BirthDate.ToUniversalTime(),
					DriverLicenseNumber = request.DriverLicenseNumber,
					DriverLicenseType = request.DriverLicenseType,
					DriverLicenseImage = path
				};

				await courierRepository.AddAsync(courier, cancellationToken);

				return courier.Id;
			}
			catch
			{
				if (!string.IsNullOrEmpty(path))
				{
					try
					{
						await storageService.DeleteAsync(path, cancellationToken);
					}
					catch
					{

					}
				}

				throw;
			}
		}
	}
}
