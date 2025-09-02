namespace MotoRent.Domain.Common
{
	public static class PlateRegexPatterns
	{
		// Padrão antigo (AAA-0000)
		public const string Normal = @"^[A-Z]{3}-\d{4}$";

		// Padrão Mercosul (AAA0A00)
		public const string Mercosul = @"^[A-Z]{3}\d[A-Z]\d{2}$";
	}
}