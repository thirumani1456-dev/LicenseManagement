using LicenseService.Data;
using LicenseService.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Tenant connection strings (adjust to your DB names)
var tenants = new Dictionary<string, string>
{
	{ "00000000-0000-0000-0000-000000000001", "Server=(localdb)\\MSSQLLocalDB;Database=LicenseDB_Tenant1;Trusted_Connection=True;" },
	{ "default", "Server=(localdb)\\MSSQLLocalDB;Database=LicenseDB_Default;Trusted_Connection=True;" }
};
builder.Services.AddSingleton(tenants);

// 3. HttpContextAccessor (needed in DbContext & controllers)
builder.Services.AddHttpContextAccessor();

// 4. DbContext – chooses connection string per request based on TenantId
builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
	var httpContext = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;
	var tenantId = httpContext?.Items["TenantId"]?.ToString() ?? "default";

	var cs = tenants.ContainsKey(tenantId) ? tenants[tenantId] : tenants["default"];
	options.UseSqlServer(cs);
});

/*var jwtSection = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSection["Key"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.RequireHttpsMetadata = false;
		options.SaveToken = true;
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = jwtSection["Issuer"],
			ValidAudience = jwtSection["Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(key)
		};
	});*/

builder.Services.AddAuthorization();



var app = builder.Build();

// 5. Swagger in development
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// 6. Our custom tenant middleware (reads X-Tenant-Id header)
app.UseTenantMiddleware();

app.UseAuthorization();

app.MapControllers();

// 7. Ensure databases/tables exist for each tenant (optional safety)
using (var scope = app.Services.CreateScope())
{
	var tenantConnections = scope.ServiceProvider.GetRequiredService<Dictionary<string, string>>();

	foreach (var kvp in tenantConnections)
	{
		var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
		optionsBuilder.UseSqlServer(kvp.Value);

		using var ctx = new ApplicationDbContext(optionsBuilder.Options);
		ctx.Database.EnsureCreated();
	}
}

app.Run();
