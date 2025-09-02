using MotoRent.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MotoRent.Domain.Interfaces
{
	public interface ILeaseContractRepository
	{
		Task AddAsync(LeaseContract leaseContract, CancellationToken cancellationToken);
		Task<bool> UpdateEndOfContract(Guid id, DateTime endDate, decimal amount, decimal? fine, CancellationToken cancellationToken);
		Task<IEnumerable<LeaseContract>> FindAsync(int? planType, CancellationToken cancellationToken);
		Task<LeaseContract?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
	}
}
