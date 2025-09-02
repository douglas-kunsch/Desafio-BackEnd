using MotoRent.Domain.Entities;

namespace MotoRent.Api.Response
{
	public static class CourierMapper
	{
		public static CourierResponse ToResponse(this Courier courier)
		{
			return new CourierResponse()
			{
				Id = courier.Id,
				Identificador = courier.Identifier,
				Nome = courier.Name,
				Cnpj = courier.Cnpj,
				DataNascimento = courier.BirthDate,
				NumeroCnh = courier.DriverLicenseNumber,
				TipoCnh = courier.DriverLicenseType,
				ImagemCnh = courier.DriverLicenseImage
			};
		}

		public static IEnumerable<CourierResponse> ToResponse(this IEnumerable<Courier> couriers)
		{
			return couriers.Select(c => c.ToResponse());
		}
	}
}
