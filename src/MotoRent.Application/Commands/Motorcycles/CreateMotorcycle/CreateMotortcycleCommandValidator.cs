using FluentValidation;
using MotoRent.Domain.Common;
using MotoRent.Domain.Interfaces;
using System;

namespace MotoRent.Application.Commands.Motorcycles.CreateMotorcycle
{
	public class CreateMotortcycleCommandValidator : AbstractValidator<CreateMotortcycleCommand>
	{
		public CreateMotortcycleCommandValidator(IMotorcycleRepository motorcycleRepository)
		{
			RuleFor(x => x.Identifier)
				.NotEmpty()
				.OverridePropertyName("Identificador")
				.WithMessage("Identificador é obrigatório");

			RuleFor(x => x.Year)
				.NotEmpty()
				.OverridePropertyName("Ano")
				.WithMessage("Ano é obrigatório")
				.InclusiveBetween(1900, DateTime.UtcNow.Year)
				.WithMessage($"Ano deve estar entre 1900 e {DateTime.UtcNow.Year}");

			RuleFor(x => x.Model)
				.NotEmpty()
				.OverridePropertyName("Modelo")
				.WithMessage("Modelo é obrigatório");

			RuleFor(x => x.Plate)
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
