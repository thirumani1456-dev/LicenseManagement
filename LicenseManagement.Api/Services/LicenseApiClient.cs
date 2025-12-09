using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace LicenseManagement.Api.Services
{
	public class LicenseApiClient
	{

		//private const string DemoJwt ="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...."; 
		private readonly HttpClient _httpClient;
		private readonly HttpClient _client;

		public LicenseApiClient(IHttpClientFactory factory)
		{
			_client = factory.CreateClient("GatewayClient");
		}
		public LicenseApiClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<List<LicenseViewModel>> GetLicensesAsync(string tenantId)
		{
			var request = new HttpRequestMessage(HttpMethod.Get, "/licenses");
			request.Headers.Add("X-Tenant-Id", tenantId);
			//request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", DemoJwt);

			var response = await _client.SendAsync(request);
			response.EnsureSuccessStatusCode();

			var data = await response.Content.ReadFromJsonAsync<List<LicenseViewModel>>();
			return data ?? new List<LicenseViewModel>();
		}


		/*public async Task<List<LicenseViewModel>> GetLicensesAsync(string tenantId)
		{
			var request = new HttpRequestMessage(HttpMethod.Get, "/licenses");
			request.Headers.Add("X-Tenant-Id", tenantId);
			var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();
			return (await response.Content.ReadFromJsonAsync<List<LicenseViewModel>>())!;
		}*/


		public async Task<Guid> CreateLicenseAsync(string tenantId, CreateLicenseViewModel model)
		{
			var request = new HttpRequestMessage(HttpMethod.Post, "/licenses");
			//request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", DemoJwt);
			request.Headers.Add("X-Tenant-Id", tenantId);
			request.Content = JsonContent.Create(model);
			var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();
			var result = await response.Content.ReadFromJsonAsync<LicenseCreatedResponse>();
			return result!.Id;
		}
	}

	public class LicenseViewModel
	{
		public Guid Id { get; set; }
		public string LicenseType { get; set; } = string.Empty;
		public string ApplicantName { get; set; } = string.Empty;
		public string Status { get; set; } = string.Empty;
		public DateTime? ExpiryDate { get; set; }
	}

	public class CreateLicenseViewModel
	{
		public string LicenseType { get; set; } = string.Empty;
		public string ApplicantName { get; set; } = string.Empty;
	}

	public class LicenseCreatedResponse
	{
		public Guid Id { get; set; }
	}
}
