using MotoRent.Domain.Entities;
using MotoRent.Domain.Interfaces;
using MotoRent.Infrastructure.Context;
using System.Threading;
using System.Threading.Tasks;

namespace MotoRent.Infrastructure.Repositories
{
	public class ConsumedMessageRepository : IConsumedMessageRepository
	{
		private readonly AppDbContext _context;

		public ConsumedMessageRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task AddAsync(ConsumedMessage message, CancellationToken ct)
		{
			await _context.ConsumedMessages.AddAsync(message, ct);
			await _context.SaveChangesAsync(ct);
		}
	}
}
