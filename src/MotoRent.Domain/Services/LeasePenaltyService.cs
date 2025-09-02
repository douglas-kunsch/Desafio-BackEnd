using System;

namespace MotoRent.Domain.Services
{
	public static class LeasePenaltyService
	{
		public static decimal CalculatePenalty(
			int planDays,
			DateTime startDate,
			DateTime expectedEndDate,
			DateTime actualEndDate)
		{
			decimal dailyRate = LeasePlan.GetDailyRate(planDays);

			if (actualEndDate <= expectedEndDate)
			{
				int unusedDays;

				// Caso devolva no mesmo dia do início → conta como 1 diária paga
				if (actualEndDate.Date == startDate.Date)
					unusedDays = planDays - 1;
				else
					unusedDays = (expectedEndDate.Date - actualEndDate.Date).Days;

				if (unusedDays <= 0)
					return 0m;

				decimal penaltyRate = planDays switch
				{
					7 => 0.20m,
					15 => 0.40m,
					_ => 0m
				};

				return unusedDays * dailyRate * penaltyRate;
			}

			// Devolução atrasada
			if (actualEndDate > expectedEndDate)
			{
				int extraDays = (actualEndDate.Date - expectedEndDate.Date).Days;
				return extraDays * 50m;
			}

			return 0m;
		}
	}
}