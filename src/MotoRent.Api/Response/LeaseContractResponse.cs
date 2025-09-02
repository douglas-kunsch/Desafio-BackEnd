using System.Text.Json.Serialization;

namespace MotoRent.Api.Response
{
	public class LeaseContractResponse
	{
		[JsonPropertyName("Id")]
		public required Guid Id { get; set; }

		[JsonPropertyName("valor_diaria")]
		public int ValorDiaria { get; set; }

		[JsonPropertyName("entregador_id")]
		public required Guid EntregadorId { get; set; }

		[JsonPropertyName("moto_id")]
		public required Guid MotoId { get; set; }

		[JsonPropertyName("data_inicio")]
		public DateTime DataInicio { get; set; }

		[JsonPropertyName("data_termino")]
		public DateTime? DataTermino { get; set; }

		[JsonPropertyName("data_previsao_termino")]
		public DateTime DataPrevisaoTermino { get; set; }

		[JsonPropertyName("data_devolucao")]
		public DateTime? DataDevolucao { get; set; }

		[JsonPropertyName("previsao_valor")]
		public decimal PrevisaoValor { get; set; }

		[JsonPropertyName("valor")]
		public decimal? Valor { get; set; }

		[JsonPropertyName("multa")]
		public decimal? Multa { get; set; }

		[JsonPropertyName("status")]
		public required string Status { get; set; }
	}
}
