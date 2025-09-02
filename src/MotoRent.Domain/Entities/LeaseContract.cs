using System;

namespace MotoRent.Domain.Entities
{
	public class LeaseContract : Entity
	{
		public Guid MotorcycleId { get; set; }
		public Motorcycle Motorcycle { get; set; } = null!;
		public Guid CourierId { get; set; }
		public Courier Courier { get; set; } = null!;
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public decimal? Amount { get; set; }
		public decimal? Fine { get; set; }
		public bool IsActive { get; set; }
		public DateTime ExpectedEndDate { get; set; }
		public decimal ExpectedAmount { get; set; }
		public int Plan { get; set; }
	}
}
