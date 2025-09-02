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
	public class MotorcycleRepositoryTests
	{
		private AppDbContext CreateDbContext()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;
			return new AppDbContext(options);
		}

		private Motorcycle CreateMotorcycle(string plate = "ABC1234") =>
			new Motorcycle
			{
				LicensePlate = plate,
				Model = "Honda CG 160",
				Year = 2022,
				Identifier = Guid.NewGuid().ToString()
			};

		[Fact]
		public async Task AddAsync_ShouldAddMotorcycle()
		{
			using var context = CreateDbContext();
			var repo = new MotorcycleRepository(context);

			var moto = CreateMotorcycle();

			await repo.AddAsync(moto, CancellationToken.None);

			bool exists = await context.Motorcycles.AnyAsync(m => m.LicensePlate == moto.LicensePlate);
			Assert.True(exists);
		}

		[Fact]
		public async Task DeleteByIdAsync_ShouldDelete_WhenExists()
		{
			using var context = CreateDbContext();
			var repo = new MotorcycleRepository(context);

			var moto = CreateMotorcycle();
			await context.Motorcycles.AddAsync(moto);
			await context.SaveChangesAsync();

			bool result = await repo.DeleteByIdAsync(moto.Id, CancellationToken.None);

			Assert.True(result);
			Assert.False(await context.Motorcycles.AnyAsync(m => m.Id == moto.Id));
		}

		[Fact]
		public async Task DeleteByIdAsync_ShouldReturnFalse_WhenNotExists()
		{
			using var context = CreateDbContext();
			var repo = new MotorcycleRepository(context);

			bool result = await repo.DeleteByIdAsync(Guid.NewGuid(), CancellationToken.None);

			Assert.False(result);
		}

		[Fact]
		public async Task ExistsByIdAsync_ShouldReturnTrue_WhenExists()
		{
			using var context = CreateDbContext();
			var repo = new MotorcycleRepository(context);

			var moto = CreateMotorcycle();
			await context.Motorcycles.AddAsync(moto);
			await context.SaveChangesAsync();

			bool result = await repo.ExistsByIdAsync(moto.Id, CancellationToken.None);

			Assert.True(result);
		}

		[Fact]
		public async Task ExistsByLicensePlateAsync_ShouldReturnTrue_WhenExists()
		{
			using var context = CreateDbContext();
			var repo = new MotorcycleRepository(context);

			var moto = CreateMotorcycle("XYZ9876");
			await context.Motorcycles.AddAsync(moto);
			await context.SaveChangesAsync();

			bool result = await repo.ExistsByLicensePlateAsync("XYZ9876", CancellationToken.None);

			Assert.True(result);
		}

		[Fact]
		public async Task FindAsync_ShouldReturnAll_WhenLicensePlateIsNull()
		{
			using var context = CreateDbContext();
			var repo = new MotorcycleRepository(context);

			await context.Motorcycles.AddRangeAsync(
				CreateMotorcycle("AAA1111"),
				CreateMotorcycle("BBB2222")
			);
			await context.SaveChangesAsync();

			var result = await repo.FindAsync(null, CancellationToken.None);

			Assert.Equal(2, result.Count());
		}

		[Fact]
		public async Task FindAsync_ShouldFilterByLicensePlate()
		{
			using var context = CreateDbContext();
			var repo = new MotorcycleRepository(context);

			var m1 = CreateMotorcycle("AAA1111");
			var m2 = CreateMotorcycle("BBB2222");

			await context.Motorcycles.AddRangeAsync(m1, m2);
			await context.SaveChangesAsync();

			var result = await repo.FindAsync("BBB2222", CancellationToken.None);

			Assert.Single(result);
			Assert.Equal("BBB2222", result.First().LicensePlate);
		}

		[Fact]
		public async Task FindByIdAsync_ShouldReturnMotorcycle_WhenExists()
		{
			using var context = CreateDbContext();
			var repo = new MotorcycleRepository(context);

			var moto = CreateMotorcycle();
			await context.Motorcycles.AddAsync(moto);
			await context.SaveChangesAsync();

			var found = await repo.FindByIdAsync(moto.Id, CancellationToken.None);

			Assert.NotNull(found);
			Assert.Equal(moto.LicensePlate, found!.LicensePlate);
		}

		[Fact]
		public async Task UpdateLicensePlate_ShouldUpdate_WhenExists()
		{
			using var context = CreateDbContext();
			var repo = new MotorcycleRepository(context);

			var moto = CreateMotorcycle("OLD1234");
			await context.Motorcycles.AddAsync(moto);
			await context.SaveChangesAsync();

			bool result = await repo.UpdateLicensePlate(moto.Id, "NEW5678", CancellationToken.None);

			var updated = await context.Motorcycles.FirstAsync(m => m.Id == moto.Id);

			Assert.True(result);
			Assert.Equal("NEW5678", updated.LicensePlate);
		}

		[Fact]
		public async Task UpdateLicensePlate_ShouldReturnFalse_WhenNotExists()
		{
			using var context = CreateDbContext();
			var repo = new MotorcycleRepository(context);

			bool result = await repo.UpdateLicensePlate(Guid.NewGuid(), "NEW5678", CancellationToken.None);

			Assert.False(result);
		}
	}
}
