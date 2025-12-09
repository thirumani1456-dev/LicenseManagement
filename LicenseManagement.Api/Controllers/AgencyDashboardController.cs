using LicenseManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LicenseManagement.Api.Controllers
{
	[Authorize(Policy = "AgencyAdmin")]
	public class AgencyDashboardController : Controller
	{
		private readonly LicenseApiClient _api;
		private const string TenantId = "00000000-0000-0000-0000-000000000001";

		public AgencyDashboardController(LicenseApiClient api)
		{
			_api = api;
		}

		public async Task<IActionResult> Index()
		{
			var licenses = await _api.GetLicensesAsync(TenantId);
			return View(licenses);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View(new CreateLicenseViewModel());
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateLicenseViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			await _api.CreateLicenseAsync(TenantId, model);
			return RedirectToAction(nameof(Index));
		}
	}
}
