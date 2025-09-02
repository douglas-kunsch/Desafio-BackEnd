using System;

namespace MotoRent.Domain.Entities
{
	public class Entity
	{
		public Guid Id { get; private set; } = Guid.NewGuid();
		public DateTime CreateDate { get; private set; } = DateTime.UtcNow;
	}
}
