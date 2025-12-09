using Hangfire;
using Microsoft.EntityFrameworkCore;


namespace LicenseService.Jobs
{
	public class LicenseRenewalJob
	{
		private readonly LicenseService.Data.ApplicationDbContext _context;

		public LicenseRenewalJob(LicenseService.Data.ApplicationDbContext context)
		{
			_context = context;
		}

		[AutomaticRetry(Attempts = 3)]
		public async Task ScheduleRenewal(Guid licenseId)
		{
			var license = await _context.LicenseApplications.FindAsync(licenseId);
			if (license?.Status == "Approved" && license.ExpiryDate <= DateTime.UtcNow.AddDays(30))
			{
				// Send renewal reminder
				BackgroundJob.Enqueue<NotificationJob>(x => x.SendRenewalNotification(licenseId));
			}
		}

		public async Task ProcessRenewals()
		{
			// Monthly renewal check for all licenses
			var expiringLicenses = await _context.LicenseApplications
				.Where(x => x.Status == "Approved" && x.ExpiryDate <= DateTime.UtcNow.AddDays(60))
				.ToListAsync();

			foreach (var license in expiringLicenses)
			{
				BackgroundJob.Enqueue<NotificationJob>(x => x.SendRenewalNotification(license.Id));
			}
		}
	}
}
