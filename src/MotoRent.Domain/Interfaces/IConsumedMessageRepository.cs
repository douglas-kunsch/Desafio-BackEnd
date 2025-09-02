using MotoRent.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace MotoRent.Domain.Interfaces
{
	public interface IConsumedMessageRepository
	{
		Task AddAsync(ConsumedMessage message, CancellationToken ct);
	}
}
