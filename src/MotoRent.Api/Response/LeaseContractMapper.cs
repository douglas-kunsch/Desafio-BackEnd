using MotoRent.Domain.Entities;

namespace MotoRent.Api.Response
{
	public static class LeaseContractMapper
	{
		public static LeaseContractResponse ToResponse(this LeaseContract leaseContract)
		{
			return new LeaseContractResponse()
			{
				Id = leaseContract.Id,
				ValorDiaria = leaseContract.Plan,
				EntregadorId = leaseContract.CourierId,
				MotoId = leaseContract.MotorcycleId,
				DataInicio = leaseContract.StartDate,
				DataTermino = leaseContract.EndDate,
				DataPrevisaoTermino = leaseContract.ExpectedEndDate,
				DataDevolucao = leaseContract.DeliveryDate,
				PrevisaoValor = leaseContract.ExpectedAmount,
				Valor = leaseContract.Amount,
				Multa = leaseContract.Fine,
				Status = leaseContract.IsActive ? "Ativo" : "Encerrado"
			};
		}

		public static IEnumerable<LeaseContractResponse> ToResponse(this IEnumerable<LeaseContract> leaseContracts)
		{
			return leaseContracts.Select(c => c.ToResponse());
		}
	}
}
