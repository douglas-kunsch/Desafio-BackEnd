using System;

namespace MotoRent.Domain.Services
{
	public static class LeaseBillingService
	{
		public static (decimal Amount, decimal Fine) CloseContract(
			int planDays,
			DateTime startDate,
			DateTime expectedEndDate,
			DateTime actualEndDate)
		{
			decimal amount = LeasePricingService.CalculateCost(planDays, startDate, actualEndDate);

			decimal fine = LeasePenaltyService.CalculatePenalty(
				planDays,
				startDate,
				expectedEndDate,
				actualEndDate
			);

			return (amount, fine);
		}
	}
}
