using FluentValidation;
using MotoRent.Domain.Common; // importa PlateValidator
using MotoRent.Domain.Interfaces;

namespace MotoRent.Application.Commands.Motorcycles.UpdateMotorcycleLicensePlate
{
	public class UpdateMotorcycleLicensePlateCommandValidator : AbstractValidator<UpdateMotorcycleLicensePlateCommand>
	{
		public UpdateMotorcycleLicensePlateCommandValidator(IMotorcycleRepository motorcycleRepository)
		{
			RuleFor(x => x.Id)
				.NotEmpty()
				.WithMessage("Id é obrigatório")
				.MustAsync(async (id, ct) =>
				{
					bool exists = await motorcycleRepository.ExistsByIdAsync(id, ct);
					return exists;
				})
				.WithMessage("Moto não encontrada");

			RuleFor(x => x.LicensePlate)
				.NotEmpty()
				.OverridePropertyName("Placa")
				.WithMessage("Placa é obrigatória")
				.Must(plate => PlateValidator.IsValid(plate?.Trim().ToUpperInvariant()))
				.WithMessage("Placa deve estar no formato válido (AAA-0000 ou AAA0A00)")
				.MustAsync(async (plate, ct) =>
				{
					string? normalized = plate?.Trim().ToUpperInvariant();
					bool exists = await motorcycleRepository.ExistsByLicensePlateAsync(normalized!, ct);
					return !exists;
				})
				.WithMessage("Placa já está cadastrada");
		}
	}
}
