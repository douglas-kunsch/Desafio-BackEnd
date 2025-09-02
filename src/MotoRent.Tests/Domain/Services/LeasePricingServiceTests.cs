using MotoRent.Domain.Services;
using System;
using Xunit;

namespace MotoRent.Tests.Domain.Services
{
	public class LeasePricingServiceTests
	{
		[Fact]
		public void CalculateCost_ShouldReturnDailyRateTimesDays_WhenValidDates()
		{
			// Arrange
			int planDays = 7;
			var startDate = new DateTime(2025, 1, 1);
			var endDate = new DateTime(2025, 1, 8);

			// Act
			decimal cost = LeasePricingService.CalculateCost(planDays, startDate, endDate);

			// Assert
			Assert.Equal(7 * LeasePlan.GetDailyRate(planDays), cost);
		}

		[Fact]
		public void CalculateCost_ShouldThrow_WhenEndDateBeforeStartDate()
		{
			// Arrange
			int planDays = 7;
			var startDate = new DateTime(2025, 1, 10);
			var endDate = new DateTime(2025, 1, 9);

			// Act & Assert
			Assert.Throws<InvalidOperationException>(() =>
				LeasePricingService.CalculateCost(planDays, startDate, endDate));
		}

		[Fact]
		public void CalculateCost_ShouldCountSameDayAsOneDay()
		{
			// Arrange
			int planDays = 7;
			var startDate = new DateTime(2025, 1, 1);
			var endDate = new DateTime(2025, 1, 1);

			// Act
			decimal cost = LeasePricingService.CalculateCost(planDays, startDate, endDate);

			// Assert
			Assert.Equal(LeasePlan.GetDailyRate(planDays), cost);
		}

		[Theory]
		[InlineData(7, 3)]   // devolveu em 3 dias de aluguel
		[InlineData(15, 10)] // devolveu em 10 dias de aluguel
		[InlineData(30, 25)] // devolveu em 25 dias de aluguel
		public void CalculateCost_ShouldHandlePartialPeriods(int planDays, int rentedDays)
		{
			// Arrange
			var startDate = new DateTime(2025, 1, 1);
			var endDate = startDate.AddDays(rentedDays);

			// Act
			decimal cost = LeasePricingService.CalculateCost(planDays, startDate, endDate);

			// Assert
			Assert.Equal(rentedDays * LeasePlan.GetDailyRate(planDays), cost);
		}

		[Theory]
		[InlineData(7)]
		[InlineData(15)]
		[InlineData(30)]
		public void CalculateCost_ShouldCalculateFullPlanPeriod(int planDays)
		{
			// Arrange
			var startDate = new DateTime(2025, 1, 1);
			var endDate = startDate.AddDays(planDays);

			// Act
			decimal cost = LeasePricingService.CalculateCost(planDays, startDate, endDate);

			// Assert
			Assert.Equal(planDays * LeasePlan.GetDailyRate(planDays), cost);
		}

		[Theory]
		[InlineData(7, 9)]   // usou 2 dias a mais
		[InlineData(15, 20)] // usou 5 dias a mais
		[InlineData(30, 35)] // usou 5 dias a mais
		public void CalculateCost_ShouldAllowBeyondPlanDays(int planDays, int rentedDays)
		{
			// Arrange
			var startDate = new DateTime(2025, 1, 1);
			var endDate = startDate.AddDays(rentedDays);

			// Act
			decimal cost = LeasePricingService.CalculateCost(planDays, startDate, endDate);

			// Assert
			Assert.Equal(rentedDays * LeasePlan.GetDailyRate(planDays), cost);
		}
	}
}
