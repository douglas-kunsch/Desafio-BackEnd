using System.Threading;
using System.Threading.Tasks;

namespace MotoRent.Domain.Interfaces
{
	public interface IPublisherService
	{
		Task PublishAsync<T>(string queueName, T message, CancellationToken cancellationToken = default);
	}
}
