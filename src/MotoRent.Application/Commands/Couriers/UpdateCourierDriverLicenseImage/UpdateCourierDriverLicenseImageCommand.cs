using MediatR;
using System;
using System.IO;

namespace MotoRent.Application.Commands.Couriers.UpdateCourierDriverLicenseImage
{
	public class UpdateCourierDriverLicenseImageCommand : IRequest<bool>
	{
		public Guid Id { get; set; }
		public string FileName { get; set; } = null!;
		public string ContentType { get; set; } = null!;
		public Stream FileStream { get; set; } = null!;
	}
}
