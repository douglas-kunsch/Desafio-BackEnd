using MotoRent.Domain.Entities;

namespace MotoRent.Api.Response
{
	public static class MotorcycleMapper
	{
		public static MotorcycleResponse ToResponse(this Motorcycle motorcycle)
		{
			return new MotorcycleResponse()
			{
				Id = motorcycle.Id,
				Identifier = motorcycle.Identifier,
				Year = motorcycle.Year,
				Model = motorcycle.Model,
				LicensePlate = motorcycle.LicensePlate
			};
		}

		public static IEnumerable<MotorcycleResponse> ToResponse(this IEnumerable<Motorcycle> motorcycles)
		{
			return motorcycles.Select(m => m.ToResponse());
		}
	}
}
