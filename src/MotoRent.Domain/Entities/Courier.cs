using System;
using System.Collections.Generic;

namespace MotoRent.Domain.Entities
{
	public class Courier : Entity
	{
		public string Identifier { get; set; } = null!;
		public string Name { get; set; } = null!;
		public string Cnpj { get; set; } = null!;
		public DateTime BirthDate { get; set; }
		public string DriverLicenseNumber { get; set; } = null!;
		public string DriverLicenseType { get; set; } = null!;
		public string DriverLicenseImage { get; set; } = null!;

		public ICollection<LeaseContract> LeaseContracts { get; set; } = new List<LeaseContract>();
	}
}
