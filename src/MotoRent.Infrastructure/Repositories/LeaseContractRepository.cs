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
	public class LeaseContractRepository : ILeaseContractRepository
	{
		private readonly AppDbContext _db;
		public LeaseContractRepository(AppDbContext db) => _db = db;

		public async Task AddAsync(LeaseContract leaseContract, CancellationToken cancellationToken)
		{
			await _db.LeaseContracts.AddAsync(leaseContract, cancellationToken);
			await _db.SaveChangesAsync(cancellationToken);
		}

		public async Task<IEnumerable<LeaseContract>> FindAsync(int? planType, CancellationToken cancellationToken)
		{
			return await _db.LeaseContracts.Where(m => planType == null || m.Plan == planType).ToListAsync(cancellationToken);
		}

		public async Task<LeaseContract?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			return await _db.LeaseContracts.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
		}

		public async Task<bool> UpdateEndOfContract(Guid id, DateTime endDate, decimal amount, decimal? fine, CancellationToken cancellationToken)
		{
			var leaseContract = await _db.LeaseContracts.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

			if (leaseContract is null)
				return false;

			leaseContract.EndDate = endDate;
			leaseContract.Amount = amount;
			leaseContract.Fine = fine;
			leaseContract.DeliveryDate = DateTime.UtcNow;
			leaseContract.IsActive = false;

			await _db.SaveChangesAsync(cancellationToken);
			return true;
		}
	}
}
