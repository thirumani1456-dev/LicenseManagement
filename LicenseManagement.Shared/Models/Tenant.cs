namespace LicenseManagement.Shared.Models
{
	public class Tenant
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string ConnectionString { get; set; } = string.Empty;
		public bool IsActive { get; set; }
	}

	public interface IMultiTenantEntity
	{
		Guid TenantId { get; set; }
	}
}
