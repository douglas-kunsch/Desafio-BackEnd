using FluentValidation;
using MotoRent.Domain.Interfaces;

namespace MotoRent.Application.Commands.LeaseContracts.UpdateLeaseContractEndContract
{
	public class UpdateLeaseContractEndContractCommandValidator : AbstractValidator<UpdateLeaseContractEndContractCommand>
	{
		public UpdateLeaseContractEndContractCommandValidator(ILeaseContractRepository leaseContractRepository)
		{
			RuleFor(x => x.Id)
				.NotEmpty()
				.OverridePropertyName("Id")
				.WithMessage("Id é obrigatório")
				.MustAsync(async (id, ct) =>
				{
					var contract = await leaseContractRepository.FindByIdAsync(id, ct);
					return contract != null;
				})
				.WithMessage("Contrato de locação não encontrado");

			RuleFor(x => x.DeliveryDate)
				.NotEmpty()
				.OverridePropertyName("Data_Devolucao")
				.WithMessage("Data_Devolucao é obrigatória")
				.MustAsync(async (command, deliveryDate, ct) =>
				{
					var contract = await leaseContractRepository.FindByIdAsync(command.Id, ct);
					if (contract == null) return false;

					return deliveryDate.Date >= contract.StartDate.Date;
				})
				.WithMessage("Data_Devolucao deve ser maior ou igual à Data_Inicio do contrato");
		}
	}
}
