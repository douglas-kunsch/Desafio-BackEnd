using System.Text.Json.Serialization;

namespace MotoRent.Api.Response
{
	public class MotorcycleResponse
	{
		[JsonPropertyName("id")]
		public Guid Id { get; set; }

		[JsonPropertyName("identificador")]
		public required string Identifier { get; set; }

		[JsonPropertyName("ano")]
		public int Year { get; set; }

		[JsonPropertyName("modelo")]
		public required string Model { get; set; }

		[JsonPropertyName("placa")]
		public required string LicensePlate { get; set; }
	}
}
