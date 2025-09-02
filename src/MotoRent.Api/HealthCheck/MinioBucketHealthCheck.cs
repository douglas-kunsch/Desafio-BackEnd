namespace MotoRent.Api.HealthCheck
{
	using Microsoft.Extensions.Diagnostics.HealthChecks;
	using Microsoft.Extensions.Options;
	using Minio;
	using Minio.DataModel.Args;
	using MotoRent.Infrastructure.Common.Option;
	using System.Threading;
	using System.Threading.Tasks;

	public class MinioBucketHealthCheck : IHealthCheck
	{
		private readonly IMinioClient _minioClient;
		private readonly MinioOptions _settings;

		public MinioBucketHealthCheck(IMinioClient minioClient, IOptions<MinioOptions> settings)
		{
			_minioClient = minioClient;
			_settings = settings.Value;
		}

		public async Task<HealthCheckResult> CheckHealthAsync(
			HealthCheckContext context,
			CancellationToken cancellationToken = default)
		{
			try
			{
				bool exists = await _minioClient.BucketExistsAsync(
					new BucketExistsArgs().WithBucket(_settings.Bucket),
					cancellationToken);

				return exists
					? HealthCheckResult.Healthy($"Bucket '{_settings.Bucket}' available")
					: HealthCheckResult.Unhealthy($"Bucket '{_settings.Bucket}' not found");
			}
			catch (Exception ex)
			{
				return HealthCheckResult.Unhealthy("Error verifying Minio", ex);
			}
		}
	}
}