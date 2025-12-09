using Hangfire;
using LicenseService.Jobs;
using MediatR;

public record ApproveLicenseCommand(Guid Id, Guid TenantId) : IRequest<bool>;

public class ApproveLicenseCommandHandler : IRequestHandler<ApproveLicenseCommand, bool>
{
	private readonly LicenseService.Data.ApplicationDbContext _context;

	public ApproveLicenseCommandHandler(LicenseService.Data.ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<bool> Handle(ApproveLicenseCommand request, CancellationToken cancellationToken)
	{
		//var license = await _context.LicenseApplications
			//.FirstOrDefaultAsync(x => x.Id == request.Id && x.TenantId == request.TenantId);

		//if (license == null || license.Status != "Pending") return false;

		//license.Status = "Approved";
		//license.ExpiryDate = DateTime.UtcNow.AddYears(1);
		//await _context.SaveChangesAsync(cancellationToken);

		BackgroundJob.Enqueue<NotificationJob>(x => x.SendApprovalNotification(request.Id));
		return true;
	}
}
