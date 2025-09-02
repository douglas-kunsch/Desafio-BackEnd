using System.Collections.Generic;

namespace MotoRent.Domain.Entities;
public class Motorcycle : Entity
{
	public int Year { get; set; }
	public string Model { get; set; } = null!;
	public string Identifier { get; set; } = null!;
	public string LicensePlate { get; set; } = null!;
	public bool HasFine { get; set; } = false;
	public ICollection<LeaseContract> LeaseContracts { get; set; } = new List<LeaseContract>();

	public Motorcycle() { }

	public Motorcycle(string identifier, int year, string model, string licensePlate)
	{
		this.Identifier = identifier;
		this.Year = year;
		this.Model = model;
		this.LicensePlate = licensePlate;
	}
}
