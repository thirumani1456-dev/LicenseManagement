using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LicenseService.Middleware
{
	public class TenantMiddleware
	{
		private readonly RequestDelegate _next;

		public TenantMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var tenantId = context.Request.Headers["X-Tenant-Id"].FirstOrDefault()
						   ?? "00000000-0000-0000-0000-000000000001";
			context.Items["TenantId"] = tenantId;
			await _next(context);
		}
	}

	public static class TenantMiddlewareExtensions
	{
		public static IApplicationBuilder UseTenantMiddleware(this IApplicationBuilder app)
		{
			return app.UseMiddleware<TenantMiddleware>();
		}
	}
}
