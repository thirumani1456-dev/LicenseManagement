namespace LicenseManagement.Shared.Models
{
	public class LicenseApplication : IMultiTenantEntity
	{
		public Guid Id { get; set; }
		public Guid TenantId { get; set; }
		public string LicenseType { get; set; } = string.Empty;
		public string ApplicantName { get; set; } = string.Empty;
		public DateTime ApplicationDate { get; set; }
		public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
		public DateTime? ExpiryDate { get; set; }
	}
}
