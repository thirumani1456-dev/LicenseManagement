using LicenseManagement.Api.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace LicenseManagement.Api.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

	[HttpPost]
	public async Task<IActionResult> Login(string role)
	{
		// role = "AgencyAdmin" or "Licensee"
		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.Name, "DemoUser"),
			new Claim(ClaimTypes.Role, role)
		};

		var identity = new ClaimsIdentity(claims, "Dummy");
		var principal = new ClaimsPrincipal(identity);

		await HttpContext.SignInAsync("Dummy", principal);

		if (role == "AgencyAdmin")
			return RedirectToAction("Index", "AgencyDashboard");
		else
			return RedirectToAction("Index", "LicenseeDashboard");
	}

	[HttpGet]
	public IActionResult Login() => View();

	public async Task<IActionResult> Logout()
	{
		await HttpContext.SignOutAsync("Dummy");
		return RedirectToAction("Index");
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
