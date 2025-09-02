using System;

namespace MotoRent.Domain.Services
{
	public static class LeasePlan
	{
		public static decimal GetDailyRate(int planDays)
		{
			return planDays switch
			{
				7 => 30m,
				15 => 28m,
				30 => 22m,
				45 => 20m,
				50 => 18m,
				_ => throw new InvalidOperationException("Plano inválido")
			};
		}
	}
}
