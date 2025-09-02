using Microsoft.EntityFrameworkCore;
using MotoRent.Domain.Entities;
using MotoRent.Infrastructure.Context;
using MotoRent.Infrastructure.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MotoRent.Tests.Infrastructure.Repositories
{
	public class LeaseContractRepositoryTests
	{
		private AppDbContext CreateDbContext()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;
			return new AppDbContext(options);
		}

		private Courier CreateCourier() => new Courier
		{
			Name = "João Silva",
			Cnpj = "12345678000199",
			DriverLicenseNumber = "ABC123456",
			DriverLicenseType = "A",
			DriverLicenseImage = "/path/license.png",
			Identifier = Guid.NewGuid().ToString()
		};

		private Motorcycle CreateMotorcycle() => new Motorcycle
		{
			LicensePlate = "XYZ1A23",
			Model = "Honda CG 160",
			Year = 2022,
			Identifier = Guid.NewGuid().ToString()
		};

		private LeaseContract CreateLeaseContract(Guid courierId, Guid motorcycleId, int planDays = 7)
		{
			var start = DateTime.UtcNow.Date.AddDays(1); // mesma lógica do seu validator: começa no dia seguinte
			var end = start.AddDays(planDays);

			return new LeaseContract
			{
				CourierId = courierId,
				MotorcycleId = motorcycleId,
				StartDate = start,
				EndDate = end,
				ExpectedEndDate = end,
				ExpectedAmount = 0m,
				Plan = planDays,
				IsActive = true,
				Amount = null,
				Fine = null,
				DeliveryDate = null
			};
		}

		[Fact]
		public async Task AddAsync_ShouldAddLeaseContract()
		{
			using var context = CreateDbContext();
			var repo = new LeaseContractRepository(context);

			var courier = CreateCourier();
			var moto = CreateMotorcycle();
			await context.Couriers.AddAsync(courier);
			await context.Motorcycles.AddAsync(moto);
			await context.SaveChangesAsync();

			var contract = CreateLeaseContract(courier.Id, moto.Id, planDays: 7);

			await repo.AddAsync(contract, CancellationToken.None);

			bool exists = await context.LeaseContracts.AnyAsync(c => c.Id == contract.Id);
			Assert.True(exists);
		}

		[Fact]
		public async Task FindAsync_ShouldReturnFilteredByPlan()
		{
			using var context = CreateDbContext();
			var repo = new LeaseContractRepository(context);

			var courier = CreateCourier();
			var moto = CreateMotorcycle();
			await context.Couriers.AddAsync(courier);
			await context.Motorcycles.AddAsync(moto);
			await context.SaveChangesAsync();

			var c1 = CreateLeaseContract(courier.Id, moto.Id, planDays: 7);
			var c2 = CreateLeaseContract(courier.Id, moto.Id, planDays: 30);

			await context.LeaseContracts.AddRangeAsync(c1, c2);
			await context.SaveChangesAsync();

			var result = await repo.FindAsync(7, CancellationToken.None);

			Assert.Single(result);
			Assert.Equal(7, result.First().Plan);
		}

		[Fact]
		public async Task FindByIdAsync_ShouldReturnLeaseContract_WhenExists()
		{
			using var context = CreateDbContext();
			var repo = new LeaseContractRepository(context);

			var courier = CreateCourier();
			var moto = CreateMotorcycle();
			await context.Couriers.AddAsync(courier);
			await context.Motorcycles.AddAsync(moto);
			await context.SaveChangesAsync();

			var contract = CreateLeaseContract(courier.Id, moto.Id, planDays: 15);
			await context.LeaseContracts.AddAsync(contract);
			await context.SaveChangesAsync();

			var found = await repo.FindByIdAsync(contract.Id, CancellationToken.None);

			Assert.NotNull(found);
			Assert.Equal(contract.Id, found!.Id);
		}

		[Fact]
		public async Task UpdateEndOfContract_ShouldUpdate_WhenExists()
		{
			using var context = CreateDbContext();
			var repo = new LeaseContractRepository(context);

			var courier = CreateCourier();
			var moto = CreateMotorcycle();
			await context.Couriers.AddAsync(courier);
			await context.Motorcycles.AddAsync(moto);
			await context.SaveChangesAsync();

			var contract = CreateLeaseContract(courier.Id, moto.Id, planDays: 7);
			await context.LeaseContracts.AddAsync(contract);
			await context.SaveChangesAsync();

			var newEndDate = contract.StartDate.AddDays(9); // simulando devolução tardia
			bool result = await repo.UpdateEndOfContract(
				contract.Id, newEndDate, amount: 500m, fine: 50m, CancellationToken.None);

			var updated = await context.LeaseContracts.FirstAsync(c => c.Id == contract.Id);

			Assert.True(result);
			Assert.False(updated.IsActive);
			Assert.Equal(500m, updated.Amount);
			Assert.Equal(50m, updated.Fine);
			Assert.Equal(newEndDate, updated.EndDate);
			Assert.NotNull(updated.DeliveryDate);
		}

		[Fact]
		public async Task UpdateEndOfContract_ShouldReturnFalse_WhenNotExists()
		{
			using var context = CreateDbContext();
			var repo = new LeaseContractRepository(context);

			bool result = await repo.UpdateEndOfContract(Guid.NewGuid(), DateTime.UtcNow, 100, null, CancellationToken.None);

			Assert.False(result);
		}
	}
}
