using LicenseManagement.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace LicenseService.Features.Licenses.Queries
{
	public record GetLicensesQuery(Guid TenantId) : IRequest<List<LicenseApplication>>;

	public class GetLicensesQueryHandler : IRequestHandler<GetLicensesQuery, List<LicenseApplication>>
	{
		private readonly LicenseService.Data.ApplicationDbContext _context;

		public GetLicensesQueryHandler(LicenseService.Data.ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<LicenseApplication>> Handle(GetLicensesQuery request, CancellationToken cancellationToken)
		{
			return await _context.LicenseApplications
				.Where(x => x.TenantId == request.TenantId)
				.ToListAsync(cancellationToken);
		}
	}
}
