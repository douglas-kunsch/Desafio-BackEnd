
using Microsoft.EntityFrameworkCore;
using MotoRent.Domain.Entities;
using MotoRent.Domain.Interfaces;
using MotoRent.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MotoRent.Infrastructure.Repositories;
public class MotorcycleRepository : IMotorcycleRepository
{
	private readonly AppDbContext _db;
	public MotorcycleRepository(AppDbContext db) => _db = db;
	public async Task AddAsync(Motorcycle motorcycle, CancellationToken cancellationToken)
	{
		await _db.Motorcycles.AddAsync(motorcycle, cancellationToken);
		await _db.SaveChangesAsync(cancellationToken);
	}

	public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		var motorcycle = await _db.Motorcycles.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

		if (motorcycle is null)
			return false;

		_db.Motorcycles.Remove(motorcycle);
		await _db.SaveChangesAsync(cancellationToken);

		return true;
	}

	public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken) => await _db.Motorcycles.AnyAsync(m => m.Id == id, cancellationToken);
	public async Task<bool> ExistsByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken) => await _db.Motorcycles.AnyAsync(m => m.LicensePlate == licensePlate, cancellationToken);
	public async Task<IEnumerable<Motorcycle>> FindAsync(string? licensePlate, CancellationToken cancellationToken) =>
		await _db.Motorcycles.Where(m => licensePlate == null || m.LicensePlate == licensePlate).ToListAsync(cancellationToken);

	public async Task<Motorcycle?> FindByIdAsync(Guid id, CancellationToken cancellationToken) => await _db.Motorcycles.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

	public async Task<bool> UpdateLicensePlate(Guid id, string licensePlate, CancellationToken cancellationToken)
	{
		var motorcycle = await _db.Motorcycles.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

		if (motorcycle is null)
			return false;

		motorcycle.LicensePlate = licensePlate;

		await _db.SaveChangesAsync(cancellationToken);
		return true;
	}
}
