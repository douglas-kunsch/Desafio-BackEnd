using FluentValidation;
using MotoRent.Domain.Interfaces;
using MotoRent.Domain.Services;
using System;

namespace MotoRent.Application.Commands.LeaseContracts.CreateLeaseContract
{
	public class CreateLeaseContractCommandValidator : AbstractValidator<CreateLeaseContractCommand>
	{
		public CreateLeaseContractCommandValidator(
			ICourierRepository courierRepository,
			IMotorcycleRepository motorcycleRepository)
		{
			RuleFor(x => x.CourierId)
				.NotEmpty()
				.OverridePropertyName("Entregador_Id")
				.WithMessage("Entregador_Id é obrigatório")
				.MustAsync(async (courierId, ct) =>
				{
					var courier = await courierRepository.FindByIdAsync(courierId, ct);
					return courier != null;
				})
				.WithMessage("Entregador não encontrado")
				.DependentRules(() =>
				{
					RuleFor(x => x.CourierId)
						.MustAsync(async (courierId, ct) =>
						{
							var courier = await courierRepository.FindByIdAsync(courierId, ct);
							return courier != null &&
								   (courier.DriverLicenseType == "A" || courier.DriverLicenseType == "A+B");
						})
						.WithMessage("Somente entregadores habilitados na categoria A podem efetuar uma locação");
				});

			RuleFor(x => x.MotorcycleId)
				.NotEmpty()
				.OverridePropertyName("Moto_Id")
				.WithMessage("Moto_Id é obrigatório")
				.MustAsync(async (motoId, ct) =>
				{
					return await motorcycleRepository.ExistsByIdAsync(motoId, ct);
				})
				.WithMessage("Moto não encontrada");

			RuleFor(x => x.StartDate)
				.NotEmpty()
				.OverridePropertyName("Data_Inicio")
				.WithMessage("Data_Inicio é obrigatória")
				.Must(date => date.Date == DateTime.UtcNow.Date.AddDays(1))
				.WithMessage("Data_Inicio deve ser o primeiro dia após a data de criação");

			RuleFor(x => x.EndDate)
				.NotEmpty()
				.OverridePropertyName("Data_Termino")
				.WithMessage("Data_Termino é obrigatória")
				.Must((command, endDate) =>
					endDate.Date == command.StartDate.Date.AddDays(command.Plan))
				.WithMessage("Data_Termino deve ser igual a Data_Inicio + o número de dias do plano");

			RuleFor(x => x.ExpectedEndDate)
				.NotEmpty()
				.OverridePropertyName("Data_Previsao_Termino")
				.WithMessage("Data_Previsao_Termino é obrigatória")
				.Must((command, expectedEndDate) =>
					expectedEndDate.Date == command.StartDate.Date.AddDays(command.Plan))
				.WithMessage("Data_Previsao_Termino deve ser igual a Data_Inicio + o número de dias do plano");

			RuleFor(x => x.Plan)
				.NotEmpty()
				.OverridePropertyName("Plano")
				.WithMessage("Plano é obrigatório")
				.Must(plan =>
				{
					try
					{
						LeasePlan.GetDailyRate(plan);
						return true;
					}
					catch
					{
						return false;
					}
				})
				.WithMessage("Plano inválido. Os planos válidos são 7, 15, 30, 45 ou 50 dias");
		}
	}
}
