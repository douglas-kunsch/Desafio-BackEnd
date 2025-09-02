using Microsoft.EntityFrameworkCore;
using MotoRent.Domain.Entities;
using MotoRent.Domain.Interfaces;
using MotoRent.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MotoRent.Infrastructure.Repositories
{
	public class CourierRepository : ICourierRepository
	{
		private readonly AppDbContext _db;
		public CourierRepository(AppDbContext db) => _db = db;

		public async Task AddAsync(Courier courier, CancellationToken cancellationToken)
		{
			await _db.Couriers.AddAsync(courier, cancellationToken);
			await _db.SaveChangesAsync(cancellationToken);
		}

		public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			var courier = await _db.Couriers.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

			if (courier is null)
				return false;

			_db.Couriers.Remove(courier);
			await _db.SaveChangesAsync(cancellationToken);

			return true;
		}

		public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			return await _db.Couriers.AnyAsync(m => m.Id == id, cancellationToken);
		}

		public async Task<bool> ExistsByCnpjAsync(string cnpj, CancellationToken cancellationToken)
		{
			return await _db.Couriers.AnyAsync(m => m.Cnpj == cnpj, cancellationToken);
		}

		public async Task<bool> ExistsByDriverLicenseNumberAsync(string driverLicenseNumber, CancellationToken cancellationToken)
		{
			return await _db.Couriers.AnyAsync(m => m.DriverLicenseNumber == driverLicenseNumber, cancellationToken);
		}

		public async Task<IEnumerable<Courier>> FindAsync(string? driverLicenseType, CancellationToken cancellationToken)
		{
			return await _db.Couriers.Where(m => driverLicenseType == null || m.DriverLicenseType == driverLicenseType).ToListAsync(cancellationToken);
		}

		public async Task<Courier?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			return await _db.Couriers.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
		}

		public async Task<bool> UpdateImagem(Guid id, string imagemPath, CancellationToken cancellationToken)
		{
			var courier = await _db.Couriers.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

			if (courier is null)
				return false;

			courier.DriverLicenseImage = imagemPath;

			await _db.SaveChangesAsync(cancellationToken);
			return true;
		}
	}
}
