namespace LicenseService.Jobs
{
	public class NotificationJob
	{
		public Task SendApprovalNotification(Guid licenseId)
		{
			Console.WriteLine($"✅ License {licenseId} APPROVED - Email sent!");
			return Task.CompletedTask;
		}

		public Task SendRenewalNotification(Guid licenseId)
		{
			Console.WriteLine($"⚠️ License {licenseId} RENEWAL REMINDER - Email sent!");
			return Task.CompletedTask;
		}
	}
}
