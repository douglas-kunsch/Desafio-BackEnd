
using MotoRent.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MotoRent.Domain.Interfaces;

public interface IMotorcycleRepository
{
	Task AddAsync(Motorcycle motorcycle, CancellationToken cancellationToken);
	Task<bool> ExistsByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken);
	Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken);
	Task<bool> UpdateLicensePlate(Guid id, string licensePlate, CancellationToken cancellationToken);
	Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken);
	Task<IEnumerable<Motorcycle>> FindAsync(string? licensePlate, CancellationToken cancellationToken);
	Task<Motorcycle?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
}
