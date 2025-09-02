using System.Text.Json.Serialization;

namespace MotoRent.Api.Request
{
	public class UpdateLicensePlateRequest
	{
		[JsonPropertyName("placa")]
		public required string LicensePlate { get; set; }
	}
}
