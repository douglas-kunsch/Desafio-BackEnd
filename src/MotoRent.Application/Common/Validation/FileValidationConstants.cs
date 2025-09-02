namespace MotoRent.Application.Common.Validation
{
	public static class FileValidationConstants
	{
		public static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png" };
		public static readonly string[] AllowedImageContentTypes = { "image/jpeg", "image/png" };
		public const long MaxImageFileSizeBytes = 5 * 1024 * 1024; // 5MB
	}
}
