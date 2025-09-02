using Microsoft.EntityFrameworkCore;
using MotoRent.Domain.Entities;
using MotoRent.Infrastructure.Context;
using MotoRent.Infrastructure.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MotoRent.Tests.Infrastructure.Repositories
{
	public class ConsumedMessageRepositoryTests
	{
		private AppDbContext CreateInMemoryDb()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString()) // banco isolado por teste
				.Options;

			return new AppDbContext(options);
		}

		[Fact]
		public async Task AddAsync_ShouldAddMessageToDatabase()
		{
			// Arrange
			var dbContext = CreateInMemoryDb();
			var repo = new ConsumedMessageRepository(dbContext);

			var message = new ConsumedMessage
			{
				Id = Guid.NewGuid(),
				EventType = "Test type",
				Payload = "Test message",
				ReceivedAt = DateTime.UtcNow
			};

			// Act
			await repo.AddAsync(message, CancellationToken.None);

			// Assert
			var saved = await dbContext.ConsumedMessages.FirstOrDefaultAsync(m => m.Id == message.Id);
			Assert.NotNull(saved);
			Assert.Equal(message.Id, saved.Id);
			Assert.Equal(message.EventType, saved.EventType);
			Assert.Equal(message.Payload, saved.Payload);
		}
	}
}
