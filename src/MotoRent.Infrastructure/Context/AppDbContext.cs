
using Microsoft.EntityFrameworkCore;
using MotoRent.Domain.Entities;

namespace MotoRent.Infrastructure.Context;
public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

	public DbSet<Motorcycle> Motorcycles => Set<Motorcycle>();
	public DbSet<Courier> Couriers { get; set; }
	public DbSet<LeaseContract> LeaseContracts { get; set; }
	public DbSet<ConsumedMessage> ConsumedMessages { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// define o schema default para todas as tabelas
		modelBuilder.HasDefaultSchema("MotoRent");

		modelBuilder.Entity<Motorcycle>().HasIndex(m => m.LicensePlate).IsUnique();
		modelBuilder.Entity<Motorcycle>().Property(m => m.LicensePlate).IsRequired();

		modelBuilder.Entity<Courier>().HasIndex(m => m.Cnpj).IsUnique();
		modelBuilder.Entity<Courier>().Property(m => m.Cnpj).IsRequired();


		modelBuilder.Entity<LeaseContract>().Property(m => m.StartDate).IsRequired();
		modelBuilder.Entity<LeaseContract>().Property(m => m.EndDate).IsRequired();
		modelBuilder.Entity<LeaseContract>().Property(m => m.ExpectedEndDate).IsRequired();


		modelBuilder.Entity<LeaseContract>()
			.HasOne(lc => lc.Motorcycle)
			.WithMany(m => m.LeaseContracts)
			.HasForeignKey(lc => lc.MotorcycleId);


		modelBuilder.Entity<LeaseContract>()
			.HasOne(lc => lc.Courier)
			.WithMany(c => c.LeaseContracts)
			.HasForeignKey(lc => lc.CourierId);
	}
}
