using Hangfire;
using LicenseManagement.Shared.Models;
using LicenseService.Jobs;
using MediatR;

namespace LicenseService.Features.Licenses.Commands
{
	public record CreateLicenseCommand(
		Guid TenantId,
		string LicenseType,
		string ApplicantName
	) : IRequest<Guid>;

	public class CreateLicenseCommandHandler : IRequestHandler<CreateLicenseCommand, Guid>
	{
		private readonly LicenseService.Data.ApplicationDbContext _context;

		public CreateLicenseCommandHandler(LicenseService.Data.ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Guid> Handle(CreateLicenseCommand request, CancellationToken cancellationToken)
		{
			var license = new LicenseApplication
			{
				Id = Guid.NewGuid(),
				TenantId = request.TenantId,
				LicenseType = request.LicenseType,
				ApplicantName = request.ApplicantName,
				ApplicationDate = DateTime.UtcNow,
				Status = "Pending"
			};

			_context.LicenseApplications.Add(license);
			await _context.SaveChangesAsync(cancellationToken);

			// Trigger renewal job (1 year from now)
			BackgroundJob.Enqueue<LicenseRenewalJob>(x => x.ScheduleRenewal(license.Id));

			return license.Id;
		}
	}
}
