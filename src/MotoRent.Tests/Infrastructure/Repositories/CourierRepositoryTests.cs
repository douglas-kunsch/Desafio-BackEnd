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
	public class CourierRepositoryTests
	{
		private AppDbContext CreateInMemoryDb()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;

			return new AppDbContext(options);
		}

		private Courier CreateCourier()
		{
			return new Courier
			{
				Name = "João Silva",
				Cnpj = "12345678000199",
				DriverLicenseNumber = "ABC123456",
				DriverLicenseType = "A",
				DriverLicenseImage = "/path/to/license.png",
				Identifier = Guid.NewGuid().ToString()
			};
		}

		[Fact]
		public async Task AddAsync_ShouldAddCourier()
		{
			var db = CreateInMemoryDb();
			var repo = new CourierRepository(db);

			var courier = CreateCourier();

			await repo.AddAsync(courier, CancellationToken.None);

			var saved = await db.Couriers.FirstOrDefaultAsync(c => c.Id == courier.Id);
			Assert.NotNull(saved);
			Assert.Equal(courier.Cnpj, saved.Cnpj);
		}

		[Fact]
		public async Task DeleteByIdAsync_ShouldDelete_WhenExists()
		{
			var db = CreateInMemoryDb();
			var repo = new CourierRepository(db);

			var courier = CreateCourier();
			await db.Couriers.AddAsync(courier);
			await db.SaveChangesAsync();

			bool result = await repo.DeleteByIdAsync(courier.Id, CancellationToken.None);

			Assert.True(result);
			Assert.False(await db.Couriers.AnyAsync(c => c.Id == courier.Id));
		}

		[Fact]
		public async Task DeleteByIdAsync_ShouldReturnFalse_WhenNotExists()
		{
			var db = CreateInMemoryDb();
			var repo = new CourierRepository(db);

			bool result = await repo.DeleteByIdAsync(Guid.NewGuid(), CancellationToken.None);

			Assert.False(result);
		}

		[Fact]
		public async Task ExistsByIdAsync_ShouldReturnTrue_WhenExists()
		{
			var db = CreateInMemoryDb();
			var repo = new CourierRepository(db);

			var courier = CreateCourier();
			await db.Couriers.AddAsync(courier);
			await db.SaveChangesAsync();

			bool exists = await repo.ExistsByIdAsync(courier.Id, CancellationToken.None);

			Assert.True(exists);
		}

		[Fact]
		public async Task ExistsByCnpjAsync_ShouldReturnTrue_WhenExists()
		{
			var db = CreateInMemoryDb();
			var repo = new CourierRepository(db);

			var courier = CreateCourier();
			await db.Couriers.AddAsync(courier);
			await db.SaveChangesAsync();

			bool exists = await repo.ExistsByCnpjAsync(courier.Cnpj, CancellationToken.None);

			Assert.True(exists);
		}

		[Fact]
		public async Task ExistsByDriverLicenseNumberAsync_ShouldReturnTrue_WhenExists()
		{
			var db = CreateInMemoryDb();
			var repo = new CourierRepository(db);

			var courier = CreateCourier();
			await db.Couriers.AddAsync(courier);
			await db.SaveChangesAsync();

			bool exists = await repo.ExistsByDriverLicenseNumberAsync(courier.DriverLicenseNumber, CancellationToken.None);

			Assert.True(exists);
		}

		[Fact]
		public async Task FindAsync_ShouldReturnFilteredCouriers()
		{
			var db = CreateInMemoryDb();
			var repo = new CourierRepository(db);

			var courier1 = CreateCourier();
			courier1.DriverLicenseType = "A";

			var courier2 = CreateCourier();
			courier2.DriverLicenseType = "B";

			await db.Couriers.AddRangeAsync(courier1, courier2);
			await db.SaveChangesAsync();

			var result = await repo.FindAsync("A", CancellationToken.None);

			Assert.Single(result);
			Assert.Equal("A", result.First().DriverLicenseType);
		}

		[Fact]
		public async Task FindByIdAsync_ShouldReturnCourier_WhenExists()
		{
			var db = CreateInMemoryDb();
			var repo = new CourierRepository(db);

			var courier = CreateCourier();
			await db.Couriers.AddAsync(courier);
			await db.SaveChangesAsync();

			var result = await repo.FindByIdAsync(courier.Id, CancellationToken.None);

			Assert.NotNull(result);
			Assert.Equal(courier.Id, result.Id);
		}

		[Fact]
		public async Task UpdateImagem_ShouldUpdate_WhenExists()
		{
			var db = CreateInMemoryDb();
			var repo = new CourierRepository(db);

			var courier = CreateCourier();
			await db.Couriers.AddAsync(courier);
			await db.SaveChangesAsync();

			string newImage = "/path/to/image.png";
			bool result = await repo.UpdateImagem(courier.Id, newImage, CancellationToken.None);

			Assert.True(result);
			var updated = await db.Couriers.FirstAsync(c => c.Id == courier.Id);
			Assert.Equal(newImage, updated.DriverLicenseImage);
		}

		[Fact]
		public async Task UpdateImagem_ShouldReturnFalse_WhenNotExists()
		{
			var db = CreateInMemoryDb();
			var repo = new CourierRepository(db);

			bool result = await repo.UpdateImagem(Guid.NewGuid(), "/path/to/image.png", CancellationToken.None);

			Assert.False(result);
		}
	}
}
