using MediatR;
using MotoRent.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MotoRent.Application.Commands.Couriers.UpdateCourierDriverLicenseImage
{
	public class UpdateCourierDriverLicenseImageCommandHandler : IRequestHandler<UpdateCourierDriverLicenseImageCommand, bool>
	{
		private readonly ICourierRepository courierRepository;
		private readonly IStorageService storageService;

		public UpdateCourierDriverLicenseImageCommandHandler(ICourierRepository courierRepository, IStorageService storageService)
		{
			this.courierRepository = courierRepository;
			this.storageService = storageService;
		}

		public async Task<bool> Handle(UpdateCourierDriverLicenseImageCommand request, CancellationToken cancellationToken)
		{
			var courier = await courierRepository.FindByIdAsync(request.Id, cancellationToken);
			if (courier == null)
				return false;

			string? oldPath = courier.DriverLicenseImage;
			string? newPath = null;

			try
			{
				newPath = await storageService.UploadAsync(
					request.FileStream,
					request.FileName,
					request.ContentType,
					cancellationToken);


				bool updated = await courierRepository.UpdateImagem(request.Id, newPath, cancellationToken);
				if (!updated)
				{
					await storageService.DeleteAsync(newPath, cancellationToken);
					return false;
				}

				if (!string.IsNullOrEmpty(oldPath))
				{
					await storageService.DeleteAsync(oldPath, cancellationToken);
				}

				return true;
			}
			catch
			{
				if (!string.IsNullOrEmpty(newPath))
				{
					await storageService.DeleteAsync(newPath, cancellationToken);
				}
				throw;
			}
		}

	}
}
