using System.Net.Http.Headers;
using LicenseManagement.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container

// MVC
builder.Services.AddControllersWithViews();

// Simple cookie auth + role policies (mocked for demo)
builder.Services.AddAuthentication("Dummy")
	.AddCookie("Dummy", options =>
	{
		options.LoginPath = "/Home/Login";
	});

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("AgencyAdmin", p => p.RequireRole("AgencyAdmin"));
	options.AddPolicy("Licensee", p => p.RequireRole("Licensee"));
});

// Named HttpClient for API Gateway
builder.Services.AddHttpClient("GatewayClient", client =>
{
	client.BaseAddress = new Uri("http://localhost:7000"); // Ocelot gateway URL
	client.DefaultRequestHeaders.Accept.Add(
		new MediaTypeWithQualityHeaderValue("application/json"));
});

// IHttpClientFactory support
builder.Services.AddHttpClient();

// Register LicenseApiClient so controllers can inject it
builder.Services.AddScoped<LicenseApiClient>();

// 2. Build app

var app = builder.Build();

// 3. Configure the HTTP request pipeline

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
