using System.Text.RegularExpressions;

namespace MotoRent.Domain.Common
{
	public static class PlateValidator
	{
		public static bool IsValid(string? plate)
		{
			if (string.IsNullOrWhiteSpace(plate))
				return false;

			return Regex.IsMatch(plate, PlateRegexPatterns.Normal)
				|| Regex.IsMatch(plate, PlateRegexPatterns.Mercosul);
		}
	}
}