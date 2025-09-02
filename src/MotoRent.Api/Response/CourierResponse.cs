using System.Text.Json.Serialization;

namespace MotoRent.Api.Response
{
	public class CourierResponse
	{
		[JsonPropertyName("id")]
		public Guid Id { get; set; }

		[JsonPropertyName("identificador")]
		public required string Identificador { get; set; }

		[JsonPropertyName("nome")]
		public required string Nome { get; set; }

		[JsonPropertyName("cnpj")]
		public required string Cnpj { get; set; }

		[JsonPropertyName("data_nascimento")]
		public DateTime DataNascimento { get; set; }

		[JsonPropertyName("numero_cnh")]
		public required string NumeroCnh { get; set; }

		[JsonPropertyName("tipo_cnh")]
		public required string TipoCnh { get; set; }

		[JsonPropertyName("imagem_cnh")]
		public required string ImagemCnh { get; set; }
	}
}
