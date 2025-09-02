using System;

namespace MotoRent.Domain.Services
{
	public static class LeasePricingService
	{
		public static decimal CalculateCost(int planDays, DateTime startDate, DateTime endDate)
		{
			decimal dailyRate = LeasePlan.GetDailyRate(planDays);

			int totalDays = (endDate.Date - startDate.Date).Days;


			if (totalDays < 0)
				throw new InvalidOperationException("Data final deve ser maior ou igual à inicial");

			// Caso devolva no mesmo dia do início → conta como 1 diária paga
			if (totalDays == 0)
				totalDays = 1;

			return dailyRate * totalDays;
		}
	}
}
