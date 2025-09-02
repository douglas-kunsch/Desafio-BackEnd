//using Microsoft.Extensions.Options;
//using Moq;
//using MotoRent.Infrastructure.Common.Option;
//using MotoRent.Infrastructure.Services;
//using RabbitMQ.Client;
//using System;
//using System.Threading.Tasks;
//using Xunit;

//namespace MotoRent.Tests.Infrastructure.Services
//{

//	public class PublisherServiceTests
//	{
//		[Fact]
//		public async Task PublishAsync_ShouldSendMessageToQueue()
//		{
//			// Arrange
//			var mockConnection = new Mock<IConnection>();
//			var mockChannel = new Mock<IModel>();
//			mockConnection.Setup(c => c.CreateModel()).Returns(mockChannel.Object);

//			var mockFactory = new Mock<ConnectionFactory>();
//			mockFactory.Setup(f => f.CreateConnection()).Returns(mockConnection.Object);

//			var options = Options.Create(new RabbitMqOptions
//			{
//				Host = "localhost",
//				User = "guest",
//				Password = "guest",
//				Queues = new() { MotorcycleRegistered = "test-queue" }
//			});

//			var service = new PublisherService(mockFactory.Object, options);

//			var testEvent = new { Id = 1, Name = "Moto" };
//			string queueName = "test-queue";

//			// Act
//			await service.PublishAsync(queueName, testEvent);

//			// Assert
//			mockChannel.Verify(ch => ch.BasicPublish(
//				"",
//				queueName,
//				false,
//				null,
//				It.IsAny<ReadOnlyMemory<byte>>()
//			), Times.Once);
//		}
//	}
//}
