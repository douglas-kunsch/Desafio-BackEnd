using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using MotoRent.Domain.Interfaces;
using MotoRent.Infrastructure.Common.Option;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MotoRent.Infrastructure.Services
{
	public class StorageService : IStorageService
	{
		private readonly IMinioClient _minio;
		private readonly string _bucket;

		public StorageService(IMinioClient minio, IOptions<MinioOptions> options)
		{
			_minio = minio;
			_bucket = options.Value.Bucket;
		}

		public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken ct)
		{
			bool exists = await _minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucket), ct);
			if (!exists)
				await _minio.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucket), ct);
			var now = DateTime.UtcNow;
			string year = now.Year.ToString("0000", CultureInfo.InvariantCulture);
			string month = now.Month.ToString("00", CultureInfo.InvariantCulture);
			string day = now.Day.ToString("00", CultureInfo.InvariantCulture);

			string extension = Path.GetExtension(fileName);
			string newFileName = $"{Guid.NewGuid()}{extension}";
			string objectName = $"{year}/{month}/{day}/{newFileName}";

			await _minio.PutObjectAsync(new PutObjectArgs()
				.WithBucket(_bucket)
				.WithObject(objectName)
				.WithStreamData(fileStream)
				.WithObjectSize(fileStream.Length)
				.WithContentType(contentType), ct);

			return objectName;
		}

		public async Task<Stream?> GetAsync(string fileName, CancellationToken cancellationToken)
		{
			MemoryStream ms = new();
			await _minio.GetObjectAsync(new GetObjectArgs()
				.WithBucket(_bucket)
				.WithObject(fileName)
				.WithCallbackStream(stream => stream.CopyTo(ms)), cancellationToken);

			ms.Position = 0;
			return ms;
		}

		public async Task<bool> DeleteAsync(string fileName, CancellationToken cancellationToken)
		{
			await _minio.RemoveObjectAsync(new RemoveObjectArgs()
				.WithBucket(_bucket)
				.WithObject(fileName), cancellationToken);

			return true;
		}
	}
}
