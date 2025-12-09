using LicenseManagement.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace LicenseService.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options) { }

		public DbSet<LicenseApplication> LicenseApplications => Set<LicenseApplication>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<LicenseApplication>().HasKey(x => x.Id);
			modelBuilder.Entity<LicenseApplication>()
						.Property(x => x.TenantId)
						.IsRequired();
		}
	}
}
