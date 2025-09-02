namespace MotoRent.Api.Request
{
	public class CreateCourierRequest
	{
		public required string Identificador { get; set; }
		public required string Nome { get; set; }
		public required string Cnpj { get; set; }
		public DateTime Data_Nascimento { get; set; }
		public required string Numero_Cnh { get; set; }
		public required string Tipo_Cnh { get; set; }
		public IFormFile Imagem_Cnh { get; set; } = null!;
	}
}
