using MotoRent.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MotoRent.Domain.Interfaces
{
	public interface ICourierRepository
	{
		Task AddAsync(Courier courier, CancellationToken cancellationToken);
		Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken);
		Task<bool> ExistsByCnpjAsync(string cnpj, CancellationToken cancellationToken);
		Task<bool> ExistsByDriverLicenseNumberAsync(string driverLicenseNumber, CancellationToken cancellationToken);
		Task<bool> UpdateImagem(Guid id, string imagemPath, CancellationToken cancellationToken);
		Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken);
		Task<IEnumerable<Courier>> FindAsync(string? driverLicenseType, CancellationToken cancellationToken);
		Task<Courier?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
	}
}