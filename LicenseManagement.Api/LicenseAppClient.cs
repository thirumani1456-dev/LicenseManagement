using LicenseManagement.Api.Services;
using Microsoft.AspNetCore.Builder;
using System.Net.Http.Headers;
using System.Text.Json;

public class LicenseApiClient
{
	private readonly HttpClient _httpClient;
	//private const string DemoJwt ="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...."; 

	public LicenseApiClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
		_httpClient.BaseAddress = new Uri("http://localhost:7000"); // gateway
	}

	public async Task<List<LicenseViewModel>> GetLicensesAsync(string tenantId)
	{
		var request = new HttpRequestMessage(HttpMethod.Get, "/licenses");
		request.Headers.Add("X-Tenant-Id", tenantId);
		//request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", DemoJwt);
		var response = await _httpClient.SendAsync(request);
		response.EnsureSuccessStatusCode();
		var json = await response.Content.ReadAsStringAsync();
		return JsonSerializer.Deserialize<List<LicenseViewModel>>(json)!;
	}

	internal async Task CreateLicenseAsync(string tenantId, CreateLicenseViewModel model)
	{
		throw new NotImplementedException();
	}
}
