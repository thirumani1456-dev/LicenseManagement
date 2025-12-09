using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LicenseService.Features.Licenses.Commands;
using LicenseService.Features.Licenses.Queries;

namespace LicenseService.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	//app.UseHangfireDashboard("/hangfire");
	[Authorize(Roles = "AgencyAdmin,User")]
	public class LicensesController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly string _tenantId;

		public LicensesController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
		{
			_mediator = mediator;
			_tenantId = httpContextAccessor.HttpContext?.Items["TenantId"]?.ToString() ?? "default";
		}

		[HttpPost]
		public async Task<IActionResult> CreateLicense(CreateLicenseCommand command)
		{
			command = command with { TenantId = Guid.Parse(_tenantId) };
			var licenseId = await _mediator.Send(command);
			return Ok(new { Id = licenseId });
		}

		[HttpGet]
		public async Task<IActionResult> GetLicenses()
		{
			var licenses = await _mediator.Send(new GetLicensesQuery(Guid.Parse(_tenantId)));
			return Ok(licenses);
		}

		[HttpPost("{id}/approve")]
		[Authorize(Roles = "AgencyAdmin")]
		public async Task<IActionResult> ApproveLicense(Guid id)
		{
			var result = await _mediator.Send(new ApproveLicenseCommand(id, Guid.Parse(_tenantId)));
			return Ok();
			//return result ? Ok() : NotFound();
		}
	}
}
