using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Moq;
using MotoRent.Infrastructure.Common.Option;
using MotoRent.Infrastructure.Services;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MotoRent.Tests.Infrastructure.Services
{
	public class StorageServiceTests
	{
		private readonly Mock<IMinioClient> _minioMock;
		private readonly StorageService _service;
		private readonly string _bucketName = "test-bucket";

		public StorageServiceTests()
		{
			_minioMock = new Mock<IMinioClient>();
			var options = Options.Create(new MinioOptions { Bucket = _bucketName });
			_service = new StorageService(_minioMock.Object, options);
		}

		[Fact]
		public async Task UploadAsync_ShouldUpload_WhenBucketExists()
		{
			// Arrange
			_minioMock
				.Setup(m => m.BucketExistsAsync(It.IsAny<BucketExistsArgs>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(true);

			using var stream = new MemoryStream(Encoding.UTF8.GetBytes("test data"));

			// Act
			string result = await _service.UploadAsync(stream, "file.txt", "text/plain", CancellationToken.None);

			// Assert
			Assert.Contains(".txt", result);
			_minioMock.Verify(m => m.PutObjectAsync(It.IsAny<PutObjectArgs>(), It.IsAny<CancellationToken>()), Times.Once);
			_minioMock.Verify(m => m.MakeBucketAsync(It.IsAny<MakeBucketArgs>(), It.IsAny<CancellationToken>()), Times.Never);
		}

		[Fact]
		public async Task UploadAsync_ShouldCreateBucket_WhenBucketDoesNotExist()
		{
			// Arrange
			_minioMock
				.Setup(m => m.BucketExistsAsync(It.IsAny<BucketExistsArgs>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(false);

			using var stream = new MemoryStream(Encoding.UTF8.GetBytes("test data"));

			// Act
			string result = await _service.UploadAsync(stream, "file.png", "image/png", CancellationToken.None);

			// Assert
			Assert.Contains(".png", result);
			_minioMock.Verify(m => m.MakeBucketAsync(It.IsAny<MakeBucketArgs>(), It.IsAny<CancellationToken>()), Times.Once);
			_minioMock.Verify(m => m.PutObjectAsync(It.IsAny<PutObjectArgs>(), It.IsAny<CancellationToken>()), Times.Once);
		}

		[Fact]
		public async Task DeleteAsync_ShouldCallRemoveObject()
		{
			// Arrange
			_minioMock
				.Setup(m => m.RemoveObjectAsync(It.IsAny<RemoveObjectArgs>(), It.IsAny<CancellationToken>()))
				.Returns(Task.CompletedTask);

			// Act
			bool result = await _service.DeleteAsync("file.txt", CancellationToken.None);

			// Assert
			Assert.True(result);
			_minioMock.Verify(m => m.RemoveObjectAsync(It.IsAny<RemoveObjectArgs>(), It.IsAny<CancellationToken>()), Times.Once);
		}
	}
}
