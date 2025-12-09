using LicenseManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LicenseManagement.Api.Controllers
{
	//[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	//[Authorize(Policy = "Licensee")]
	public class LicenseeDashboardController : Controller
	{

		private readonly LicenseApiClient _api;
		private const string TenantId = "00000000-0000-0000-0000-000000000001";
		private const string CurrentUserName = "John Doe";

		public LicenseeDashboardController(LicenseApiClient api)
		{
			_api = api;
		}

		public async Task<IActionResult> Index()
		{
			var all = await _api.GetLicensesAsync(TenantId);
			var mine = all.Where(x => x.ApplicantName == CurrentUserName).ToList();
			return View(mine);
		}
	}
}
