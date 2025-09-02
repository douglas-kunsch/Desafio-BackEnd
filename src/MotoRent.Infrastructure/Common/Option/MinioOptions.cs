namespace MotoRent.Infrastructure.Common.Option
{
	public class MinioOptions
	{
		public string Endpoint { get; set; } = string.Empty;
		public string AccessKey { get; set; } = string.Empty;
		public string SecretKey { get; set; } = string.Empty;
		public bool UseSSL { get; set; }
		public string Bucket { get; set; } = string.Empty;
	}
}
