using MotoRent.Domain.Services;
using System;
using Xunit;

namespace MotoRent.Tests.Domain.Services
{
	public class LeasePenaltyServiceTests
	{
		[Theory]
		[InlineData(7, 2, 0.20)]   // Plano 7 → devolveu 5 dias antes, penalidade 20%
		[InlineData(15, 5, 0.40)]  // Plano 15 → devolveu 10 dias antes, penalidade 40%
		[InlineData(30, 10, 0.00)] // Plano 30 → devolveu 20 dias antes, sem penalidade
		public void CalculatePenalty_ShouldReturnEarlyReturnPenalty(int planDays, int actualReturnDays, decimal penaltyRate)
		{
			// Arrange
			var start = new DateTime(2025, 1, 1);
			var expectedEnd = start.AddDays(planDays);
			var actualEnd = start.AddDays(actualReturnDays); // devolução antecipada

			int unusedDays = (expectedEnd - actualEnd).Days;
			decimal dailyRate = LeasePlan.GetDailyRate(planDays);

			// Act
			decimal penalty = LeasePenaltyService.CalculatePenalty(planDays, start, expectedEnd, actualEnd);

			// Assert
			decimal expectedPenalty = unusedDays * dailyRate * penaltyRate;
			Assert.Equal(expectedPenalty, penalty);
		}


		[Theory]
		[InlineData(7, 2)]   // Plano 7 → 2 dias atrasado
		[InlineData(15, 5)]  // Plano 15 → 5 dias atrasado
		[InlineData(30, 3)]  // Plano 30 → 3 dias atrasado
		public void CalculatePenalty_ShouldReturnLateReturnPenalty(int planDays, int extraDays)
		{
			// Arrange
			var start = new DateTime(2025, 1, 1);
			var expectedEnd = start.AddDays(planDays);
			var actualEnd = expectedEnd.AddDays(extraDays); // devolução atrasada

			// Act
			decimal penalty = LeasePenaltyService.CalculatePenalty(planDays, start, expectedEnd, actualEnd);

			// Assert
			decimal expectedPenalty = extraDays * 50m;
			Assert.Equal(expectedPenalty, penalty);
		}

		[Theory]
		[InlineData(7)]
		[InlineData(15)]
		[InlineData(30)]
		public void CalculatePenalty_ShouldReturnZero_WhenReturnedOnTime(int planDays)
		{
			// Arrange
			var start = new DateTime(2025, 1, 1);
			var expectedEnd = start.AddDays(planDays);
			var actualEnd = expectedEnd; // devolveu no dia certo

			// Act
			decimal penalty = LeasePenaltyService.CalculatePenalty(planDays, start, expectedEnd, actualEnd);

			// Assert
			Assert.Equal(0m, penalty);
		}
	}
}
