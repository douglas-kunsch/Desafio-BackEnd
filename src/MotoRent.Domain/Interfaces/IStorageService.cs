using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MotoRent.Domain.Interfaces
{
	public interface IStorageService
	{
		Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken);
		Task<Stream?> GetAsync(string fileName, CancellationToken cancellationToken);
		Task<bool> DeleteAsync(string fileName, CancellationToken cancellationToken);
	}
}
