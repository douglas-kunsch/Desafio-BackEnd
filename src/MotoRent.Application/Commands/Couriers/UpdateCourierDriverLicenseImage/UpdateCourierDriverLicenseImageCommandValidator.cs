using FluentValidation;
using MotoRent.Application.Common.Validation;
using MotoRent.Domain.Interfaces;
using System.IO;
using System.Linq;

namespace MotoRent.Application.Commands.Couriers.UpdateCourierDriverLicenseImage
{
	public class UpdateCourierDriverLicenseImageCommandValidator
		: AbstractValidator<UpdateCourierDriverLicenseImageCommand>
	{
		public UpdateCourierDriverLicenseImageCommandValidator(ICourierRepository courierRepository)
		{
			RuleFor(x => x.Id)
				.NotEmpty().WithMessage("Id é obrigatório")
				.MustAsync(async (id, ct) =>
				{
					bool exists = await courierRepository.ExistsByIdAsync(id, ct);
					return exists;
				}).WithMessage("Entregador não encontrado");

			RuleFor(x => x.FileStream)
				.NotNull().WithMessage("Imagem da CNH é obrigatória")
				.Must(fs => fs.Length > 0).WithMessage("Arquivo de imagem da CNH não pode estar vazio")
				.Must(fs => fs.Length <= FileValidationConstants.MaxImageFileSizeBytes)
				.WithMessage($"Imagem da CNH deve ter no máximo {FileValidationConstants.MaxImageFileSizeBytes / 1024 / 1024}MB");

			RuleFor(x => x.FileName)
				.NotEmpty().WithMessage("Nome do arquivo da CNH é obrigatório")
				.Must(fileName => FileValidationConstants.AllowedImageExtensions.Contains(Path.GetExtension(fileName).ToLowerInvariant()))
				.WithMessage($"Imagem da CNH deve estar nos formatos: {string.Join(", ", FileValidationConstants.AllowedImageExtensions)}");

			RuleFor(x => x.ContentType)
				.NotEmpty().WithMessage("Content-Type da imagem é obrigatório")
				.Must(ct => FileValidationConstants.AllowedImageContentTypes.Contains(ct.ToLowerInvariant()))
				.WithMessage($"Imagem da CNH deve ter Content-Type: {string.Join(", ", FileValidationConstants.AllowedImageContentTypes)}");
		}
	}
}
