using System.Text.Json.Serialization;

namespace MotoRent.Api.Request
{
	public class UpdateLeaseContractDelivereyDateRequest
	{
		[JsonPropertyName("data_devolucao")]
		public DateTime DeliveryDate { get; set; }
	}
}
